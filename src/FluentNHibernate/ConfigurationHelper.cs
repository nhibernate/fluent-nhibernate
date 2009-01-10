using System.Reflection;
using FluentNHibernate.AutoMap;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public static class ConfigurationHelper
    {
        public static Configuration AddMappingsFromAssembly(this Configuration configuration, Assembly assembly)
        {
            var models = new PersistenceModel();
            models.addMappingsFromAssembly(assembly);
            models.Configure(configuration);

            return configuration;
        }
        
        public static Configuration AddAutoMappings(this Configuration configuration, AutoPersistenceModel model)
        {
            model.Configure(configuration);

            return configuration;
        }
    }
}