using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Conventions
{
    public class NamingConvention : MappingModelVisitorBase
    {
        public Func<MemberInfo, string> DetermineNameFromMember = info => info.Name;
        public Func<Type, string> DetermineNameFromType = type => type.AssemblyQualifiedName;
        
        protected void Process(INameable nameable, MemberInfo info)
        {
            if (!nameable.IsNameSpecified)
            {
                if(info == null)
                    throw new ConventionException("Cannot apply the naming convention. No member specified.", nameable);
                nameable.Name = DetermineNameFromMember(info);
            }
        }

        public override void ProcessClass(ClassMapping classMapping)
        {
            if (!classMapping.IsNameSpecified)
            {
                if(classMapping.Type == null)
                    throw new ConventionException("Cannot apply the naming convention. No type specified.", classMapping);
                classMapping.Name = DetermineNameFromType(classMapping.Type);
            }
        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            Process(propertyMapping, propertyMapping.PropertyInfo);
        }

        public override void ProcessCollection(ICollectionMapping collectionMapping)
        {
            Process(collectionMapping, collectionMapping.PropertyInfo);
        }

        public override void ProcessId(Identity.IdMapping idMapping)
        {
            Process(idMapping, idMapping.PropertyInfo);
        }

        public override void ProcessColumn(ColumnMapping columnMapping)
        {
            Process(columnMapping, columnMapping.PropertyInfo);
        }

        public override void ProcessManyToOne(ManyToOneMapping manyToOneMapping)
        {
            Process(manyToOneMapping, manyToOneMapping.PropertyInfo);
        }
    }    

}