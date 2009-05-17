using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Inspections;
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

        public void Apply(IClassAlteration alteration, IClassInspector inspector)
        {
            var tableName = inspector.EntityType.Name;

            if (inspector.EntityType.IsGenericType)
            {
                // special case for generics: GenericType_GenericParameterType
                tableName = inspector.EntityType.Name.Substring(0, inspector.EntityType.Name.IndexOf('`'));

                foreach (var argument in inspector.EntityType.GetGenericArguments())
                {
                    tableName += "_";
                    tableName += argument.Name;
                }
            }

            alteration.WithTable("`" + tableName + "`");
        }
    }
}