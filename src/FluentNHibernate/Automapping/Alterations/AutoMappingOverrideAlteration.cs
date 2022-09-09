using System.Linq;
using System.Reflection;

namespace FluentNHibernate.Automapping.Alterations
{
    /// <summary>
    /// Built-in alteration for altering an AutoPersistenceModel with instance of IAutoMappingOverride&lt;T&gt;.
    /// </summary>
    public class AutoMappingOverrideAlteration : IAutoMappingAlteration
    {
        private readonly Assembly assembly;
        private readonly bool includeInternal;

        /// <summary>
        /// Constructor for AutoMappingOverrideAlteration.
        /// </summary>
        /// <param name="overrideAssembly">Assembly to load overrides from.</param>
        /// <param name="includeInternal">Should internal IAutoMappingOverrides be included.</param>
        public AutoMappingOverrideAlteration(Assembly overrideAssembly, bool includeInternal)
        {
            assembly = overrideAssembly;
            this.includeInternal = includeInternal;
        }

        /// <summary>
        /// Alter the model
        /// </summary>
        /// <remarks>
        /// Finds all types in the assembly (passed in the constructor) that implement IAutoMappingOverride&lt;T&gt;, then
        /// creates an AutoMapping&lt;T&gt; and applies the override to it.
        /// </remarks>
        /// <param name="model">AutoPersistenceModel instance to alter</param>
        public void Alter(AutoPersistenceModel model)
        {
            var assemblyTypes = includeInternal ? assembly.GetTypes() : assembly.GetExportedTypes();
            // find all types deriving from IAutoMappingOverride<T>
            var types = from type in assemblyTypes
                        where !type.IsAbstract
                        let entity = (from interfaceType in type.GetInterfaces()
                                      where interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IAutoMappingOverride<>)
                                      select interfaceType.GetGenericArguments()[0]).FirstOrDefault()
                        where entity != null
                        select type;

            foreach (var type in types)
            {
            	model.Override(type);
            }
        }
    }
}