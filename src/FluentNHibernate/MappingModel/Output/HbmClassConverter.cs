using System;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Cfg.MappingSchema;
using IComponentMapping = FluentNHibernate.MappingModel.ClassBased.IComponentMapping;

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

        #region Methods paralleling XmlClassWriter

        public override void Visit(DiscriminatorMapping discriminatorMapping)
        {
            hbmClass.discriminator = ConvertFluentSubobjectToHibernateNative<DiscriminatorMapping, HbmDiscriminator>(discriminatorMapping);
        }

        public override void Visit(SubclassMapping subclassMapping)
        {
            // HbmClass.Items1 is Join / JoinedSubclass / Subclass / UnionSubclass (joined and union subclasses are refinements of SubclassMapping)
            AddToNullableArray(ref hbmClass.Items1, ConvertFluentSubobjectToHibernateNative<SubclassMapping, object>(subclassMapping));
        }

        public override void Visit(JoinMapping joinMapping)
        {
            // HbmClass.Items1 is Join / JoinedSubclass / Subclass / UnionSubclass
            AddToNullableArray(ref hbmClass.Items1, ConvertFluentSubobjectToHibernateNative<JoinMapping, HbmJoin>(joinMapping));
        }

        public override void Visit(IComponentMapping componentMapping)
        {
            // HbmClass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            // (ComponentMapping, ExternalComponentMapping, and ReferenceComponentMapping are implementations of IComponentMapping, while 
            // DynamicComponentMapping is a refinement of ComponentMapping)
            AddToNullableArray(ref hbmClass.Items, ConvertFluentSubobjectToHibernateNative<IComponentMapping, object>(componentMapping));
        }

        public override void Visit(IIdentityMapping identityMapping)
        {
            // HbmClass.Item is Id / CompositeId (IdMapping and CompositeIdMapping are implementations of IIdentityMapping)
            hbmClass.Item = ConvertFluentSubobjectToHibernateNative<IIdentityMapping, object>(identityMapping);
        }

        public override void Visit(NaturalIdMapping naturalIdMapping)
        {
            hbmClass.naturalid = ConvertFluentSubobjectToHibernateNative<NaturalIdMapping, HbmNaturalId>(naturalIdMapping);
        }

        public override void Visit(CacheMapping cacheMapping)
        {
            hbmClass.cache = ConvertFluentSubobjectToHibernateNative<CacheMapping, HbmCache>(cacheMapping);
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            // HbmClass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmClass.Items, ConvertFluentSubobjectToHibernateNative<ManyToOneMapping, HbmManyToOne>(manyToOneMapping));
        }

        public override void Visit(FilterMapping filterMapping)
        {
            AddToNullableArray(ref hbmClass.filter, ConvertFluentSubobjectToHibernateNative<FilterMapping, HbmFilter>(filterMapping));
        }

        public override void Visit(TuplizerMapping tuplizerMapping)
        {
            AddToNullableArray(ref hbmClass.tuplizer, ConvertFluentSubobjectToHibernateNative<TuplizerMapping, HbmTuplizer>(tuplizerMapping));
        }

        #endregion Methods paralleling XmlClassWriter

        #region Methods paralleling XmlClassWriterBase

        public override void Visit(PropertyMapping propertyMapping)
        {
            // HbmClass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmClass.Items, ConvertFluentSubobjectToHibernateNative<PropertyMapping, HbmProperty>(propertyMapping));
        }

        public override void Visit(VersionMapping versionMapping)
        {
            // HbmClass.Item1 is Timestamp / Version
            hbmClass.Item1 = ConvertFluentSubobjectToHibernateNative<VersionMapping, HbmVersion>(versionMapping);
        }

        public override void Visit(OneToOneMapping oneToOneMapping)
        {
            // HbmClass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmClass.Items, ConvertFluentSubobjectToHibernateNative<OneToOneMapping, HbmOneToOne>(oneToOneMapping));
        }

        // Visit(ManyToOneMapping) is defined for both XmlClassWriter and XmlClassWriterBase, so the implementation for this class lives in the "parallel to XmlClassWriter" section

        public override void Visit(AnyMapping anyMapping)
        {
            // HbmClass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmClass.Items, ConvertFluentSubobjectToHibernateNative<AnyMapping, HbmAny>(anyMapping));
        }

        public override void Visit(CollectionMapping collectionMapping)
        {
            // HbmClass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            // (array, bag, list, map, and set are refinements of CollectionMapping, while idbag and primitivearray do not yet appear to be implemented)
            AddToNullableArray(ref hbmClass.Items, ConvertFluentSubobjectToHibernateNative<CollectionMapping, object>(collectionMapping));
        }

        public override void Visit(StoredProcedureMapping storedProcedureMapping)
        {
            var spType = storedProcedureMapping.SPType;
            var sprocSql = ConvertFluentSubobjectToHibernateNative<StoredProcedureMapping, HbmCustomSQL>(storedProcedureMapping);
            switch (spType)
            {
                case "sql-insert":
                    hbmClass.sqlinsert = sprocSql;
                    break;
                case "sql-update":
                    hbmClass.sqlupdate = sprocSql;
                    break;
                case "sql-delete":
                    hbmClass.sqldelete = sprocSql;
                    break;
                default:
                    throw new NotSupportedException(string.Format("Stored procedure type {0} is not supported", spType));
            }
        }

        #endregion Methods paralleling XmlClassWriterBase
    }
}
