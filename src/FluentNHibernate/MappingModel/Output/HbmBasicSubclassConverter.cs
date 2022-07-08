using System;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using NHibernate.Cfg.MappingSchema;
using IComponentMapping = FluentNHibernate.MappingModel.ClassBased.IComponentMapping;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmBasicSubclassConverter : HbmConverterBase<SubclassMapping, HbmSubclass>
    {
        private HbmSubclass hbmSubclass;

        public HbmBasicSubclassConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override HbmSubclass Convert(SubclassMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmSubclass;
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            hbmSubclass = new HbmSubclass();

            if (subclassMapping.IsSpecified("Name"))
                hbmSubclass.name = subclassMapping.Name;

            if (subclassMapping.IsSpecified("Proxy"))
                hbmSubclass.proxy = subclassMapping.Proxy;

            bool lazySpecified = subclassMapping.IsSpecified("Lazy");
            hbmSubclass.lazySpecified = lazySpecified;
            if (lazySpecified)
                hbmSubclass.lazy = subclassMapping.Lazy;

            if (subclassMapping.IsSpecified("DynamicUpdate"))
                hbmSubclass.dynamicupdate = subclassMapping.DynamicUpdate;

            if (subclassMapping.IsSpecified("DynamicInsert"))
                hbmSubclass.dynamicinsert = subclassMapping.DynamicInsert;

            if (subclassMapping.IsSpecified("SelectBeforeUpdate"))
                hbmSubclass.selectbeforeupdate = subclassMapping.SelectBeforeUpdate;

            bool abstractSpecified = subclassMapping.IsSpecified("Abstract");
            hbmSubclass.abstractSpecified = abstractSpecified;
            if (abstractSpecified)
                hbmSubclass.@abstract = subclassMapping.Abstract;

            if (subclassMapping.IsSpecified("EntityName"))
                hbmSubclass.entityname = subclassMapping.EntityName;

            if (subclassMapping.IsSpecified("BatchSize"))
                hbmSubclass.batchsize = subclassMapping.BatchSize.ToString();

            if (subclassMapping.IsSpecified("DiscriminatorValue"))
                hbmSubclass.discriminatorvalue = subclassMapping.DiscriminatorValue.ToString();
        }

        #region Methods paralleling XmlSubclassWriter

        public override void Visit(SubclassMapping subclassMapping)
        {
            var subType = subclassMapping.SubclassType;
            if (subType == SubclassType.Subclass)
            {
                AddToNullableArray(ref hbmSubclass.subclass1, ConvertFluentSubobjectToHibernateNative<SubclassMapping, HbmSubclass>(subclassMapping));
            }
            else
            {
                throw new NotSupportedException(string.Format("Cannot mix subclass types (subclass type {0} not supported within subclass type {1})", subType, SubclassType.Subclass));
            }
        }

        public override void Visit(IComponentMapping componentMapping)
        {
            // HbmSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            // (ComponentMapping, ExternalComponentMapping, and ReferenceComponentMapping are implementations of IComponentMapping, while 
            // DynamicComponentMapping is a refinement of ComponentMapping)
            AddToNullableArray(ref hbmSubclass.Items, ConvertFluentSubobjectToHibernateNative<IComponentMapping, object>(componentMapping));
        }

        public override void Visit(JoinMapping joinMapping)
        {
            // HbmSubclass.Join is Joins (but nothing else)
            AddToNullableArray(ref hbmSubclass.join, ConvertFluentSubobjectToHibernateNative<JoinMapping, HbmJoin>(joinMapping));
        }

        #endregion Methods paralleling XmlSubclassWriter

        #region Methods paralleling XmlClassWriterBase

        public override void Visit(PropertyMapping propertyMapping)
        {
            // HbmSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmSubclass.Items, ConvertFluentSubobjectToHibernateNative<PropertyMapping, HbmProperty>(propertyMapping));
        }

        public override void Visit(OneToOneMapping oneToOneMapping)
        {
            // HbmSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmSubclass.Items, ConvertFluentSubobjectToHibernateNative<OneToOneMapping, HbmOneToOne>(oneToOneMapping));
        }

        public override void Visit(ManyToOneMapping manyToOneMapping)
        {
            // HbmSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmSubclass.Items, ConvertFluentSubobjectToHibernateNative<ManyToOneMapping, HbmManyToOne>(manyToOneMapping));
        }

        public override void Visit(AnyMapping anyMapping)
        {
            // HbmSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            AddToNullableArray(ref hbmSubclass.Items, ConvertFluentSubobjectToHibernateNative<AnyMapping, HbmAny>(anyMapping));
        }

        public override void Visit(CollectionMapping collectionMapping)
        {
            // HbmSubclass.Items is Any / Array / Bag / Component / DynamicComponent / Idbag / List / ManyToOne / Map / OneToOne / PrimitiveArray / Properties / Property / Set
            // (array, bag, list, map, and set are refinements of CollectionMapping, while idbag and primitivearray do not yet appear to be implemented)
            AddToNullableArray(ref hbmSubclass.Items, ConvertFluentSubobjectToHibernateNative<CollectionMapping, object>(collectionMapping));
        }

        public override void Visit(StoredProcedureMapping storedProcedureMapping)
        {
            var spType = storedProcedureMapping.SPType;
            var sprocSql = ConvertFluentSubobjectToHibernateNative<StoredProcedureMapping, HbmCustomSQL>(storedProcedureMapping);
            switch (spType)
            {
                case "sql-insert":
                    hbmSubclass.sqlinsert = sprocSql;
                    break;
                case "sql-update":
                    hbmSubclass.sqlupdate = sprocSql;
                    break;
                case "sql-delete":
                    hbmSubclass.sqldelete = sprocSql;
                    break;
                default:
                    throw new NotSupportedException(string.Format("Stored procedure type {0} is not supported", spType));
            }
        }

        #endregion Methods paralleling XmlClassWriterBase
    }
}
