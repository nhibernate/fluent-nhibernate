using System;
using System.Diagnostics;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping.Builders
{
    public class KeyBuilder
    {
        readonly KeyMapping mapping;
        readonly AttributeStore sharedColumnAttributes = new AttributeStore();
        bool nextBool = true;

        public KeyBuilder(KeyMapping mapping)
        {
            this.mapping = mapping;
            Columns = new ColumnMappingCollection(mapping, sharedColumnAttributes);
        }

        /// <summary>
        /// Inverts the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public KeyBuilder Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public ColumnMappingCollection Columns { get; set; }

        /// <summary>
        /// Sets the column name
        /// </summary>
        /// <param name="keyColumnName"></param>
        public void Column(string keyColumnName)
        {
            mapping.AddColumn(new ColumnMapping(sharedColumnAttributes) { Name = keyColumnName });
        }

        /// <summary>
        /// This method is used to set a different key column in this table to be used for joins.
        /// The output is set as the property-ref attribute in the "key" subelement of the collection
        /// </summary>
        /// <param name="propertyRef">The name of the column in this table which is linked to the foreign key</param>
        public void PropertyRef(string propertyRef)
        {
            mapping.PropertyRef = propertyRef;
        }

        /// <summary>
        /// Specifies that the foreign key should cascade when deleted.
        /// </summary>
        public void CascadeOnDelete()
        {
            mapping.OnDelete = "cascade";
        }

        /// <summary>
        /// Specifies the foreign key constraint name
        /// </summary>
        /// <param name="foreignKeyName">Constraint name</param>
        public void ForeignKey(string foreignKeyName)
        {
            mapping.ForeignKey = foreignKeyName;
        }

        /// <summary>
        /// Specifies that the foreign key is updatable.
        /// </summary>
        public void Update()
        {
            mapping.Update = nextBool;
            nextBool = true;
        }

        /// <summary>
        /// Specifies that the foreign key is unique.
        /// </summary>
        public void Unique()
        {
            mapping.Unique = nextBool;
            nextBool = true;
        }

        /// <summary>
        /// Specify that the key is nullable.
        /// </summary>
        public void Nullable()
        {
            mapping.NotNull = !nextBool;
            nextBool = true;
        }
    }
}