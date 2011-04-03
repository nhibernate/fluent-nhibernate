using FluentNHibernate.Mapping;

namespace FluentNHibernate.Specs.Conventions.Fixtures
{
    public class FormulaTarget
    {
        public int Id { get; private set; }
        public string Prop { get; private set; }
        public FormulaTarget Target { get; private set; }
    }

    public class FormulaTargetMap : ClassMap<FormulaTarget>
    {
        public FormulaTargetMap()
        {
            Id(x => x.Id);
            Map(x => x.Prop, "a-column");
            References(x => x.Target);
        }
    }
}