using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions;
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
                new AutoMapColumn(conventionFinder),
                new ManyToManyAutoMapper(expressions),
                new AutoMapManyToOne(),
                new AutoMapOneToMany(),
            };
        }

        public AutoMap<T> MergeMap<T>(AutoMap<T> map, IEnumerable<InlineOverride> inlineOverrides)
        {
            if (mappingTypes != null)
            {
				var discriminatorSet = false;
				var isDiscriminated = expressions.IsDiscriminated(typeof(T));

                foreach (var inheritedClass in mappingTypes.Where(q =>
                    q.Type.BaseType == typeof(T) &&
                    !expressions.IsConcreteBaseType(q.Type.BaseType)))
                {
                    if (isDiscriminated && !discriminatorSet)
                    {
						var discriminator = map.Discriminator;
                        var discriminatorColumn = expressions.DiscriminatorColumn(typeof(T));
						if (discriminator == null)
						{
							discriminator = map.DiscriminateSubClassesOnColumn(discriminatorColumn);
						}
						else
						{
							var discriminatorMapping = discriminator.GetDiscriminatorMapping();
							if (string.IsNullOrEmpty(discriminatorMapping.ColumnName))
							{
								discriminatorMapping.ColumnName = discriminatorColumn;
							}
						}
                        discriminatorSet = true;
                    }

					object subclassMapping;
                    var subclassStrategy = expressions.SubclassStrategy(typeof(T));

                    if (subclassStrategy == SubclassStrategy.JoinedSubclass)
                    {
                        var subclass = map.JoinedSubClass(inheritedClass.Type, typeof(T).Name + "_id");

                        inlineOverrides
                            .Where(x => x.CanOverride(subclass.EntityType))
                            .Each(x => x.Apply(subclass));

                        var method = GetType().GetMethod("MapEverythingInClass");
                        var genericMethod = method.MakeGenericMethod(inheritedClass.Type);
                        genericMethod.Invoke(this, new[] { subclass });
                        inheritedClass.IsMapped = true;
						subclassMapping = subclass;
                    }
                    else
                    {
                        var subclass = map.SubClass(inheritedClass.Type, inheritedClass.Type.Name);

                        inlineOverrides
                            .Where(x => x.CanOverride(subclass.EntityType))
                            .Each(x => x.Apply(subclass));

                        var method = GetType().GetMethod("MapEverythingInClass");
                        var genericMethod = method.MakeGenericMethod(inheritedClass.Type);
                        genericMethod.Invoke(this, new[] {subclass});
                        inheritedClass.IsMapped = true;
						subclassMapping = subclass;
                    }

					InvocationHelper.InvokeGenericMethodWithDynamicTypeArguments(
						this, a => a.MergeMap<object>(null, null), new[] { subclassMapping, inlineOverrides },
						inheritedClass.Type);
                }
            }

            inlineOverrides
                .Where(x => x.CanOverride(typeof(T)))
                .Each(x => x.Apply(map));

            MapEverythingInClass(map);
            
            return map;
        }

        public virtual void MapEverythingInClass<T>(AutoMap<T> map)
        {
            foreach (var property in typeof(T).GetProperties())
            {
                TryToMapProperty(map, property);
            }
        }

        protected void TryToMapProperty<T>(AutoMap<T> map, PropertyInfo property)
        {
            if (property.GetIndexParameters().Length == 0)
            {
                foreach (var rule in mappingRules)
                {
                    if (rule.MapsProperty(property))
                    {
                        if (map.PropertiesMapped.Count(name => name == property.Name) == 0)
                        {
                            rule.Map(map, property);
                            break;
                        }
                    }
                }
            }
        }

        public AutoMap<T> Map<T>(List<AutoMapType> types, IEnumerable<InlineOverride> overrides)
        {
            var classMap = (AutoMap<T>)Activator.CreateInstance(typeof(AutoMap<T>));
            mappingTypes = types;
            return MergeMap(classMap, overrides);
        }
    }
}
