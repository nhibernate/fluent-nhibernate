using System.Collections.Generic;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmHibernateMappingWriter : MappingModelVisitorBase, IHbmWriter<HibernateMapping>
    {
        private readonly IHbmWriter<ClassMapping> _classWriter;
        private HbmMapping _hbm;

        public HbmHibernateMappingWriter(IHbmWriter<ClassMapping> classWriter)
        {
            _classWriter = classWriter;
        }

        object IHbmWriter<HibernateMapping>.Write(HibernateMapping mapping)
        {
            return Write(mapping);
        }

        public HbmMapping Write(HibernateMapping mapping)
        {
            mapping.AcceptVisitor(this);                        
            return _hbm;
        }

        public override void ProcessHibernateMapping(HibernateMapping hibernateMapping)
        {
            _hbm = new HbmMapping();
            _hbm.defaultlazy = hibernateMapping.DefaultLazy;
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            object hbmClass = _classWriter.Write(classMapping);
            hbmClass.AddTo(ref _hbm.Items);
        }
    }
}