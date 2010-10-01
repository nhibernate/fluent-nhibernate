using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping.Builders
{
    public class IndexBuilder
    {
        readonly IndexMapping mapping;
        readonly AttributeStore sharedColumnAttributes = new AttributeStore();

        public IndexBuilder(IndexMapping mapping)
        {
            this.mapping = mapping;
        }

        /// <summary>
        /// Specifies that this index is a many-to-many index. Note: not all methods are available
        /// for many-to-many indexes.
        /// </summary>
        public void AsManyToMany()
        {
            mapping.IsManyToMany = true;
        }

        /// <summary>
        /// Specifies that this index is a one-to-many index. Note: not all methods are available
        /// for one-to-many indexes.
        /// </summary>
        public void AsOneToMany()
        {
            mapping.IsManyToMany = false;
        }

        /// <summary>
        /// Specifies the column name for the index or key of the dictionary.
        /// </summary>
        /// <param name="indexColumnName">Column name</param>
        public void Column(string indexColumnName)
        {
            mapping.AddColumn(new ColumnMapping(sharedColumnAttributes) { Name = indexColumnName });
        }

        /// <summary>
        /// Modify the columns for this element
        /// </summary>
        public ColumnMappingCollection Columns
        {
            get { return new ColumnMappingCollection(mapping, sharedColumnAttributes); }
        }

        /// <summary>
        /// Specifies the type of the index/key column
        /// </summary>
        /// <typeparam name="TIndex">Index type</typeparam>
        public void Type<TIndex>()
        {
            mapping.Type = new TypeReference(typeof(TIndex));
        }

        /// <summary>
        /// Specifies the type of the index/key column
        /// </summary>
        /// <param name="type">Type</param>
        public void Type(Type type)
        {
            mapping.Type = new TypeReference(type);
        }

        /// <summary>
        /// Specifies the type of the index/key column
        /// </summary>
        /// <param name="type">Type</param>
        public void Type(string type)
        {
            mapping.Type = new TypeReference(type);
        }

        /// <summary>
        /// Specifies a foreign key constraint name
        /// </summary>
        /// <param name="constraint">Constraint name</param>
        public void ForeignKey(string constraint)
        {
            mapping.ForeignKey = constraint;
        }

        /// <summary>
        /// Specify the index column length
        /// </summary>
        /// <param name="length">Column length</param>
        public void Length(int length)
        {
            mapping.Length = length;
        }

        /// <summary>
        /// Specify the index entity-name
        /// </summary>
        /// <param name="entityName">Entity name</param>
        public void EntityName(string entityName)
        {
            mapping.EntityName = entityName;
        }
    }
}