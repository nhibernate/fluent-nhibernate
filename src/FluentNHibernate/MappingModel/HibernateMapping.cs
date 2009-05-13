using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.MappingModel
{
    public class HibernateMapping : MappingBase
    {
        private readonly IList<ClassMapping> classes;
        private readonly IList<ImportMapping> imports;
        private readonly AttributeStore<HibernateMapping> attributes;
        private readonly IDictionary<string, string> unmigratedAttributes = new Dictionary<string, string>();

        public HibernateMapping()
        {
            attributes = new AttributeStore<HibernateMapping>();
            classes = new List<ClassMapping>();
            imports = new List<ImportMapping>();
        }        

        public IEnumerable<ClassMapping> Classes
        {
            get { return classes; }
        }

        public IEnumerable<ImportMapping> Imports
        {
            get { return imports; }
        }

        public AttributeStore<HibernateMapping> Attributes
        {
            get { return attributes; }
        }

        public IDictionary<string, string> UnmigratedAttributes
        {
            get { return unmigratedAttributes; }
        }

        public void AddClass(ClassMapping classMapping)
        {
            classes.Add(classMapping);            
        }

        public void AddImport(ImportMapping importMapping)
        {
            imports.Add(importMapping);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessHibernateMapping(this);

            foreach (var import in Imports)
                visitor.Visit(import);

            foreach (var classMapping in Classes)
                visitor.Visit(classMapping);
        }

        public string DefaultAccess
        {
            get { return attributes.Get(x => x.DefaultAccess); }
            set { attributes.Set(x => x.DefaultAccess, value); }
        }

        public bool AutoImport
        {
            get { return attributes.Get(x => x.AutoImport); }
            set { attributes.Set(x => x.AutoImport, value); }
        }

        public void AddUnmigratedAttribute(string key, string value)
        {
            unmigratedAttributes.Add(key, value);
        }
    }
}