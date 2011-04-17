using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Instances
{
    public class CompositeIdentityInstance : CompositeIdentityInspector, ICompositeIdentityInstance
    {
        private readonly CompositeIdMapping mapping;
        private bool nextBool = true;


        public CompositeIdentityInstance(CompositeIdMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }


        public new void UnsavedValue(string unsavedValue)
        {
            mapping.Set(x => x.UnsavedValue, Layer.Conventions, unsavedValue);
        }

        public new IAccessInstance Access
        {
            get { return new AccessInstance(value => mapping.Set(x => x.Access, Layer.Conventions, value)); }
        }

        public new void Mapped()
        {
            mapping.Set(x => x.Mapped, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public ICompositeIdentityInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public new IEnumerable<IKeyPropertyInstance> KeyProperties
        {
            get
            {
                return mapping.Keys
                    .Where(x => x is KeyPropertyMapping)
                    .Select(x => new KeyPropertyInstance((KeyPropertyMapping)x))
                    .Cast<IKeyPropertyInstance>();
            }
        }

        public new IEnumerable<IKeyManyToOneInstance> KeyManyToOnes
        {
            get
            {
                return mapping.Keys
                    .Where(x => x is KeyManyToOneMapping)
                    .Select(x => new KeyManyToOneInstance((KeyManyToOneMapping)x))
                    .Cast<IKeyManyToOneInstance>();
            }
        }
    }
}
