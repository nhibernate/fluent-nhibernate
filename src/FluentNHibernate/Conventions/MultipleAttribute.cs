using System;

namespace FluentNHibernate.Conventions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MultipleAttribute : Attribute
    {}
}