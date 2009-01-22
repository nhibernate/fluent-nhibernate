using NHibernate.Cfg.MappingSchema;
using FluentNHibernate.Versioning.HbmExtensions;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmPropertyWriter : MappingModelVisitorBase, IHbmWriter<PropertyMapping>
    {
        private HbmProperty _hbm;

        public object Write(PropertyMapping mappingModel)
        {
            mappingModel.AcceptVisitor(this);
            return _hbm;
        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            _hbm = new HbmProperty();

            _hbm.name = propertyMapping.Name;

            if(propertyMapping.Attributes.IsSpecified(x => x.IsNotNullable))
            {
                _hbm.SetNotNull(propertyMapping.IsNotNullable);
            }

            if (propertyMapping.Attributes.IsSpecified(x => x.Length))
                _hbm.length = propertyMapping.Length.ToString();
        }
    }
}