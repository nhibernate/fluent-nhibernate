using System;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Alterations;
using FluentNHibernate.Conventions.Alterations.Instances;
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

        public void Apply(IClassInstance instance)
        {
            var tableName = instance.EntityType.Name;

            if (instance.EntityType.IsGenericType)
            {
                // special case for generics: GenericType_GenericParameterType
                tableName = instance.EntityType.Name.Substring(0, instance.EntityType.Name.IndexOf('`'));

                foreach (var argument in instance.EntityType.GetGenericArguments())
                {
                    tableName += "_";
                    tableName += argument.Name;
                }
            }

            instance.WithTable("`" + tableName + "`");
        }
    }
}