using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Automapping.Alterations;

namespace FluentNHibernate.Automapping
{
    public class AutoMappingAlterationCollection : IEnumerable<IAutoMappingAlteration>
    {
        private readonly List<IAutoMappingAlteration> alterations = new List<IAutoMappingAlteration>();

        /// <summary>
        /// Creates an instance of an IAutoMappingAlteration from a type instance, then adds it to the alterations collection.
        /// </summary>
        /// <param name="type">Type of an IAutoMappingAlteration</param>
        private void Add(Type type)
        {
            Add((IAutoMappingAlteration)Activator.CreateInstance(type));
        }

        /// <summary>
        /// Creates an instance of an IAutoMappingAlteration from a generic type parameter, then adds it to the alterations collection.
        /// </summary>
        /// <typeparam name="T">Type of an IAutoMappingAlteration</typeparam>
        /// <returns>Container</returns>
        public AutoMappingAlterationCollection Add<T>() where T : IAutoMappingAlteration
        {
            Add(typeof(T));
            return this;
        }

        /// <summary>
        /// Adds an alteration
        /// </summary>
        /// <param name="alteration">Alteration to add</param>
        /// <returns>Container</returns>
        public AutoMappingAlterationCollection Add(IAutoMappingAlteration alteration)
        {
            if (!alterations.Exists(a => a.GetType() == alteration.GetType()))
                alterations.Add(alteration);
            return this;
        }

        /// <summary>
        /// Adds all alterations from an assembly
        /// </summary>
        /// <param name="assembly">Assembly to search</param>
        /// <returns>Container</returns>
        public AutoMappingAlterationCollection AddFromAssembly(Assembly assembly)
        {
            foreach (var type in assembly.GetExportedTypes())
            {
                if (typeof(IAutoMappingAlteration).IsAssignableFrom(type))
                    Add(type);
            }
            
            return this;
        }

        /// <summary>
        /// Adds all alterations from an assembly that contains T.
        /// </summary>
        /// <typeparam name="T">Type who's assembly to search</typeparam>
        /// <returns>Container</returns>
        public AutoMappingAlterationCollection AddFromAssemblyOf<T>()
        {
            return AddFromAssembly(typeof(T).Assembly);
        }

        /// <summary>
        /// Apply alterations to an AutoPersisteceModel
        /// </summary>
        /// <param name="model">AutoPersistenceModel instance to apply alterations to</param>
        protected internal void Apply(AutoPersistenceModel model)
        {
            foreach (var alteration in alterations)
            {
                alteration.Alter(model);
            }
        }

        public IEnumerator<IAutoMappingAlteration> GetEnumerator()
        {
            return alterations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}