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
                        let entity = (from interfaceType in type.GetInterfaces()
                                      where interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IAutoMappingOverride<>)
                                      select interfaceType.GetGenericArguments()[0]).FirstOrDefault()
                        where entity != null
                        select new { OverrideType = type, EntityType = entity };

            foreach (var typeMatch in types)
            {
                var mappingOverride = Activator.CreateInstance(typeMatch.OverrideType);
                var autoMapType = typeof(AutoMapping<>).MakeGenericType(typeMatch.EntityType);
                var mapping = (IMappingProvider)Activator.CreateInstance(autoMapType, new List<string>());

                // HACK: call the Override method with the generic AutoMapping<T>
                var overrideMethod = typeMatch.OverrideType
                    .GetMethod("Override");

                GetType()
                    .GetMethod("AddOverride", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(typeMatch.EntityType)
                    .Invoke(this, new[] { model, typeMatch.EntityType, mappingOverride });
            }
        }

        private void AddOverride<T>(AutoPersistenceModel model, Type entity, IAutoMappingOverride<T> mappingOverride)
        {
            model.AddOverride(entity, x =>
            {
                if (x is AutoMapping<T>)
                    mappingOverride.Override((AutoMapping<T>)x);
            });
        }
    }
}