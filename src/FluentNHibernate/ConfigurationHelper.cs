using System.Reflection;
using FluentNHibernate.Mapping;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public static class ConfigurationHelper
    {
        public static void AddMappingsFromAssembly(this Configuration configuration, Assembly assembly)
        {
            var models = new PersistenceModel();
            models.addMappingsFromAssembly(assembly);
            models.Configure(configuration);   
        }
        
    }
}