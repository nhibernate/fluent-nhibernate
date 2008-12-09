using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Identity
{
    public class IdGeneratorMapping : MappingBase<HbmGenerator>
    {
        public IdGeneratorMapping(string className)
        {
            ClassName = className;
        }

        public string ClassName
        {
            get { return _hbm.@class; }
            set { _hbm.@class = value; }
        }

        public static IdGeneratorMapping NativeGenerator
        {
            get { return new IdGeneratorMapping("native"); }
        }
    }
}