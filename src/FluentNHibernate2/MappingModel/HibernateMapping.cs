using System.Collections;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg.MappingSchema;
using System.Collections.Generic;
using System;

namespace FluentNHibernate.MappingModel
{
    public class HibernateMapping : MappingBase
    {
        private readonly IList<ClassMapping> _classes;
        private readonly AttributeStore<HibernateMapping> _attributes;

        public HibernateMapping()
        {
            _attributes = new AttributeStore<HibernateMapping>();
            _classes = new List<ClassMapping>();
        }        

        public IEnumerable<ClassMapping> Classes
        {
            get { return _classes; }
        }

        public void AddClass(ClassMapping classMapping)
        {
            _classes.Add(classMapping);            
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessHibernateMapping(this);

            foreach (var classMapping in Classes)
                visitor.ProcessClass(classMapping);
        }

        public bool DefaultLazy
        {
            get { return _attributes.Get(x => x.DefaultLazy); }
            set { _attributes.Set(x => x.DefaultLazy, value); }
        }
    }
}