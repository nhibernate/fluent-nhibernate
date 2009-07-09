using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class KeyInstance : IKeyInstance
    {
        private readonly KeyMapping mapping;

        public KeyInstance(KeyMapping mapping)
        {
            this.mapping = mapping;
        }

        public void ColumnName(string columnName)
        {
            var column = mapping.Columns.FirstOrDefault();
            var columnAttributes = column == null ? new AttributeStore<ColumnMapping>() : column.Attributes.Clone();

            mapping.ClearColumns();
            mapping.AddColumn(new ColumnMapping(columnAttributes) { Name = columnName });
        }

        public Type EntityType
        {
            get { throw new NotImplementedException(); }
        }
        /// <summary>
        /// Represents a string identifier for the model instance, used in conventions for a lazy
        /// shortcut.
        /// 
        /// e.g. for a ColumnMapping the StringIdentifierForModel would be the Name attribute,
        /// this allows the user to find any columns with the matching name.
        /// </summary>
        public string StringIdentifierForModel
        {
            get { throw new NotImplementedException(); }
        }
        public bool IsSet(PropertyInfo property)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IColumnInspector> Columns
        {
            get
            {
                foreach (var column in mapping.Columns.UserDefined)
                    yield return new ColumnInspector(mapping.ContainingEntityType, column);
            }
        }
    }
}