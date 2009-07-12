using System;
using System.Linq.Expressions;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public class DynamicComponentMapping : ComponentMappingBase
    {
        public DynamicComponentMapping()
            : this(new AttributeStore())
        { }

        private DynamicComponentMapping(AttributeStore store)
            : base(store)
        {}

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessComponent(this);

            base.AcceptVisitor(visitor);
        }
    }
}