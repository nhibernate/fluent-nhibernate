using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;

namespace FluentNHibernate.MappingModel
{
    [Serializable]
    public class HibernateMapping : MappingBase
    {
        readonly IList<ClassMapping> classes;
        readonly IList<FilterDefinitionMapping> filters;
        readonly IList<ImportMapping> imports;
        readonly AttributeStore attributes;

        public HibernateMapping()
            : this(new AttributeStore())
        {}

        public HibernateMapping(AttributeStore attributes)
        {
            this.attributes = attributes;
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
            get { return attributes.GetOrDefault<string>("Catalog"); }
        }

        public string DefaultAccess
        {
            get { return attributes.GetOrDefault<string>("DefaultAccess"); }
        }

        public string DefaultCascade
        {
            get { return attributes.GetOrDefault<string>("DefaultCascade"); }
        }

        public bool AutoImport
        {
            get { return attributes.GetOrDefault<bool>("AutoImport"); }
        }

        public string Schema
        {
            get { return attributes.GetOrDefault<string>("Schema"); }
        }

        public bool DefaultLazy
        {
            get { return attributes.GetOrDefault<bool>("DefaultLazy"); }
        }

        public string Namespace
        {
            get { return attributes.GetOrDefault<string>("Namespace"); }
        }

        public string Assembly
        {
            get { return attributes.GetOrDefault<string>("Assembly"); }
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

        public void Set<T>(Expression<Func<HibernateMapping, T>> expression, int layer, T value)
        {
            Set(expression.ToMember().Name, layer, value);
        }

        protected override void Set(string attribute, int layer, object value)
        {
            attributes.Set(attribute, layer, value);
        }

        public override bool IsSpecified(string attribute)
        {
            return attributes.IsSpecified(attribute);
        }
    }
}