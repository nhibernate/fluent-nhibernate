using System;
using System.Linq;
using System.Reflection;

namespace FluentNHibernate.AutoMap.Alterations
{
    /// <summary>
    /// Built-in alteration for altering an AutoPersistenceModel with instance of IAutoMappingOverride<T>.
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
        /// Finds all types in the assembly (passed in the constructor) that implement IAutoMappingOverride<T>, then
        /// creates an AutoMap<T> and applies the override to it.
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
                var mapping = (IMappingProvider)Activator.CreateInstance(typeof(AutoMap<>).MakeGenericType(typeMatch.EntityType));

                // HACK: call the Override method with the generic AutoMap<T>
                typeMatch.OverrideType
                    .GetMethod("Override")
                    .Invoke(mappingOverride, new[] { mapping });

                model.Add(mapping);
            }
        }
    }
}