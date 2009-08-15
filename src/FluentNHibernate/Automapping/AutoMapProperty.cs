using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public class AutoMapProperty : IAutoMapper
    {
        private readonly IConventionFinder conventionFinder;
        private readonly AutoMappingExpressions expressions;

        public AutoMapProperty(IConventionFinder conventionFinder, AutoMappingExpressions expressions)
        {
            this.conventionFinder = conventionFinder;
            this.expressions = expressions;
        }

        public bool MapsProperty(PropertyInfo property)
        {
            if (HasExplicitTypeConvention(property))
                return true;

            if (property.CanWrite)
                return IsMappableToColumnType(property);

            return false;
        }

        private bool HasExplicitTypeConvention(PropertyInfo property)
        {
            // todo: clean this up!
            //        What it's doing is finding if there are any IUserType conventions
            //        that would be applied to this property, if there are then we should
            //        definitely automap it. The nasty part is that right now we don't have
            //        a model, so we're having to create a fake one so the convention will
            //        apply to it.
            var conventions = conventionFinder
                .Find<IPropertyConvention>()
                .Where(c =>
                {
                    if (!typeof(IUserTypeConvention).IsAssignableFrom(c.GetType()))
                        return false;

                    var criteria = new ConcreteAcceptanceCriteria<IPropertyInspector>();
                    var acceptance = c as IConventionAcceptance<IPropertyInspector>;
                    
                    if (acceptance != null)
                        acceptance.Accept(criteria);

                    return criteria.Matches(new PropertyInspector(new PropertyMapping
                    {
                        Type = new TypeReference(property.PropertyType),
                        PropertyInfo = property
                    }));
                });

            return conventions.FirstOrDefault() != null;
        }

        private static bool IsMappableToColumnType(PropertyInfo property)
        {
            return property.PropertyType.Namespace == "System"
                    || property.PropertyType.FullName == "System.Drawing.Bitmap"
                    || property.PropertyType.IsEnum;
        }

        public void Map(ClassMappingBase classMap, PropertyInfo property)
        {
            classMap.AddProperty(GetPropertyMapping(classMap.Type, property, classMap as ComponentMapping));
        }

        private PropertyMapping GetPropertyMapping(Type type, PropertyInfo property, ComponentMapping component)
        {
            var mapping = new PropertyMapping
            {
                ContainingEntityType = type,
                PropertyInfo = property
            };

            var columnName = property.Name;
            
            if (component != null)
                columnName = expressions.GetComponentColumnPrefix(component.PropertyInfo) + columnName;

            mapping.AddDefaultColumn(new ColumnMapping { Name = columnName });

            if (!mapping.IsSpecified(x => x.Name))
                mapping.Name = mapping.PropertyInfo.Name;

            if (!mapping.IsSpecified(x => x.Type))
                mapping.SetDefaultValue(x => x.Type, new TypeReference(mapping.PropertyInfo.PropertyType));

            return mapping;
        }
    }
}