using System;
using System.Reflection;

namespace FluentNHibernate.Automapping
{
    public static class AutoMap
    {
        public static AutoPersistenceModel AssemblyOf<T>()
        {
            return Assembly(typeof(T).Assembly);
        }

        public static AutoPersistenceModel AssemblyOf<T>(Func<Type, bool> where)
        {
            return Assembly(typeof(T).Assembly, where);
        }

        public static AutoPersistenceModel Assembly(Assembly assembly)
        {
            return Assembly(assembly, null);
        }

        public static AutoPersistenceModel Assembly(Assembly assembly, Func<Type, bool> where)
        {
            var persistenceModel = new AutoPersistenceModel();

            persistenceModel.AddEntityAssembly(assembly);

            if (where != null)
                persistenceModel.Where(where);

            return persistenceModel;
        }
    }
}