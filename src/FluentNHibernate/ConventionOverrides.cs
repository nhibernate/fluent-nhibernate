using System;
using System.Reflection;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Conventions;

namespace FluentNHibernate
{
    public class ConventionOverrides
    {
        public IConventionFinder Finder { get; private set; }

        internal ConventionOverrides(IConventionFinder conventionFinder)
            : this()
        {
            Finder = conventionFinder;
        }

        public ConventionOverrides()
        {
            DefaultLazyLoad = true;
        }

        /// <summary>
        /// Sets the default length for strings
        /// </summary>
        public int DefaultStringLength { get; set; }

        /// <summary>
        /// Overrides the table name convention
        /// </summary>
        public Func<Type, string> GetTableName;

        /// <summary>
        /// Overrides the primary key column name convention
        /// </summary>
        public Func<IIdentityPart, string> GetPrimaryKeyName;

        /// <summary>
        /// Overrides the foreign key column name convention
        /// </summary>
        public Func<PropertyInfo, string> GetForeignKeyName;

        /// <summary>
        /// Overrides the foreign key column naming for types
        /// </summary>
        public Func<Type, string> GetForeignKeyNameForType;

        /// <summary>
        /// Overrides the naming convention for the backing field of a method accessed collection.
        /// </summary>
        /// <remarks>
        /// Slightly unclear name. This is for this situation:
        /// 
        /// public IEnumerable<Entity> GetEntities()
        /// {
        ///   return entities;
        /// }
        /// 
        /// The convention is used for finding the name of 'entities'.
        /// </remarks>
        public Func<MethodInfo, string> GetMethodCollectionAccessorBackingFieldName;

        #region General alterations

        /// <summary>
        /// Specifies alterations to be made to identity mappings
        /// </summary>
        [Obsolete("Depreciated. All the cool kids are using IIdConvention, see http://wiki.fluentnhibernate.org/show/Conventions")]
        public Action<IIdentityPart> IdConvention;

        /// <summary>
        /// Specifies alterations to be made to HasMany mappings
        /// </summary>
        [Obsolete("Depreciated. All the cool kids are using IHasManyConvention, see http://wiki.fluentnhibernate.org/show/Conventions")]
        public Action<IOneToManyPart> OneToManyConvention;

        /// <summary>
        /// Specifies alterations to be made to Reference mappings
        /// </summary>
        [Obsolete("Depreciated. All the cool kids are using IReferencesConvention, see http://wiki.fluentnhibernate.org/show/Conventions")]
        public Action<IManyToOnePart> ManyToOneConvention;

        /// <summary>
        /// Specifies alterations to be made to Join mappings
        /// </summary>
        [Obsolete("Depreciated. All the cool kids are using IJoinConvention, see http://wiki.fluentnhibernate.org/show/Conventions")]
        public Action<IJoin> JoinConvention;

        /// <summary>
        /// Specifies alterations to be made to HasOne mappings
        /// </summary>
        [Obsolete("Depreciated. All the cool kids are using IHasOneConvention, see http://wiki.fluentnhibernate.org/show/Conventions")]
        public Action<IOneToOnePart> OneToOneConvention;

        #endregion

        /// <summary>
        /// Overrides the default cache setting
        /// </summary>
        public Action<ICache> DefaultCache;

        /// <summary>
        /// Overrides the version column naming
        /// </summary>
        public Func<PropertyInfo, string> GetVersionColumnName;

        /// <summary>
        /// Sets the value of the default-lazy attribute for all entities mapped
        /// </summary>
        public bool DefaultLazyLoad = false;

        /// <summary>
        /// Sets the value of the dynamic-update attribute for the supplied type. Return null for no setting, or a bool otherwise.
        /// </summary>
        public Func<IClassMap, bool?> DynamicUpdate;

        /// <summary>
        /// Sets the value of the dynamic-insert attribute for the supplied type. Return null for no setting, or a bool otherwise.
        /// </summary>
        public Func<IClassMap, bool?> DynamicInsert;

        /// <summary>
        /// Sets the optimistic locking for the supplied type. Use the 2nd parameter to set the locking for the type.
        /// </summary>
        public Action<IClassMap, OptimisticLock> OptimisticLock;

        /// <summary>
        /// Overrides the naming of the many-to-many join table
        /// </summary>
        public Func<IManyToManyPart, string> GetManyToManyTableName;
    }
}
