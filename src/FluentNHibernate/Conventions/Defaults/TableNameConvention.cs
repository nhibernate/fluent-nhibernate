using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.InspectionDsl;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Defaults
{
    /// <summary>
    /// Default entity table name convention
    /// </summary>
    public class TableNameConvention : IClassConvention
    {
        public void Accept(IAcceptanceCriteria<IClassInspector> acceptance)
        {
            acceptance.Expect(x => x.TableName, Is.Not.Set);
        }

        public void Apply(IClassInspector classMap)
        {
            //string tableName = classMap.EntityType.Name;

            //if (classMap.EntityType.IsGenericType)
            //{
            //    // special case for generics: GenericType_GenericParameterType
            //    tableName = classMap.EntityType.Name.Substring(0, classMap.EntityType.Name.IndexOf('`'));

            //    foreach (var argument in classMap.EntityType.GetGenericArguments())
            //    {
            //        tableName += "_";
            //        tableName += argument.Name;
            //    }
            //}
            
            //classMap.WithTable("`" + tableName + "`");
        }
    }
}