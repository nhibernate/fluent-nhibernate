using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace FluentNHibernate.Conventions
{
    /// <summary>
    /// Base convention for specifying your own many-to-many table naming style. Implement
    /// the abstract members defined by this class to control how your join tables are named
    /// for uni and bi-directional many-to-many's.
    /// </summary>
    public abstract class ManyToManyTableNameConvention : IHasManyToManyConvention
    {
        public void Apply(IManyToManyCollectionInstance instance)
        {
            if (instance.OtherSide == null)
            {
                // uni-directional
                var tableName = GetUniDirectionalTableName(instance);

                instance.Table(tableName);
            }
            else
            {
                // bi-directional
                if (instance.HasExplicitTable && instance.OtherSide.HasExplicitTable)
                {
                    // TODO: We could check if they're the same here and warn the user if they're not
                    return;
                }

                if (instance.HasExplicitTable && !instance.OtherSide.HasExplicitTable)
                    instance.OtherSide.Table(instance.TableName);
                else if (!instance.HasExplicitTable && instance.OtherSide.HasExplicitTable)
                    instance.Table(instance.OtherSide.TableName);
                else
                {
                    var tableName = GetBiDirectionalTableName(instance, instance.OtherSide);

                    instance.Table(tableName);
                    instance.OtherSide.Table(tableName);
                }
            }
        }

        /// <summary>
        /// Gets the name used for bi-directional many-to-many tables. Implement this member to control how
        /// your join table is named for bi-directional relationships.
        /// </summary>
        /// <remarks>
        /// This method will be called once per bi-directional relationship; once one side of the relationship
        /// has been saved, then the other side will assume that name aswell.
        /// </remarks>
        /// <param name="collection">Main collection</param>
        /// <param name="otherSide">Inverse collection</param>
        /// <returns>Many-to-many table name</returns>
        protected abstract string GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide);

        /// <summary>
        /// Gets the name used for uni-directional many-to-many tables. Implement this member to control how
        /// your join table is named for uni-directional relationships.
        /// </summary>
        /// <param name="collection">Main collection</param>
        /// <returns>Many-to-many table name</returns>
        protected abstract string GetUniDirectionalTableName(IManyToManyCollectionInspector collection);
    }
}