using FluentNHibernate.Infrastructure;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmConverterServiceLocator : IHbmConverterServiceLocator
    {
        private readonly Container container;

        public HbmConverterServiceLocator(Container container)
        {
            this.container = container;
        }

        public IHbmConverter<F, H> GetConverter<F, H>()
        {
            return container.Resolve<IHbmConverter<F, H>>();
        }
    }
}