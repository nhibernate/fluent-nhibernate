using System;
using FluentNHibernate.MappingModel.ClassBased;
using NHibernate.Cfg.MappingSchema;

namespace FluentNHibernate.MappingModel.Output
{
    public class HbmSubclassConverter : HbmConverterBase<SubclassMapping, object>
    {
        private object hbmSubclass;

        public HbmSubclassConverter(IHbmConverterServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        public override object Convert(SubclassMapping mapping)
        {
            mapping.AcceptVisitor(this);
            return hbmSubclass;
        }

        public override void ProcessSubclass(SubclassMapping subclassMapping)
        {
            // Yes, this should really be a switch/case statement, but the way SubclassType is currently implemented
            // makes that even uglier, for for now it gets to be a giant sequence of if blocks.
            var subType = subclassMapping.SubclassType;
            if (subType == SubclassType.Subclass)
            {
                hbmSubclass = ConvertFluentSubobjectToHibernateNative<SubclassMapping, HbmSubclass>(subclassMapping);
            }
            else if (subType == SubclassType.JoinedSubclass)
            {
                hbmSubclass = ConvertFluentSubobjectToHibernateNative<SubclassMapping, HbmJoinedSubclass>(subclassMapping);
            }
            else if (subType == SubclassType.UnionSubclass)
            {
                hbmSubclass = ConvertFluentSubobjectToHibernateNative<SubclassMapping, HbmUnionSubclass>(subclassMapping);
            }
            else
            {
                throw new NotSupportedException(string.Format("Subclass type {0} is not supported", subType));
            }
        }
    }
}
