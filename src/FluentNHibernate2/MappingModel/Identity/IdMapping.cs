using System;
using NHibernate.Cfg.MappingSchema;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel.Identity
{
    public class IdMapping : MappingBase, IIdentityMapping
    {
        private readonly AttributeStore<IdMapping> _attributes;
        private readonly IList<ColumnMapping> _columns;

        public IdMapping()
        {
            _attributes = new AttributeStore<IdMapping>();
            _columns = new List<ColumnMapping>();
        }

        public IdMapping(ColumnMapping columnMapping) : this()
        {
            AddIdColumn(columnMapping);
        }

        public IdGeneratorMapping Generator { get; set; }

        public void AddIdColumn(ColumnMapping column)
        {
            _columns.Add(column);
        }

        public IEnumerable<ColumnMapping> Columns
        {
            get { return _columns; }
        }

        public override void AcceptVisitor(IMappingModelVisitor visitor)
        {
            visitor.ProcessId(this);

            if(Generator != null)
                visitor.ProcessIdGenerator(Generator);
            
            foreach(var column in Columns)
                visitor.ProcessColumn(column);
        }

        public string Name
        {
            get { return _attributes.Get(x => x.Name); }
            set { _attributes.Set(x => x.Name, value); }
        }

        public AttributeStore<IdMapping> Attributes
        {
            get { return _attributes; }
        }
    }
}