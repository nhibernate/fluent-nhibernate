using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Instances
{
    public class KeyManyToOneInstance : KeyManyToOneInspector, IKeyManyToOneInstance
    {
        private readonly KeyManyToOneMapping mapping;
        private bool nextBool = true;

        public KeyManyToOneInstance(KeyManyToOneMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public new IAccessInstance Access
        {
            get { return new AccessInstance(value => mapping.Set(x => x.Access, Layer.Conventions, value)); }
        }

        public new void ForeignKey(string name)
        {
            mapping.Set(x => x.ForeignKey, Layer.Conventions, name);
        }

        public void Lazy()
        {
            mapping.Set(x => x.Lazy, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new INotFoundInstance NotFound
        {
            get { return new NotFoundInstance(value => mapping.Set(x => x.NotFound, Layer.Conventions, value)); }
        }

        public IKeyManyToOneInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }
    }
}
