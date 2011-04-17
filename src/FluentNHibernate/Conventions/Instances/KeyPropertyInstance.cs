using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Instances
{
    public class KeyPropertyInstance : KeyPropertyInspector, IKeyPropertyInstance
    {
        private readonly KeyPropertyMapping mapping;

        public KeyPropertyInstance(KeyPropertyMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IAccessInstance Access
        {
            get { return new AccessInstance(value => mapping.Set(x => x.Access, Layer.Conventions, value)); }
        }

        public new void Length(int length)
        {
            mapping.Set(x => x.Length, Layer.Conventions, length);
        }
    }
}
