using System;
using System.Reflection;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.MappingModel.Conventions
{
    public class NamingConvention : DefaultMappingModelVisitor
    {
        public Func<MemberInfo, string> DetermineNameFromMember = info => info.Name;
        public Func<Type, string> DetermineNameFromType = type => type.AssemblyQualifiedName;

        protected void Process(INameable nameable, MemberInfo info)
        {
            if (!nameable.IsNameSpecified)
            {
                if (info == null)
                    throw new ConventionException("Cannot apply the naming convention. No member specified.", nameable);
                nameable.Name = DetermineNameFromMember(info);
            }
        }

        protected override void ProcessClassBase(ClassMappingBase classMapping)
        {
            if (!classMapping.IsNameSpecified)
            {
                if (classMapping.Type == null)
                    throw new ConventionException("Cannot apply the naming convention. No type specified.", classMapping);
                classMapping.Name = DetermineNameFromType(classMapping.Type);
            }
        }

        public override void ProcessProperty(PropertyMapping propertyMapping)
        {
            Process(propertyMapping, propertyMapping.PropertyInfo);
        }

        protected override void ProcessCollection(ICollectionMapping collectionMapping)
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

        public override void ProcessOneToMany(OneToManyMapping oneToManyMapping)
        {
            if (!oneToManyMapping.Attributes.IsSpecified(x => x.ClassName))
            {
                if (oneToManyMapping.ChildType == null)
                    throw new ConventionException("Cannot apply the naming convention. No type specified.", oneToManyMapping);
                oneToManyMapping.ClassName = DetermineNameFromType(oneToManyMapping.ChildType);
            }
        }

        public override void ProcessManyToMany(ManyToManyMapping manyToManyMapping)
        {
            if (!manyToManyMapping.Attributes.IsSpecified(x => x.ClassName))
            {
                if (manyToManyMapping.ChildType == null)
                    throw new ConventionException("Cannot apply the naming convention. No type specified.", manyToManyMapping);
                manyToManyMapping.ClassName = DetermineNameFromType(manyToManyMapping.ChildType);
            }
        }

        public override void ProcessComponent(ComponentMappingBase componentMapping)
        {
            if (!componentMapping.Attributes.IsSpecified(x => x.PropertyName))
            {
                if (componentMapping.PropertyInfo == null)
                    throw new ConventionException("Cannot apply the naming convention. No member specified.", componentMapping);

                componentMapping.PropertyName = DetermineNameFromMember(componentMapping.PropertyInfo);
            }
        }
    }
}