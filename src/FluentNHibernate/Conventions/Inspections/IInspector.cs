using System;
using System.Reflection;

namespace FluentNHibernate.Conventions.Inspections
{
    public interface IInspector
    {
        Type EntityType { get; }

        /// <summary>
        /// Represents a string identifier for the model instance, used in conventions for a lazy
        /// shortcut.
        /// 
        /// e.g. for a ColumnMapping the StringIdentifierForModel would be the Name attribute,
        /// this allows the user to find any columns with the matching name.
        /// </summary>
        string StringIdentifierForModel { get; }

        bool IsSet(PropertyInfo property);
    }
}