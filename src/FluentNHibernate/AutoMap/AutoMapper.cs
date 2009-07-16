using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.AutoMap
{
    public class AutoMapper
    {
        protected readonly List<IAutoMapper> mappingRules;
        protected List<AutoMapType> mappingTypes;
        protected AutoMappingExpressions expressions;

        public AutoMapper(AutoMappingExpressions expressions, IConventionFinder conventionFinder)
        {
            this.expressions = expressions;

            mappingRules = new List<IAutoMapper>
            {
                new AutoMapIdentity(expressions), 
                new AutoMapVersion(), 
                new AutoMapComponent(expressions),
                new AutoMapProperty(conventionFinder),
                new AutoMapManyToMany(expressions),
                new AutoMapManyToOne(),
                new AutoMapOneToMany(),
            };
        }

        private void ApplyOverrides(IDictionary<Type, Action<object>> inlineOverrides, Type classType, IList<string> mappedProperties, ClassMappingBase mapping)
        {
            if (inlineOverrides.ContainsKey(classType))
            {
                var autoMapType = typeof(AutoMap<>).MakeGenericType(classType);
                var autoMap = Activator.CreateInstance(autoMapType, mappedProperties);
                inlineOverrides[classType](autoMap);

                ((IAutoClasslike)autoMap).AlterModel(mapping);
            }
        }

        public ClassMappingBase MergeMap(Type classType, ClassMappingBase mapping, IDictionary<Type, Action<object>> inlineOverrides, IList<string> mappedProperties)
        {
            // map class first, then subclasses - this way subclasses can inspect the class model
            // to see which properties have already been mapped
            ApplyOverrides(inlineOverrides, classType, mappedProperties, mapping);

            MapEverythingInClass(mapping, classType, mappedProperties);

            if (mappingTypes != null)
                MapInheritanceTree(classType, inlineOverrides, mapping, mappedProperties);

            return mapping;
        }

        private void MapInheritanceTree(Type classType, IDictionary<Type, Action<object>> inlineOverrides, ClassMappingBase mapping, IList<string> mappedProperties)
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

                    ((ClassMapping)mapping).Discriminator = new DiscriminatorMapping((ClassMapping)mapping)
                    {
                        ColumnName = discriminatorColumn
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

                MapSubclass(classType, inlineOverrides, mappedProperties, subclassMapping, inheritedClass);

                mapping.AddSubclass(subclassMapping);

                MergeMap(inheritedClass.Type, (ClassMappingBase)subclassMapping, inlineOverrides, mappedProperties);
            }
        }

        private void MapSubclass(Type classType, IDictionary<Type, Action<object>> inlineOverrides, IList<string> mappedProperties, ISubclassMapping subclass, AutoMapType inheritedClass)
        {
            subclass.Name = inheritedClass.Type.AssemblyQualifiedName;
            ApplyOverrides(inlineOverrides, classType, mappedProperties, (ClassMappingBase)subclass);
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
                        if (mappedProperties.Count(p => p == property.Name) == 0)
                        {
                            if (mapping is ClassMapping)
                                rule.Map((ClassMapping)mapping, property);
                            else if (mapping is SubclassMapping)
                                rule.Map((SubclassMapping)mapping, property);
                            else if (mapping is JoinedSubclassMapping)
                                rule.Map((JoinedSubclassMapping)mapping, property);

                            mappedProperties.Add(property.Name);

                            break;
                        }
                    }
                }
            }
        }

        public ClassMapping Map(Type classType, List<AutoMapType> types, IDictionary<Type, Action<object>> overrides)
        {
            var classMap = new ClassMapping(classType);
            classMap.SetDefaultValue(x => x.Name, classType.AssemblyQualifiedName);
            mappingTypes = types;
            return (ClassMapping)MergeMap(classType, classMap, overrides, new List<string>());
        }
    }
}
