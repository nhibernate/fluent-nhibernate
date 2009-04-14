using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
using FluentNHibernate.Versioning.HbmExtensions;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmKeyWriter : NullMappingModelVisitor, IXmlWriter<KeyMapping>
    {
        private HbmKey _hbm;

        public object Write(KeyMapping mappingModel)
        {
            _hbm = null;
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessKey(KeyMapping keyMapping)
        {
            _hbm = new HbmKey();

            if (keyMapping.Attributes.IsSpecified(x => x.Column))
                _hbm.column1 = keyMapping.Column;

            if (keyMapping.Attributes.IsSpecified(x => x.ForeignKey))
                _hbm.foreignkey = keyMapping.ForeignKey;

            if (keyMapping.Attributes.IsSpecified(x => x.PropertyReference))
                _hbm.propertyref = keyMapping.PropertyReference;

            if (keyMapping.Attributes.IsSpecified(x => x.CascadeOnDelete))
                _hbm.SetCascadeOnDelete(keyMapping.CascadeOnDelete);
        }
    }
}
