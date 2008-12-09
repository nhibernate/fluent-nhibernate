using NHibernate.Cfg.MappingSchema;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel.Identity
{
    public class IdMapping : MappingBase<HbmId>, IIdentityMapping
    {
        private IdGeneratorMapping _generator;
        private readonly IList<IdColumnMapping> _columns;

        public IdMapping(string name, IdColumnMapping column, IdGeneratorMapping generator)
        {
            _columns = new List<IdColumnMapping>();

            Name = name;
            Generator = generator;
            AddIdColumn(column);            
        }

        public IdGeneratorMapping Generator
        {
            get { return _generator; }
            set
            {
                _generator = value;
                _hbm.generator = _generator.Hbm;
            }
        }        

        public string Name
        {
            get { return _hbm.name; }
            set { _hbm.name = value; }
        }

        public void AddIdColumn(IdColumnMapping column)
        {
            _columns.Add(column);
            column.Hbm.AddTo(ref _hbm.column);
        }

        public IEnumerable<IdColumnMapping> Columns
        {
            get { return _columns; }
        }

        
    }
}