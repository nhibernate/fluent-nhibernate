using System;
using System.Diagnostics;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Mapping.Builders
{
    public class ManyToManyBuilder
    {
        readonly ManyToManyMapping mapping;
        readonly AttributeStore sharedColumnAttributes = new AttributeStore();
        bool nextBool = true;

        public ManyToManyBuilder(ManyToManyMapping mapping)
        {
            this.mapping = mapping;
        }

        /// <summary>
        /// Inverts the next boolean operation
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ManyToManyBuilder Not
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
        /// <typeparam name="TChild">Child type</typeparam>
        public void Type<TChild>()
        {
            mapping.Class = new TypeReference(typeof(TChild));
        }

        /// <summary>
        /// Specifies the type of the values contained in the dictionary, while using the
        /// default column name.
        /// </summary>
        /// <param name="type">Type</param>
        public void Type(Type type)
        {
            mapping.Class = new TypeReference(type);
        }

        /// <summary>
        /// Specifies the type of the values contained in the dictionary, while using the
        /// default column name.
        /// </summary>
        /// <param name="type">Type name</param>
        public void Type(string type)
        {
            mapping.Class = new TypeReference(type);
        }

        /// <summary>
        /// Specifies the entity-name for the relationship
        /// </summary>
        /// <param name="entityName">Entity name</param>
        public void EntityName(string entityName)
        {
            mapping.EntityName = entityName;
        }

        /// <summary>
        /// Specifies the formula for the relationship
        /// </summary>
        /// <param name="formula">Formula</param>
        public void Formula(string formula)
        {
            mapping.Formula = formula;
        }

        /// <summary>
        /// Specifies the not-found behaviour
        /// </summary>
        public NotFoundBuilder NotFound
        {
            get { return new NotFoundBuilder(value => mapping.NotFound = value);}
        }

        /// <summary>
        /// Specifies the fetch behaviour
        /// </summary>
        public FetchTypeBuilder Fetch
        {
            get { return new FetchTypeBuilder(value => mapping.Fetch = value);}
        }

        /// <summary>
        /// Specifies whether this relationship is lazy
        /// </summary>
        public void Lazy()
        {
            mapping.Lazy = nextBool;
            nextBool = true;
        }

        /// <summary>
        /// Specifies the foreign key constraint
        /// </summary>
        public void ForeignKey(string constraint)
        {
            mapping.ForeignKey = constraint;
        }

        /// <summary>
        /// Specifies whether this relationship is unique
        /// </summary>
        public void Unique()
        {
            mapping.Unique = nextBool;
            nextBool = true;
        }

        /// <summary>
        /// Specifies the where clause
        /// </summary>
        public void Where(string where)
        {
            mapping.Where = where;
        }

        /// <summary>
        /// Specifies the order by clause
        /// </summary>
        public void OrderBy(string orderBy)
        {
            mapping.OrderBy = orderBy;
        }

        /// <summary>
        /// Specifies the property ref
        /// </summary>
        public void PropertyRef(string propertyRef)
        {
            mapping.ChildPropertyRef = propertyRef;
        }

        /// <summary>
        /// Specifies the name of the column used for the values of the dictionary, while using
        /// the type inferred from the dictionary signature.
        /// </summary>
        /// <param name="relationshipColumn">Value column name</param>
        public void Column(string relationshipColumn)
        {
            mapping.AddColumn(new ColumnMapping(sharedColumnAttributes) { Name = relationshipColumn });
        }

        /// <summary>
        /// Modify the columns for this element
        /// </summary>
        public ColumnMappingCollection Columns
        {
            get { return new ColumnMappingCollection(mapping, sharedColumnAttributes); }
        }
    }
}