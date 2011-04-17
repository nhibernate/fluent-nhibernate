using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class AnyInstance : AnyInspector, IAnyInstance
    {
        private readonly AnyMapping mapping;

        public AnyInstance(AnyMapping mapping) : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IAccessInstance Access
        {
            get { return new AccessInstance(value => mapping.Set(x => x.Access, Layer.Conventions, value)); }
        }
    }
}