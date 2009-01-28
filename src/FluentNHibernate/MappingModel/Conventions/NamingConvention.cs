using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Conventions
{
    public class NamingConvention : MappingModelVisitorBase
    {        
        public override void ProcessClass(ClassMapping classMapping)
        {
            if (!classMapping.Attributes.IsSpecified(x => x.Name))
            {
                if(classMapping.Type == null)
                    throw new ConventionException("Cannot apply the naming convention because the class mapping does not have a type.");
                classMapping.Name = classMapping.Type.AssemblyQualifiedName;
            }

        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            if(!propertyMapping.Attributes.IsSpecified(x => x.Name))
            {
                if(propertyMapping.PropertyInfo == null)
                    throw new ConventionException("Cannot apply the naming convention because the property mapping does not have a property info.");
                propertyMapping.Name = propertyMapping.PropertyInfo.Name;

            }
        }

        public override void ProcessCollection(ICollectionMapping collectionMapping)
        {
            if (!collectionMapping.Attributes.IsSpecified(x => x.Name))
            {
                if (collectionMapping.PropertyInfo == null)
                    throw new ConventionException("Cannot apply the naming convention because the collection mapping does not have a property info.");
                collectionMapping.Name = collectionMapping.PropertyInfo.Name;

            }
        }

        public override void ProcessId(Identity.IdMapping idMapping)
        {
            if (!idMapping.Attributes.IsSpecified(x => x.Name))
            {
                if (idMapping.PropertyInfo == null)
                    throw new ConventionException("Cannot apply the naming convention because the id mapping does not have a property info.");
                idMapping.Name = idMapping.PropertyInfo.Name;
            }
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            if (!columnMapping.Attributes.IsSpecified(x => x.Name))
            {
                if (columnMapping.PropertyInfo == null)
                    throw new ConventionException("Cannot apply the naming convention because the column mapping does not have a property info.");
                columnMapping.Name = columnMapping.PropertyInfo.Name;
            }
        }

        public override void ProcessManyToOne(ManyToOneMapping manyToOneMapping)
        {
            if (!manyToOneMapping.Attributes.IsSpecified(x => x.Name))
            {
                if (manyToOneMapping.PropertyInfo == null)
                    throw new ConventionException("Cannot apply the naming convention because the many to one mapping does not have a property info.");
                manyToOneMapping.Name = manyToOneMapping.PropertyInfo.Name;
            }
        }
    }    

}