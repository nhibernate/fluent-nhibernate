namespace FluentNHibernate.Mapping
{
    public interface IClassMap : IClasslike, IHasAttributes, IMappingProvider
    {
        string TableName { get; }
        ICache Cache { get; }
        Cache<string, string> Attributes { get; }
        Cache<string, string> HibernateMappingAttributes { get; }
        /// <summary>
        /// Sets the optimistic locking strategy
        /// </summary>
        IOptimisticLockBuilder OptimisticLock { get; }

        /// <summary>
        /// Sets the table for the class.
        /// </summary>
        /// <param name="tableName">Table name</param>
        void WithTable(string tableName);

        void SetHibernateMappingAttribute(string name, string value);
        void SetHibernateMappingAttribute(string name, bool value);

        /// <summary>
        /// Sets the hibernate-mapping schema for this class.
        /// </summary>
        /// <param name="schema">Schema name</param>
        void SchemaIs(string schema);

        /// <summary>
        /// Sets the hibernate-mapping auto-import for this class.
        /// </summary>
        void AutoImport();

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
    }
}