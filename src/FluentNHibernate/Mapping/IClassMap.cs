namespace FluentNHibernate.Mapping
{
    public interface IClassMap : IClasslike, IMappingProvider
    {
        string TableName { get; }
        CachePart Cache { get; }
        /// <summary>
        /// Sets the optimistic locking strategy
        /// </summary>
        IOptimisticLockBuilder OptimisticLock { get; }

        /// <summary>
        /// Sets the table for the class.
        /// </summary>
        /// <param name="tableName">Table name</param>
        void WithTable(string tableName);

        /// <summary>
        /// Sets the hibernate-mapping schema for this class.
        /// </summary>
        /// <param name="schema">Schema name</param>
        void SchemaIs(string schema);

        /// <summary>
        /// Sets this entity to be lazy-loaded (overrides the default lazy load configuration).
        /// </summary>
        void LazyLoad();

        /// <summary>
        /// Imports an existing type for use in the mapping.
        /// </summary>
        /// <typeparam name="TImport">Type to import.</typeparam>
        ImportPart ImportType<TImport>();

        /// <summary>
        /// Set the mutability of this class, sets the mutable attribute.
        /// </summary>
        void ReadOnly();

        /// <summary>
        /// Sets this entity to be dynamic update
        /// </summary>
        void DynamicUpdate();

        /// <summary>
        /// Sets this entity to be dynamic insert
        /// </summary>
        void DynamicInsert();

        IClassMap BatchSize(int size);

        /// <summary>
        /// Inverse next boolean
        /// </summary>
        IClassMap Not { get; }

        HibernateMappingPart HibernateMapping { get; }
    }
}