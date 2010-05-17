using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping
{
    public class AutoMapper
    {
        List<AutoMapType> mappingTypes;
        readonly IAutomappingConfiguration cfg;
        readonly IConventionFinder conventionFinder;
        readonly IEnumerable<InlineOverride> inlineOverrides;

        public AutoMapper(IAutomappingConfiguration cfg, IConventionFinder conventionFinder, IEnumerable<InlineOverride> inlineOverrides)
        {
            this.cfg = cfg;
            this.conventionFinder = conventionFinder;
            this.inlineOverrides = inlineOverrides;
        }

        private void ApplyOverrides(Type classType, IList<Member> mappedMembers, ClassMappingBase mapping)
        {
            var autoMapType = typeof(AutoMapping<>).MakeGenericType(classType);
            var autoMap = Activator.CreateInstance(autoMapType, mappedMembers);

            inlineOverrides
                .Where(x => x.CanOverride(classType))
                .Each(x => x.Apply(autoMap));

            ((IAutoClasslike)autoMap).AlterModel(mapping);
        }

        public ClassMappingBase MergeMap(Type classType, ClassMappingBase mapping, IList<Member> mappedMembers)
        {
            // map class first, then subclasses - this way subclasses can inspect the class model
            // to see which properties have already been mapped
            ApplyOverrides(classType, mappedMembers, mapping);

            ProcessClass(mapping, classType, mappedMembers);

            if (mappingTypes != null)
                MapInheritanceTree(classType, mapping, mappedMembers);

            return mapping;
        }

        private void MapInheritanceTree(Type classType, ClassMappingBase mapping, IList<Member> mappedMembers)
        {
            var discriminatorSet = HasDiscriminator(mapping);
            var isDiscriminated = cfg.IsDiscriminated(classType) || discriminatorSet;
            var mappingTypesWithLogicalParents = GetMappingTypesWithLogicalParents();

            foreach (var inheritedClass in mappingTypesWithLogicalParents
                .Where(x => x.Value != null && x.Value.Type == classType)
                .Select(x => x.Key))
            {
                if (isDiscriminated && !discriminatorSet && mapping is ClassMapping)
                {
                    var discriminatorColumn = cfg.GetDiscriminatorColumn(classType);
                    var discriminator = new DiscriminatorMapping
                    {
                        ContainingEntityType = classType,
                        Type = new TypeReference(typeof(string))
                    };
                    discriminator.AddDefaultColumn(new ColumnMapping { Name = discriminatorColumn });

                    ((ClassMapping)mapping).Discriminator = discriminator;
                    discriminatorSet = true;
                }

                SubclassMapping subclassMapping;

                if (!isDiscriminated)
                {
                    subclassMapping = new SubclassMapping(SubclassType.JoinedSubclass)
                    {
                        Type = inheritedClass.Type
                    };
                    subclassMapping.Key = new KeyMapping();
                    subclassMapping.Key.AddDefaultColumn(new ColumnMapping { Name = mapping.Type.Name + "_id" });
                }
                else
                    subclassMapping = new SubclassMapping(SubclassType.Subclass)
                    {
                        Type = inheritedClass.Type
                    };

				// track separate set of properties for each sub-tree within inheritance hierarchy
            	var subclassMembers = new List<Member>(mappedMembers);
				MapSubclass(subclassMembers, subclassMapping, inheritedClass);

                mapping.AddSubclass(subclassMapping);

				MergeMap(inheritedClass.Type, subclassMapping, subclassMembers);
            }
        }

        bool HasDiscriminator(ClassMappingBase mapping)
        {
            if (mapping is ClassMapping && ((ClassMapping)mapping).Discriminator != null)
                return true;

            return false;
        }

        Dictionary<AutoMapType, AutoMapType> GetMappingTypesWithLogicalParents()
        {
            var excludedTypes = mappingTypes
                .Where(x => cfg.IsConcreteBaseType(x.Type.BaseType))
                .ToArray();
            var availableTypes = mappingTypes.Except(excludedTypes);
            var mappingTypesWithLogicalParents = new Dictionary<AutoMapType, AutoMapType>();

            foreach (var type in availableTypes)
                mappingTypesWithLogicalParents.Add(type, GetLogicalParent(type.Type, availableTypes));
            return mappingTypesWithLogicalParents;
        }

        AutoMapType GetLogicalParent(Type type, IEnumerable<AutoMapType> availableTypes)
        {
            if (type.BaseType == typeof(object) || type.BaseType == null)
                return null;

            var baseType = availableTypes.FirstOrDefault(x => x.Type == type.BaseType);

            if (baseType != null)
                return baseType;

            return GetLogicalParent(type.BaseType, availableTypes);
        }

        private void MapSubclass(IList<Member> mappedMembers, SubclassMapping subclass, AutoMapType inheritedClass)
        {
            subclass.Name = inheritedClass.Type.AssemblyQualifiedName;
            subclass.Type = inheritedClass.Type;
            ApplyOverrides(inheritedClass.Type, mappedMembers, subclass);
            ProcessClass(subclass, inheritedClass.Type, mappedMembers);
            inheritedClass.IsMapped = true;
        }

        public virtual void ProcessClass(ClassMappingBase mapping, Type entityType, IList<Member> mappedMembers)
        {
            entityType.GetInstanceMembers()
                .Where(cfg.ShouldMap)
                .Each(x => TryMapProperty(mapping, x, mappedMembers));
        }

        void TryMapProperty(ClassMappingBase mapping, Member member, IList<Member> mappedMembers)
        {
            if (member.HasIndexParameters) return;

            foreach (var rule in cfg.GetMappingSteps(this, conventionFinder))
            {
                if (!rule.ShouldMap(member)) continue;
                if (mappedMembers.Contains(member)) continue;

                rule.Map(mapping, member);
                mappedMembers.Add(member);

                break;
            }
        }

        public ClassMapping Map(Type classType, List<AutoMapType> types)
        {
            var classMap = new ClassMapping { Type = classType };

            classMap.SetDefaultValue(x => x.Name, classType.AssemblyQualifiedName);
            classMap.SetDefaultValue(x => x.TableName, GetDefaultTableName(classType));

            mappingTypes = types;
            return (ClassMapping)MergeMap(classType, classMap, new List<Member>());
        }

        private string GetDefaultTableName(Type type)
        {
            var tableName = type.Name;

            if (type.IsGenericType)
            {
                // special case for generics: GenericType_GenericParameterType
                tableName = type.Name.Substring(0, type.Name.IndexOf('`'));

                foreach (var argument in type.GetGenericArguments())
                {
                    tableName += "_";
                    tableName += argument.Name;
                }
            }

            return "`" + tableName + "`";
        }

        /// <summary>
        /// Flags a type as already mapped, stop it from being auto-mapped.
        /// </summary>
        public void FlagAsMapped(Type type)
        {
            mappingTypes
                .Where(x => x.Type == type)
                .Each(x => x.IsMapped = true);
        }
    }
}
