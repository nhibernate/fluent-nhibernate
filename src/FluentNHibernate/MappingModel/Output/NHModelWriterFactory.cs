using FluentNHibernate.Infrastructure;

namespace FluentNHibernate.MappingModel.Output
{
    public static class NHModelWriterFactory
    {
        private static readonly Container Container = new XmlWriterContainer();

        public static INHModelWriter<HibernateMapping> CreateHibernateMappingWriter()
        {
            return Container.Resolve<INHModelWriter<HibernateMapping>>();
        }
    }
}
