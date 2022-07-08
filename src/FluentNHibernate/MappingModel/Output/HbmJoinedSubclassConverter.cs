using System;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;
using NHibernate.Cfg.MappingSchema;
using IComponentMapping = FluentNHibernate.MappingModel.ClassBased.IComponentMapping;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmJoinedSubclassConverter : HbmConverterBase<SubclassMapping, HbmJoinedSubclass>
    {
        private HbmJoinedSubclass hbmJoinedSubclass;

        public HbmJoinedSubclassConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmJoinedSubclass Convert(SubclassMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmJoinedSubclass;
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            hbmJoinedSubclass = new HbmJoinedSubclass();

            if (subclassMapping.IsSpecified("Name"))
                hbmJoinedSubclass.name = subclassMapping.Name;

            if (subclassMapping.IsSpecified("Proxy"))
                hbmJoinedSubclass.proxy = subclassMapping.Proxy;

            bool lazySpecified = subclassMapping.IsSpecified("Lazy");
            hbmJoinedSubclass.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmJoinedSubclass.lazy = subclassMapping.Lazy;

            if (subclassMapping.IsSpecified("DynamicUpdate"))
                hbmJoinedSubclass.dynamicupdate = subclassMapping.DynamicUpdate;

            if (subclassMapping.IsSpecified("DynamicInsert"))
                hbmJoinedSubclass.dynamicinsert = subclassMapping.DynamicInsert;

            if (subclassMapping.IsSpecified("SelectBeforeUpdate"))
                hbmJoinedSubclass.selectbeforeupdate = subclassMapping.SelectBeforeUpdate;

            bool abstractSpecified = subclassMapping.IsSpecified("Abstract");
            hbmJoinedSubclass.abstractSpecified = abstractSpecified;
            if (abstractSpecified)
                hbmJoinedSubclass.@abstract = subclassMapping.Abstract;

            if (subclassMapping.IsSpecified("EntityName"))
                hbmJoinedSubclass.entityname = subclassMapping.EntityName;

            if (subclassMapping.IsSpecified("BatchSize"))
                hbmJoinedSubclass.batchsize = subclassMapping.BatchSize.ToString();

            if (subclassMapping.IsSpecified("TableName"))
                hbmJoinedSubclass.table = subclassMapping.TableName;

            if (subclassMapping.IsSpecified("Schema"))
                hbmJoinedSubclass.schema = subclassMapping.Schema;

            if (subclassMapping.IsSpecified("Check"))
                hbmJoinedSubclass.check = subclassMapping.Check;

            if (subclassMapping.IsSpecified("Subselect"))
                hbmJoinedSubclass.subselect = subclassMapping.Subselect.ToHbmSubselect();

            if (subclassMapping.IsSpecified("Persister"))
                hbmJoinedSubclass.persister = subclassMapping.Persister.ToString();
        }

        #region Methods paralleling XmlSubclassWriter

        public override void Visit(KeyMapping keyMapping)
        {
            hbmJoinedSubclass.key = ConvertFluentSubobjectToHibernateNative<KeyMapping, HbmKey>(keyMapping);
        }

        public override void Visit(SubclassMapping subclassMapping)
        {
            var subType = subclassMapping.SubclassType;
            if (subType == SubclassType.JoinedSubclass)
            {
                AddToNullableArray(ref hbmJoinedSubclass.joinedsubclass1, ConvertFluentSubobjectToHibernateNative<SubclassMapping, HbmJoinedSubclass>(subclassMapping));
            }
            else
            {
                throw new NotSupportedException(string.Format("Cannot mix subclass types (subclass type {0} not supported within subclass type {1})", subType, SubclassType.Subclass));
            }
        }

        public override void Visit(IComponentMapping componentMapping)
        {
            // HbmJoinedSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            // (ComponentMapping, ExternalComponentMapping, and ReferenceComponentMapping are implementations of IComponentMapping, while 
            // DynamicComponentMapping is a refinement of ComponentMapping)
            AddToNullableArray(ref hbmJoinedSubclass.Items, ConvertFluentSubobjectToHibernateNative<IComponentMapping, object>(componentMapping));
        }

        #endregion Methods paralleling XmlSubclassWriter

        #region Methods paralleling XmlClassWriterBase

        public override void Visit(PropertyMapping propertyMapping)
        {
            // HbmJoinedSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmJoinedSubclass.Items, ConvertFluentSubobjectToHibernateNative<PropertyMapping, HbmProperty>(propertyMapping));
        }

        public override void Visit(OneToOneMapping oneToOneMapping)
        {
            // HbmJoinedSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmJoinedSubclass.Items, ConvertFluentSubobjectToHibernateNative<OneToOneMapping, HbmOneToOne>(oneToOneMapping));
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            // HbmJoinedSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmJoinedSubclass.Items, ConvertFluentSubobjectToHibernateNative<ManyToOneMapping, HbmManyToOne>(manyToOneMapping));
        }

        public override void Visit(AnyMapping anyMapping)
        {
            // HbmJoinedSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmJoinedSubclass.Items, ConvertFluentSubobjectToHibernateNative<AnyMapping, HbmAny>(anyMapping));
        }

        public override void Visit(CollectionMapping collectionMapping)
        {
            // HbmJoinedSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            // (array, bag, list, map, and set are refinements of CollectionMapping, while idbag and primitivearray do not yet appear to be implemented)
            AddToNullableArray(ref hbmJoinedSubclass.Items, ConvertFluentSubobjectToHibernateNative<CollectionMapping, object>(collectionMapping));
        }

        public override void Visit(StoredProcedureMapping storedProcedureMapping)
        {
            var spType = storedProcedureMapping.SPType;
            var sprocSql = ConvertFluentSubobjectToHibernateNative<StoredProcedureMapping, HbmCustomSQL>(storedProcedureMapping);
            switch (spType)
            {
                case "sql-insert":
                    hbmJoinedSubclass.sqlinsert = sprocSql;
                    break;
                case "sql-update":
                    hbmJoinedSubclass.sqlupdate = sprocSql;
                    break;
                case "sql-delete":
                    hbmJoinedSubclass.sqldelete = sprocSql;
                    break;
                default:
                    throw new NotSupportedException(string.Format("Stored procedure type {0} is not supported", spType));
            }
        }

        #endregion Methods paralleling XmlClassWriterBase
    }
}
