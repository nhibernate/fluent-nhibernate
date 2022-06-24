using FluentNHibernate.MappingModel.ClassBased;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmClassConverter : HbmConverterBase<ClassMapping, HbmClass>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmPolymorphismType> polyDict = new XmlLinkedEnumBiDictionary<HbmPolymorphismType>();
        private static readonly XmlLinkedEnumBiDictionary<HbmOptimisticLockMode> optLockDict = new XmlLinkedEnumBiDictionary<HbmOptimisticLockMode>();

        private HbmClass hbmClass;

        public HbmClassConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmClass Convert(ClassMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmClass;
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            hbmClass = new HbmClass();

            if (classMapping.IsSpecified("BatchSize"))
                hbmClass.batchsize = classMapping.BatchSize;

            if (classMapping.IsSpecified("DiscriminatorValue"))
                hbmClass.discriminatorvalue = classMapping.DiscriminatorValue.ToString();

            if (classMapping.IsSpecified("DynamicInsert"))
                hbmClass.dynamicinsert = classMapping.DynamicInsert;

            if (classMapping.IsSpecified("DynamicUpdate"))
                hbmClass.dynamicupdate = classMapping.DynamicUpdate;

            if (classMapping.IsSpecified("Lazy"))
                hbmClass.lazy = classMapping.Lazy;

            if (classMapping.IsSpecified("Schema"))
                hbmClass.schema = classMapping.Schema;

            if (classMapping.IsSpecified("Mutable"))
                hbmClass.mutable = classMapping.Mutable;

            if (classMapping.IsSpecified("Polymorphism"))
                hbmClass.polymorphism = polyDict[classMapping.Polymorphism];

            if (classMapping.IsSpecified("Persister"))
                hbmClass.persister = classMapping.Persister;

            if (classMapping.IsSpecified("Where"))
                hbmClass.where = classMapping.Where;

            if (classMapping.IsSpecified("OptimisticLock"))
                hbmClass.optimisticlock = optLockDict[classMapping.OptimisticLock];

            if (classMapping.IsSpecified("Check"))
                hbmClass.check = classMapping.Check;

            if (classMapping.IsSpecified("Name"))
                hbmClass.name = classMapping.Name;

            if (classMapping.IsSpecified("TableName"))
                hbmClass.table = classMapping.TableName;

            if (classMapping.IsSpecified("Proxy"))
                hbmClass.proxy = classMapping.Proxy;

            if (classMapping.IsSpecified("SelectBeforeUpdate"))
                hbmClass.selectbeforeupdate = classMapping.SelectBeforeUpdate;

            if (classMapping.IsSpecified("Abstract"))
                hbmClass.@abstract = classMapping.Abstract;

            if (classMapping.IsSpecified("Subselect"))
                hbmClass.subselect = ToHbmSubselect(classMapping.Subselect);

            if (classMapping.IsSpecified("SchemaAction"))
                hbmClass.schemaaction = classMapping.SchemaAction;

            if (classMapping.IsSpecified("EntityName"))
                hbmClass.entityname = classMapping.EntityName;
        }

        public static HbmSubselect ToHbmSubselect(params string[] text)
        {
            return new HbmSubselect()
            {
                Text = text
            };
        }
    }
}
