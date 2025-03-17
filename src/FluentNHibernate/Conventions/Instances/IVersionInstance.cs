using System;
using System.Diagnostics;
using FluentNHibernate.Conventions.Inspections;

namespace FluentNHibernate.Conventions.Instances;

public interface IVersionInstance : IVersionInspector
{
    new IAccessInstance Access { get; }
    /// <summary>
    /// Instructs NHibernate to immediately issues a SELECT after INSERT or UPDATE to retrieve the generated values.
    /// <para> See https://nhibernate.info/doc/nhibernate-reference/mapping.html#mapping-generated for more information. </para>
    /// </summary>
    /// <remarks> It is user's responsibility to specify how the columns are generated. </remarks>
    new IGeneratedInstance Generated { get; }
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    IVersionInstance Not { get; }
    void Column(string columnName);
    new void UnsavedValue(string unsavedValue);
    new void Length(int length);
    new void Precision(int precision);
    new void Scale(int scale);
    new void Nullable();
    new void Unique();
    new void UniqueKey(string keyColumns);
    void CustomSqlType(string sqlType);
    new void Index(string index);
    new void Check(string constraint);
    new void Default(object value);
    void CustomType<T>();
    void CustomType(Type type);
    void CustomType(string type);
}
