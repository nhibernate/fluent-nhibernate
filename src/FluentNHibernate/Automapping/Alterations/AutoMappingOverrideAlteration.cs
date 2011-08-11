using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Alterations
{
    /// <summary>
    /// Built-in alteration for altering an AutoPersistenceModel with instance of IAutoMappingOverride&lt;T&gt;.
    /// </summary>
    public class AutoMappingOverrideAlteration : IAutoMappingAlteration
    {
        private readonly Assembly assembly;

        /// <summary>
        /// Constructor for AutoMappingOverrideAlteration.
        /// </summary>
        /// <param name="overrideAssembly">Assembly to load overrides from.</param>
        public AutoMappingOverrideAlteration(Assembly overrideAssembly)
        {
            assembly = overrideAssembly;
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
            // find all types deriving from IAutoMappingOverride<T>
            var types = from type in assembly.GetExportedTypes()
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