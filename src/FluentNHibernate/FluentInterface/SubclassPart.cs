using System;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Reflection;

namespace FluentNHibernate.FluentInterface
{
    public class SubclassPart<TSubclassType> : MapBase<TSubclassType>
    {
        private readonly SubclassMapping _subclass;

        public SubclassPart(SubclassMapping subclassMapping) : base(subclassMapping)
        {
            _subclass = subclassMapping;            
        }
    }
}