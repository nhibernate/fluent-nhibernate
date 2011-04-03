using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Conventions.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Conventions
{
    [Subject(typeof(IPropertyConvention))]
    public class when_a_formula_is_added_to_a_property
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Conventions.Add<FormulaConvention>();
            model.Add(new FormulaTargetMap());
        };

        Because of = () =>
            mapping = model.BuildMappingFor<FormulaTarget>();

        It should_remove_all_columns_from_the_property = () =>
            mapping.Properties.Single().Columns.ShouldBeEmpty();

        It should_add_the_formula_to_the_property = () =>
            mapping.Properties.Single().Formula.ShouldEqual(FormulaConvention.FormulaValue);
        
        static FluentNHibernate.PersistenceModel model;
        static ClassMapping mapping;
    }

    [Subject(typeof(IReferenceConvention))]
    public class when_a_formula_is_added_to_a_reference
    {
        Establish context = () =>
        {
            model = new FluentNHibernate.PersistenceModel();
            model.Conventions.Add<FormulaConvention>();
            model.Add(new FormulaTargetMap());
        };

        Because of = () =>
            mapping = model.BuildMappingFor<FormulaTarget>();

        It should_remove_all_columns_from_the_property = () =>
            mapping.References.Single().Columns.ShouldBeEmpty();

        It should_add_the_formula_to_the_property = () =>
            mapping.References.Single().Formula.ShouldEqual(FormulaConvention.FormulaValue);

        static FluentNHibernate.PersistenceModel model;
        static ClassMapping mapping;
    }
}
