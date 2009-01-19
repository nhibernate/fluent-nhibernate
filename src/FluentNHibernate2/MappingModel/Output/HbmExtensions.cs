using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.MappingModel.Output
{
    internal static class HbmExtensions
    {        
        public static void AddTo<T>(this T item, ref T[] array)
        {
            array = new List<T>(array ?? Enumerable.Empty<T>()) { item }.ToArray();
        }

    }
}