using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel
{
    public class HibernateMapping : MappingBase
    {
        private readonly IList<ClassMapping> classes;
        private readonly IList<ImportMapping> imports;
        private readonly AttributeStore<HibernateMapping> attributes;

        public HibernateMapping()
            : this(new AttributeStore())
        {}

        public HibernateMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<HibernateMapping>(underlyingStore);
            classes = new List<ClassMapping>();
            imports = new List<ImportMapping>();

            attributes.SetDefault(x => x.DefaultCascade, "none");
            attributes.SetDefault(x => x.DefaultAccess, "property");
            attributes.SetDefault(x => x.DefaultLazy, true);
            attributes.SetDefault(x => x.AutoImport, true);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessHibernateMapping(this);

            foreach (var import in Imports)
                visitor.Visit(import);

            foreach (var classMapping in Classes)
                visitor.Visit(classMapping);
        }

        public IEnumerable<ClassMapping> Classes
        {
            get { return classes; }
        }

        public IEnumerable<ImportMapping> Imports
        {
            get { return imports; }
        }

        public void AddClass(ClassMapping classMapping)
        {
            classes.Add(classMapping);            
        }

        public void AddImport(ImportMapping importMapping)
        {
            imports.Add(importMapping);
        }

        public string Catalog
        {
            get { return attributes.Get(x => x.Catalog); }
            set { attributes.Set(x => x.Catalog, value); }
        }

        public string DefaultAccess
        {
            get { return attributes.Get(x => x.DefaultAccess); }
            set { attributes.Set(x => x.DefaultAccess, value); }
        }

        public string DefaultCascade
        {
            get { return attributes.Get(x => x.DefaultCascade); }
            set { attributes.Set(x => x.DefaultCascade, value); }
        }

        public bool AutoImport
        {
            get { return attributes.Get(x => x.AutoImport); }
            set { attributes.Set(x => x.AutoImport, value); }
        }

        public string Schema
        {
            get { return attributes.Get(x => x.Schema); }
            set { attributes.Set(x => x.Schema, value); }
        }

        public bool DefaultLazy
        {
            get { return attributes.Get(x => x.DefaultLazy); }
            set { attributes.Set(x => x.DefaultLazy, value); }
        }

        public string Namespace
        {
            get { return attributes.Get(x => x.Namespace); }
            set { attributes.Set(x => x.Namespace, value); }
        }

        public string Assembly
        {
            get { return attributes.Get(x => x.Assembly); }
            set { attributes.Set(x => x.Assembly, value); }
        }

        public bool IsSpecified<TResult>(Expression<Func<HibernateMapping, TResult>> property)
        {
            return attributes.IsSpecified(property);
        }

        public bool HasValue<TResult>(Expression<Func<HibernateMapping, TResult>> property)
        {
            return attributes.HasValue(property);
        }

        public void SetDefaultValue<TResult>(Expression<Func<HibernateMapping, TResult>> property, TResult value)
        {
            attributes.SetDefault(property, value);
        }
    }
}