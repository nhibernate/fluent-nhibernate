using System;
using System.Diagnostics;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping.Builders
{
    public class ElementBuilder
    {
        readonly ElementMapping mapping;
        bool nextBool = true;
        readonly AttributeStore sharedColumnAttributes = new AttributeStore();

        public ElementBuilder(ElementMapping mapping)
        {
            this.mapping = mapping;
        }

        /// <summary>
        /// Inverts the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ElementBuilder Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        /// <summary>
        /// Specifies the type of the values contained in the dictionary, while using the
        /// default column name.
        /// </summary>
        /// <typeparam name="TElementType">Value type</typeparam>
        public void Type<TElementType>()
        {
            mapping.Type = new TypeReference(typeof(TElementType));
        }

        /// <summary>
        /// Specifies the type of the values contained in the dictionary, while using the
        /// default column name.
        /// </summary>
        /// <param name="type">Type</param>
        public void Type(Type type)
        {
            mapping.Type = new TypeReference(type);
        }

        /// <summary>
        /// Specifies the type of the values contained in the dictionary, while using the
        /// default column name.
        /// </summary>
        /// <param name="type">Type name</param>
        public void Type(string type)
        {
            mapping.Type = new TypeReference(type);
        }

        /// <summary>
        /// Specifies the name of the column used for the values of the dictionary, while using
        /// the type inferred from the dictionary signature.
        /// </summary>
        /// <param name="elementColumnName">Value column name</param>
        public void Column(string elementColumnName)
        {
            mapping.AddColumn(new ColumnMapping(sharedColumnAttributes) { Name = elementColumnName });
        }

        /// <summary>
        /// Modify the columns for this element
        /// </summary>
        public ColumnMappingCollection Columns
        {
            get { return new ColumnMappingCollection(mapping, sharedColumnAttributes); }
        }

        /// <summary>
        /// Specify the element column length
        /// </summary>
        /// <param name="length">Column length</param>
        public void Length(int length)
        {
            mapping.Length = length;
        }

        /// <summary>
        /// Specify the element column formula
        /// </summary>
        /// <param name="formula">Formula</param>
        public void Formula(string formula)
        {
            mapping.Formula = formula;
        }

        /// <summary>
        /// Specify the precision for decimal types
        /// </summary>
        /// <param name="precision">Precision</param>
        public void Precision(int precision)
        {
            mapping.Precision = precision;
        }

        /// <summary>
        /// Specify the scale for decimal types
        /// </summary>
        /// <param name="scale">Scale</param>
        public void Scale(int scale)
        {
            mapping.Scale = scale;
        }

        /// <summary>
        /// Specify the precision and scale for decimal types
        /// </summary>
        /// <param name="precision">Precision</param>
        /// <param name="scale">Scale</param>
        public void PrecisionAndScale(int precision, int scale)
        {
            Precision(precision);
            Scale(scale);
        }

        /// <summary>
        /// Specifies that the element is nullable (or not, if the <see cref="Not"/>
        /// switch is used)
        /// </summary>
        public void Nullable()
        {
            mapping.NotNull = !nextBool;
            nextBool = true;
        }

        /// <summary>
        /// Specifies that the element should be unique (or not, if the <see cref="Not"/>
        /// switch is used)
        /// </summary>
        public void Unique()
        {
            mapping.Unique = nextBool;
            nextBool = true;
        }
    }
}