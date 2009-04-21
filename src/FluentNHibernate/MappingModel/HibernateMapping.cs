using System.Collections.Generic;

namespace FluentNHibernate.MappingModel
{
    public class HibernateMapping : MappingBase
    {
        private readonly IList<ClassMapping> _classes;
        private readonly IList<ImportMapping> _imports;
        private readonly AttributeStore<HibernateMapping> _attributes;

        public HibernateMapping()
        {
            _attributes = new AttributeStore<HibernateMapping>();
            _classes = new List<ClassMapping>();
            _imports = new List<ImportMapping>();
        }        

        public IEnumerable<ClassMapping> Classes
        {
            get { return _classes; }
        }

        public IEnumerable<ImportMapping> Imports
        {
            get { return _imports; }
        }

        public AttributeStore<HibernateMapping> Attributes
        {
            get { return _attributes; }
        }

        public void AddClass(ClassMapping classMapping)
        {
            _classes.Add(classMapping);            
        }

        public void AddImport(ImportMapping importMapping)
        {
            _imports.Add(importMapping);
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessHibernateMapping(this);

            foreach (var import in Imports)
                visitor.Visit(import);

            foreach (var classMapping in Classes)
                visitor.Visit(classMapping);
        }

        public bool DefaultLazy
        {
            get { return _attributes.Get(x => x.DefaultLazy); }
            set { _attributes.Set(x => x.DefaultLazy, value); }
        }

        public string DefaultAccess
        {
            get { return _attributes.Get(x => x.DefaultAccess); }
            set { _attributes.Set(x => x.DefaultAccess, value); }
        }

        public bool AutoImport
        {
            get { return _attributes.Get(x => x.AutoImport); }
            set { _attributes.Set(x => x.AutoImport, value); }
        }
    }
}