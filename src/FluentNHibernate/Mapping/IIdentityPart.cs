using System;
using System.Reflection;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping
{
    public interface IIdentityPart : IAccessStrategy<IIdentityPart>
    {
        IdentityGenerationStrategyBuilder<IIdentityPart> GeneratedBy { get; }
        Type IdentityType { get; }
        Type EntityType { get; }
        PropertyInfo Property { get; }
        string GetColumnName();

        /// <summary>
        /// Sets the unsaved-value of the identity.
        /// </summary>
        /// <param name="unsavedValue">Value that represents an unsaved value.</param>
        IIdentityPart UnsavedValue(object unsavedValue);

        /// <summary>
        /// Sets the column name for the identity field.
        /// </summary>
        /// <param name="columnName">Column name</param>
        IIdentityPart ColumnName(string columnName);

        IdMapping GetIdMapping();
    }
}