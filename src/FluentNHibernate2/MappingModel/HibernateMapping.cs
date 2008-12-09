using System.Collections;
using System.Linq.Expressions;
using NHibernate.Cfg.MappingSchema;
using System.Collections.Generic;
using System;
using System.Linq;

namespace FluentNHibernate.MappingModel
{
    public class HibernateMapping : MappingBase<HbmMapping>
    {
        private readonly IList<ClassMapping> _classes;

        public HibernateMapping()
        {
            _classes = new List<ClassMapping>();
        }

        public IEnumerable<ClassMapping> Classes
        {
            get { return _classes; }
        }

        public bool DefaultLazy
        {
            get { return _hbm.defaultlazy; }
            set { _hbm.defaultlazy = value; }
        }

        public void AddClass(ClassMapping classMapping)
        {
            _classes.Add(classMapping);            
            classMapping.Hbm.AddTo(ref _hbm.Items);
        }
    }

    internal static class Extensions
    {        
        public static void AddTo<T>(this T item, ref T[] array)
        {
            array = new List<T>(array ?? Enumerable.Empty<T>()) { item }.ToArray();
        }

    }
}