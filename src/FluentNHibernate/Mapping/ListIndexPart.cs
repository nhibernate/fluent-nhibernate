using System;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping
{
    public class ListIndexPart
    {
        readonly IndexMapping mapping;
        readonly AttributeStore sharedColumnAttributes = new AttributeStore();
    
        public ListIndexPart(IndexMapping mapping)
        {
            this.mapping = mapping;
        }

        /// <summary>
        /// offset added to index in column
        /// </summary>
        /// <remarks>mutual exclusive with Type()</remarks>
        /// <param name="offset">offset</param>
        public void Offset(int offset)
        {
            if (mapping.IsSpecified("Type"))
                throw new NotSupportedException("Offset is mutual exclusive with Type()");

            mapping.Set(x => x.Offset, Layer.UserSupplied, offset);
        }
        
        /// <summary>
        /// Specifies the column name for the index or key of the dictionary.
        /// </summary>
        /// <param name="indexColumnName">Column name</param>
        public void Column(string indexColumnName)
        {
            var column = new ColumnMapping(sharedColumnAttributes);
            column.Set(x => x.Name, Layer.UserSupplied, indexColumnName);
            mapping.AddColumn(Layer.UserSupplied, column);
        }

        /// <summary>
        /// Specifies the type of the index/key column
        /// </summary>
        /// <remarks>mutual exclusive with offset</remarks>
        /// <typeparam name="TIndex">Index type</typeparam>
        public void Type<TIndex>()
        {
            if (mapping.IsSpecified("Offset"))
                throw new NotSupportedException("Type() is mutual exclusive with Offset()");

            mapping.Set(x => x.Type, Layer.UserSupplied, new TypeReference(typeof(TIndex)));
        }

        /// <summary>
        /// Specifies the type of the index/key column
        /// </summary>
        /// <remarks>mutual exclusive with offset</remarks>
        /// <param name="type">Type</param>
        public void Type(Type type)
        {
            if (mapping.IsSpecified("Offset"))
                throw new NotSupportedException("Type() is mutual exclusive with Offset()");
         
            mapping.Set(x => x.Type, Layer.UserSupplied, new TypeReference(type));
        }

        /// <summary>
        /// Specifies the type of the index/key column
        /// </summary>
        /// <remarks>mutual exclusive with offset</remarks>
        /// <param name="type">Type</param>
        public void Type(string type)
        {
            if (mapping.IsSpecified("Offset"))
                throw new NotSupportedException("Type() is mutual exclusive with Offset()");

            mapping.Set(x => x.Type, Layer.UserSupplied, new TypeReference(type));
        }
    }
}
