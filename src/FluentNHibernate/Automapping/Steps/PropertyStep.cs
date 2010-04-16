using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Steps
{
    public class PropertyStep : IAutomappingStep
    {
        private readonly IConventionFinder conventionFinder;
        private readonly IAutomappingConfiguration cfg;

        public PropertyStep(IConventionFinder conventionFinder, IAutomappingConfiguration cfg)
        {
            this.conventionFinder = conventionFinder;
            this.cfg = cfg;
        }

        public bool ShouldMap(Member member)
        {
            if (HasExplicitTypeConvention(member))
                return true;

            if (member.CanWrite)
                return IsMappableToColumnType(member);

            return false;
        }

        private bool HasExplicitTypeConvention(Member property)
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
                        Member = property
                    }));
                });

            return conventions.FirstOrDefault() != null;
        }

        private static bool IsMappableToColumnType(Member property)
        {
            return property.PropertyType.Namespace == "System"
                    || property.PropertyType.FullName == "System.Drawing.Bitmap"
                    || property.PropertyType.IsEnum;
        }

        public void Map(ClassMappingBase classMap, Member property)
        {
            classMap.AddProperty(GetPropertyMapping(classMap.Type, property, classMap as ComponentMapping));
        }

        private PropertyMapping GetPropertyMapping(Type type, Member property, ComponentMapping component)
        {
            var mapping = new PropertyMapping
            {
                ContainingEntityType = type,
                Member = property
            };

            var columnName = property.Name;
            
            if (component != null)
                columnName = cfg.GetComponentColumnPrefix(component.Member) + columnName;

            mapping.AddDefaultColumn(new ColumnMapping { Name = columnName });

            if (!mapping.IsSpecified("Name"))
                mapping.Name = mapping.Member.Name;

            if (!mapping.IsSpecified("Type"))
                mapping.SetDefaultValue("Type", GetDefaultType(property));

            return mapping;
        }

        private TypeReference GetDefaultType(Member property)
        {
            var type = new TypeReference(property.PropertyType);

            if (property.PropertyType.IsEnum())
                type = new TypeReference(typeof(GenericEnumMapper<>).MakeGenericType(property.PropertyType));

            if (property.PropertyType.IsNullable() && property.PropertyType.IsEnum())
                type = new TypeReference(typeof(GenericEnumMapper<>).MakeGenericType(property.PropertyType.GetGenericArguments()[0]));

            return type;
        }

    }
}