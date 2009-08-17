using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping
{
    public class AutoMapper
    {
        private readonly List<IAutoMapper> mappingRules;
        private List<AutoMapType> mappingTypes;
        private readonly AutoMappingExpressions expressions;
        private readonly IEnumerable<InlineOverride> inlineOverrides;

        public AutoMapper(AutoMappingExpressions expressions, IConventionFinder conventionFinder, IEnumerable<InlineOverride> inlineOverrides)
        {
            this.expressions = expressions;
            this.inlineOverrides = inlineOverrides;

            mappingRules = new List<IAutoMapper>
            {
                new AutoMapIdentity(expressions), 
                new AutoMapVersion(), 
                new AutoMapComponent(expressions, this),
                new AutoMapProperty(conventionFinder, expressions),
                new AutoMapManyToMany(expressions),
                new AutoMapManyToOne(),
                new AutoMapOneToMany(expressions),
            };
        }

        private void ApplyOverrides(Type classType, IList<string> mappedProperties, ClassMappingBase mapping)
        {
            var autoMapType = typeof(AutoMapping<>).MakeGenericType(classType);
            var autoMap = Activator.CreateInstance(autoMapType, mappedProperties);

            inlineOverrides
                .Where(x => x.CanOverride(classType))
                .Each(x => x.Apply(autoMap));

            ((IAutoClasslike)autoMap).AlterModel(mapping);
        }

        public ClassMappingBase MergeMap(Type classType, ClassMappingBase mapping, IList<string> mappedProperties)
        {
            // map class first, then subclasses - this way subclasses can inspect the class model
            // to see which properties have already been mapped
            ApplyOverrides(classType, mappedProperties, mapping);

            MapEverythingInClass(mapping, classType, mappedProperties);

            if (mappingTypes != null)
                MapInheritanceTree(classType, mapping, mappedProperties);

            return mapping;
        }

        private void MapInheritanceTree(Type classType, ClassMappingBase mapping, IList<string> mappedProperties)
        {
            var discriminatorSet = false;
            var isDiscriminated = expressions.IsDiscriminated(classType);

            foreach (var inheritedClass in mappingTypes.Where(q =>
                q.Type.BaseType == classType &&
                    !expressions.IsConcreteBaseType(q.Type.BaseType)))
            {
                if (isDiscriminated && !discriminatorSet && mapping is ClassMapping)
                {
                    var discriminatorColumn = expressions.DiscriminatorColumn(classType);

                    ((ClassMapping)mapping).Discriminator = new DiscriminatorMapping
                    {
                        Column = discriminatorColumn,
                        ContainingEntityType = classType
                    };
                    discriminatorSet = true;
                }

                ISubclassMapping subclassMapping;
                var subclassStrategy = expressions.SubclassStrategy(classType);

                if (subclassStrategy == SubclassStrategy.JoinedSubclass)
                {
                    // TODO: This id name should be removed. Ideally it needs to be set by a
                    // default and be overridable by a convention (preferably the ForeignKey convention
                    // that already exists)
                    var subclass = new JoinedSubclassMapping
                    {
                        Type = inheritedClass.Type
                    };

                    subclass.Key = new KeyMapping();
                    subclass.Key.AddDefaultColumn(new ColumnMapping { Name = mapping.Type.Name + "_id" });

                    subclassMapping = subclass;
                }
                else
                    subclassMapping = new SubclassMapping();

                MapSubclass(classType, mappedProperties, subclassMapping, inheritedClass);

                mapping.AddSubclass(subclassMapping);

                MergeMap(inheritedClass.Type, (ClassMappingBase)subclassMapping, mappedProperties);
            }
        }

        private void MapSubclass(Type classType, IList<string> mappedProperties, ISubclassMapping subclass, AutoMapType inheritedClass)
        {
            subclass.Name = inheritedClass.Type.AssemblyQualifiedName;
            ApplyOverrides(classType, mappedProperties, (ClassMappingBase)subclass);
            MapEverythingInClass((ClassMappingBase)subclass, inheritedClass.Type, mappedProperties);
            inheritedClass.IsMapped = true;
        }

        public virtual void MapEverythingInClass(ClassMappingBase mapping, Type entityType, IList<string> mappedProperties)
        {
            foreach (var property in entityType.GetProperties())
            {
                TryToMapProperty(mapping, property, mappedProperties);
            }
        }

        protected void TryToMapProperty(ClassMappingBase mapping, PropertyInfo property, IList<string> mappedProperties)
        {
            if (property.GetIndexParameters().Length == 0)
            {
                foreach (var rule in mappingRules)
                {
                    if (rule.MapsProperty(property))
                    {
                        if (mappedProperties.Count(name => name == property.Name) == 0)
                        {
                            rule.Map(mapping, property);
                            mappedProperties.Add(property.Name);

                            break;
                        }
                    }
                }
            }
        }

        public ClassMapping Map(Type classType, List<AutoMapType> types)
        {
            var classMap = new ClassMapping { Type = classType };

            classMap.SetDefaultValue(x => x.Name, classType.AssemblyQualifiedName);
            classMap.SetDefaultValue(x => x.TableName, GetDefaultTableName(classType));

            mappingTypes = types;
            return (ClassMapping)MergeMap(classType, classMap, new List<string>());
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
