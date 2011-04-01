using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class HibernateMapping : MappingBase
    {
        private readonly IList<ClassMapping> classes;
        private readonly IList<FilterDefinitionMapping> filters;
        private readonly IList<ImportMapping> imports;
        private readonly AttributeStore<HibernateMapping> attributes;

        public HibernateMapping()
            : this(new AttributeStore())
        {}

        public HibernateMapping(AttributeStore underlyingStore)
        {
            attributes = new AttributeStore<HibernateMapping>(underlyingStore);
            classes = new List<ClassMapping>();
            filters = new List<FilterDefinitionMapping>();
            imports = new List<ImportMapping>();
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessHibernateMapping(this);

            foreach (var import in Imports)
                visitor.Visit(import);

            foreach (var classMapping in Classes)
                visitor.Visit(classMapping);

            foreach (var filterMapping in Filters)
                visitor.Visit(filterMapping);
        }

        public IEnumerable<ClassMapping> Classes
        {
            get { return classes; }
        }

        public IEnumerable<FilterDefinitionMapping> Filters
        {
            get { return filters; }
        }

        public IEnumerable<ImportMapping> Imports
        {
            get { return imports; }
        }

        public void AddClass(ClassMapping classMapping)
        {
            classes.Add(classMapping);            
        }

        public void AddFilter(FilterDefinitionMapping filterMapping)
        {
            filters.Add(filterMapping);
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

        public override bool IsSpecified(string property)
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

        public bool Equals(HibernateMapping other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.classes.ContentEquals(classes) &&
                other.filters.ContentEquals(filters) &&
                other.imports.ContentEquals(imports) &&
                Equals(other.attributes, attributes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(HibernateMapping)) return false;
            return Equals((HibernateMapping)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (classes != null ? classes.GetHashCode() : 0);
                result = (result * 397) ^ (filters != null ? filters.GetHashCode() : 0);
                result = (result * 397) ^ (imports != null ? imports.GetHashCode() : 0);
                result = (result * 397) ^ (attributes != null ? attributes.GetHashCode() : 0);
                return result;
            }
        }
    }
}