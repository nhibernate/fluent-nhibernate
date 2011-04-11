using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping.Builders
{
    public class ListIndexBuilder
    {
        readonly IndexMapping mapping;
        readonly AttributeStore sharedColumnAttributes = new AttributeStore();
    
        public ListIndexBuilder(IndexMapping mapping)
        {
            this.mapping = mapping;
            mapping.IsManyToMany = false;
        }

        /// <summary>
        /// offset added to index in column
        /// </summary>
        /// <remarks>mutual exclusive with Type()</remarks>
        /// <param name="offset">offset</param>
        public void Offset(int offset)
        {
            if (mapping.HasValue(x => x.Type))
                throw new NotSupportedException("Offset is mutual exclusive with Type()");
            mapping.Offset = offset;
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
        /// Specifies the type of the index/key column
        /// </summary>
        /// <remarks>mutual exclusive with offset</remarks>
        /// <typeparam name="TIndex">Index type</typeparam>
        public void Type<TIndex>()
        {
            if (mapping.HasValue(x => x.Offset))
                throw new NotSupportedException("Type() is mutual exclusive with Offset()");
            mapping.Type = new TypeReference(typeof(TIndex));
        }

        /// <summary>
        /// Specifies the type of the index/key column
        /// </summary>
        /// <remarks>mutual exclusive with offset</remarks>
        /// <param name="type">Type</param>
        public void Type(Type type)
        {
            if (mapping.HasValue(x => x.Offset))
                throw new NotSupportedException("Type() is mutual exclusive with Offset()");
            mapping.Type = new TypeReference(type);
        }

        /// <summary>
        /// Specifies the type of the index/key column
        /// </summary>
        /// <remarks>mutual exclusive with offset</remarks>
        /// <param name="type">Type</param>
        public void Type(string type)
        {
            if (mapping.HasValue(x => x.Offset))
                throw new NotSupportedException("Type() is mutual exclusive with Offset()");
            mapping.Type = new TypeReference(type);
        }
    }
}
