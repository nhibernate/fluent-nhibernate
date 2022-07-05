using System;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;
using IComponentMapping = FluentNHibernate.MappingModel.ClassBased.IComponentMapping;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmJoinConverter : HbmConverterBase<JoinMapping, HbmJoin>
    {
        private static readonly XmlLinkedEnumBiDictionary<HbmJoinFetch> fetchDict = new XmlLinkedEnumBiDictionary<HbmJoinFetch>();

        private HbmJoin hbmJoin;

        public HbmJoinConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmJoin Convert(JoinMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmJoin;
        }

        public override void ProcessJoin(JoinMapping joinMapping)
        {
            hbmJoin = new HbmJoin();

            if (joinMapping.IsSpecified("TableName"))
                hbmJoin.table = joinMapping.TableName;

            if (joinMapping.IsSpecified("Schema"))
                hbmJoin.schema = joinMapping.Schema;

            if (joinMapping.IsSpecified("Fetch"))
                hbmJoin.fetch = LookupEnumValueIn(fetchDict, joinMapping.Fetch);

            if (joinMapping.IsSpecified("Catalog"))
                hbmJoin.catalog = joinMapping.Catalog;

            if (joinMapping.IsSpecified("Subselect"))
                hbmJoin.subselect = joinMapping.Subselect.ToHbmSubselect();

            if (joinMapping.IsSpecified("Inverse"))
                hbmJoin.inverse = joinMapping.Inverse;

            if (joinMapping.IsSpecified("Optional"))
                hbmJoin.optional = joinMapping.Optional;
        }

        public override void Visit(PropertyMapping propertyMapping)
        {
            // HbmJoin.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / PrimitiveArray / Property / Set
            AddToNullableArray(ref hbmJoin.Items, ConvertFluentSubobjectToHibernateNative<PropertyMapping, HbmProperty>(propertyMapping));
        }

        public override void Visit(KeyMapping keyMapping)
        {
            hbmJoin.key = ConvertFluentSubobjectToHibernateNative<KeyMapping, HbmKey>(keyMapping);
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            // HbmJoin.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / PrimitiveArray / Property / Set
            AddToNullableArray(ref hbmJoin.Items, ConvertFluentSubobjectToHibernateNative<ManyToOneMapping, HbmManyToOne>(manyToOneMapping));
        }

        public override void Visit(IComponentMapping componentMapping)
        {
            // HbmJoin.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / PrimitiveArray / Property / Set
            // (ComponentMapping, ExternalComponentMapping, and ReferenceComponentMapping are implementations of IComponentMapping, while 
            // DynamicComponentMapping is a refinement of ComponentMapping)
            AddToNullableArray(ref hbmJoin.Items, ConvertFluentSubobjectToHibernateNative<IComponentMapping, object>(componentMapping));
        }

        public override void Visit(AnyMapping anyMapping)
        {
            // HbmJoin.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / PrimitiveArray / Property / Set
            AddToNullableArray(ref hbmJoin.Items, ConvertFluentSubobjectToHibernateNative<AnyMapping, HbmAny>(anyMapping));
        }

        public override void Visit(CollectionMapping collectionMapping)
        {
            // HbmJoin.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / PrimitiveArray / Property / Set
            // (array, bag, list, map, and set are refinements of CollectionMapping, while idbag and primitivearray do not yet appear to be implemented)
            AddToNullableArray(ref hbmJoin.Items, ConvertFluentSubobjectToHibernateNative<CollectionMapping, object>(collectionMapping));
        }

        public override void Visit(StoredProcedureMapping storedProcedureMapping)
        {
            var spType = storedProcedureMapping.SPType;
            var sprocSql = ConvertFluentSubobjectToHibernateNative<StoredProcedureMapping, HbmCustomSQL>(storedProcedureMapping);
            switch (spType)
            {
                case "sql-insert":
                    hbmJoin.sqlinsert = sprocSql;
                    break;
                case "sql-update":
                    hbmJoin.sqlupdate = sprocSql;
                    break;
                case "sql-delete":
                    hbmJoin.sqldelete = sprocSql;
                    break;
                default:
                    throw new NotSupportedException(string.Format("Stored procedure type {0} is not supported", spType));
            }
        }
    }
}
