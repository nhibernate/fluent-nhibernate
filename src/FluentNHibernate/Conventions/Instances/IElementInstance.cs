using System;

using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IElementInstance : IElementInspector
    {
        new void Type<T>();
        new void Type(string type);
        new void Type(Type type);
        void Column(string name);
    }
}