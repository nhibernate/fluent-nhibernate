using System;
using System.Xml;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.AutoMap
{
    public class AutoSubClassPart<T> : AutoMap<T>, ISubclass
    {
        private readonly object discriminatorValue;
        private readonly Cache<string, string> attributes = new Cache<string, string>();

        public AutoSubClassPart(object discriminatorValue)
        {
            this.discriminatorValue = discriminatorValue;
        }

        void ISubclass.Proxy(Type type)
        {
            throw new NotImplementedException();
        }

        void ISubclass.Proxy<T1>()
        {
            throw new NotImplementedException();
        }

        void ISubclass.SelectBeforeUpdate()
        {
            throw new NotImplementedException();
        }

        void ISubclass.Abstract()
        {
            throw new NotImplementedException();
        }

        ISubclass ISubclass.Not
        {
            get { throw new NotImplementedException(); }
        }

        SubclassMapping ISubclass.GetSubclassMapping()
        {
            throw new NotImplementedException();
        }
    }
}