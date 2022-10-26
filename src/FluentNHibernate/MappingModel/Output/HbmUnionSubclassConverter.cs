using System;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;
using IComponentMapping = FluentNHibernate.MappingModel.ClassBased.IComponentMapping;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmUnionSubclassConverter : HbmConverterBase<SubclassMapping, HbmUnionSubclass>
    {
        private HbmUnionSubclass hbmUnionSubclass;

        public HbmUnionSubclassConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmUnionSubclass Convert(SubclassMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmUnionSubclass;
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            hbmUnionSubclass = new HbmUnionSubclass();

            if (subclassMapping.IsSpecified("Name"))
                hbmUnionSubclass.name = subclassMapping.Name;

            if (subclassMapping.IsSpecified("Proxy"))
                hbmUnionSubclass.proxy = subclassMapping.Proxy;

            bool lazySpecified = subclassMapping.IsSpecified("Lazy");
            hbmUnionSubclass.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmUnionSubclass.lazy = subclassMapping.Lazy;

            if (subclassMapping.IsSpecified("DynamicUpdate"))
                hbmUnionSubclass.dynamicupdate = subclassMapping.DynamicUpdate;

            if (subclassMapping.IsSpecified("DynamicInsert"))
                hbmUnionSubclass.dynamicinsert = subclassMapping.DynamicInsert;

            if (subclassMapping.IsSpecified("SelectBeforeUpdate"))
                hbmUnionSubclass.selectbeforeupdate = subclassMapping.SelectBeforeUpdate;

            bool abstractSpecified = subclassMapping.IsSpecified("Abstract");
            hbmUnionSubclass.abstractSpecified = abstractSpecified;
            if (abstractSpecified)
                hbmUnionSubclass.@abstract = subclassMapping.Abstract;

            if (subclassMapping.IsSpecified("EntityName"))
                hbmUnionSubclass.entityname = subclassMapping.EntityName;

            if (subclassMapping.IsSpecified("BatchSize"))
                hbmUnionSubclass.batchsize = subclassMapping.BatchSize.ToString();

            if (subclassMapping.IsSpecified("TableName"))
                hbmUnionSubclass.table = subclassMapping.TableName;

            if (subclassMapping.IsSpecified("Schema"))
                hbmUnionSubclass.schema = subclassMapping.Schema;

            if (subclassMapping.IsSpecified("Check"))
                hbmUnionSubclass.check = subclassMapping.Check;

            if (subclassMapping.IsSpecified("Subselect"))
                hbmUnionSubclass.subselect = subclassMapping.Subselect.ToHbmSubselect();

            if (subclassMapping.IsSpecified("Persister"))
                hbmUnionSubclass.persister = subclassMapping.Persister.ToString();
        }

        #region Methods paralleling XmlSubclassWriter

        public override void Visit(SubclassMapping subclassMapping)
        {
            var subType = subclassMapping.SubclassType;
            if (subType == SubclassType.UnionSubclass)
            {
                AddToNullableArray(ref hbmUnionSubclass.unionsubclass1, ConvertFluentSubobjectToHibernateNative<SubclassMapping, HbmUnionSubclass>(subclassMapping));
            }
            else
            {
                throw new NotSupportedException(string.Format("Cannot mix subclass types (subclass type {0} not supported within subclass type {1})", subType, SubclassType.Subclass));
            }
        }

        public override void Visit(IComponentMapping componentMapping)
        {
            // HbmUnionSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            // (ComponentMapping, ExternalComponentMapping, and ReferenceComponentMapping are implementations of IComponentMapping, while 
            // DynamicComponentMapping is a refinement of ComponentMapping)
            AddToNullableArray(ref hbmUnionSubclass.Items, ConvertFluentSubobjectToHibernateNative<IComponentMapping, object>(componentMapping));
        }

        #endregion Methods paralleling XmlSubclassWriter

        #region Methods paralleling XmlClassWriterBase

        public override void Visit(PropertyMapping propertyMapping)
        {
            // HbmUnionSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmUnionSubclass.Items, ConvertFluentSubobjectToHibernateNative<PropertyMapping, HbmProperty>(propertyMapping));
        }

        public override void Visit(OneToOneMapping oneToOneMapping)
        {
            // HbmUnionSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmUnionSubclass.Items, ConvertFluentSubobjectToHibernateNative<OneToOneMapping, HbmOneToOne>(oneToOneMapping));
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            // HbmUnionSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmUnionSubclass.Items, ConvertFluentSubobjectToHibernateNative<ManyToOneMapping, HbmManyToOne>(manyToOneMapping));
        }

        public override void Visit(AnyMapping anyMapping)
        {
            // HbmUnionSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmUnionSubclass.Items, ConvertFluentSubobjectToHibernateNative<AnyMapping, HbmAny>(anyMapping));
        }

        public override void Visit(CollectionMapping collectionMapping)
        {
            // HbmUnionSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            // (array, bag, list, map, and set are refinements of CollectionMapping, while idbag and primitivearray do not yet appear to be implemented)
            AddToNullableArray(ref hbmUnionSubclass.Items, ConvertFluentSubobjectToHibernateNative<CollectionMapping, object>(collectionMapping));
        }

        public override void Visit(StoredProcedureMapping storedProcedureMapping)
        {
            var spType = storedProcedureMapping.SPType;
            var sprocSql = ConvertFluentSubobjectToHibernateNative<StoredProcedureMapping, HbmCustomSQL>(storedProcedureMapping);
            switch (spType)
            {
                case "sql-insert":
                    hbmUnionSubclass.sqlinsert = sprocSql;
                    break;
                case "sql-update":
                    hbmUnionSubclass.sqlupdate = sprocSql;
                    break;
                case "sql-delete":
                    hbmUnionSubclass.sqldelete = sprocSql;
                    break;
                default:
                    throw new NotSupportedException(string.Format("Stored procedure type {0} is not supported", spType));
            }
        }

        #endregion Methods paralleling XmlClassWriterBase
    }
}
