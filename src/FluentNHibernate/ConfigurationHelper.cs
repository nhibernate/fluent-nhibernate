using System.Reflection;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public static class ConfigurationHelper
    {
        public static Configuration LoadMappingAssembly(this Configuration configuration, Assembly assembly)
        {
            var persistenceModel = new PersistenceModel();
            persistenceModel.addMappingsFromAssembly(assembly);
            persistenceModel.Configure(configuration);
            return configuration;
        }
    }
}