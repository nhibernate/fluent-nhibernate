using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.MappingModel.Output
{
    /// <summary>
    /// A two-stage mapping application strategy which first transforms a <see cref="HibernateMapping"/> into an intermediate
    /// form, and then applies the intermediate form to a <see cref="Configuration"/>. The stages are separated in order to
    /// address thread safety considerations; see the documentation for <see cref="ToIntermediateForm(HibernateMapping)"/> and
    /// <see cref="ApplyIntermediateFormToConfiguration(I, Configuration)"/> for details.
    /// </summary>
    /// <typeparam name="I">Intermediate type used by this strategy</typeparam>
    public abstract class MappingApplicationStrategyBase<I> : IMappingApplicationStrategy
    {
        protected MappingApplicationStrategyBase()
        {
        }

        /// <summary>
        /// Translate a <see cref="HibernateMapping"/> to the intermediate form. Implementations of this method must be
        /// thread-safe and should expect that they may run with a high degree of parallelism.
        /// </summary>
        /// <param name="mapping">a fluent Hibernate mapping</param>
        /// <returns>the intermediate translation of the input mapping</returns>
        protected abstract I ToIntermediateForm(HibernateMapping mapping);

        /// <summary>
        /// Apply a translated mapping to a <see cref="Configuration"/>. Implementations of this method are not required
        /// to be thread-safe, and may safely assume that calls to this method will be invoked serially.
        /// </summary>
        /// <param name="intermediateForm">a translated Hibernate mapping</param>
        /// <param name="cfg">the target configuration</param>
        protected abstract void ApplyIntermediateFormToConfiguration(I intermediateForm, Configuration cfg);

        public void ApplyMappingsToConfiguration(IEnumerable<HibernateMapping> mappings, Configuration cfg, int degreeOfParallelism)
        {
            foreach (var intermediate in mappings.Where(m => !m.Classes.Any()).AsParallel().AsOrdered().WithDegreeOfParallelism(degreeOfParallelism).Select(m => ToIntermediateForm(m)))
            {
                ApplyIntermediateFormToConfiguration(intermediate, cfg);
            }

            foreach (var mappingAndIntermediate in mappings.Where(m => m.Classes.Any()).AsParallel().AsOrdered().WithDegreeOfParallelism(degreeOfParallelism).Select(m => Tuple.Create(m, ToIntermediateForm(m))))
            {
                var mapping = mappingAndIntermediate.Item1;
                var intermediate = mappingAndIntermediate.Item2;
                if (cfg.GetClassMapping(mapping.Classes.First().Type) == null)
                    ApplyIntermediateFormToConfiguration(intermediate, cfg);
            }
        }
    }
}
