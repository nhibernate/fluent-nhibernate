using FluentNHibernate.MappingModel;

namespace FluentNHibernate.FluentInterface
{
    public class ManyToOnePart
    {
        private readonly ManyToOneMapping _mapping;

        public ManyToOnePart(ManyToOneMapping mapping)
        {
            _mapping = mapping;
        }


        public ManyToOnePart CanNotBeNull()
        {
            _mapping.IsNotNullable = true;
            return this;
        }
    }
}