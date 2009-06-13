using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class KeyInspector : IKeyInspector
    {
        private readonly KeyMapping mapping;

        public KeyInspector(KeyMapping mapping)
        {
            this.mapping = mapping;
        }

        public Type EntityType
        {
            get { throw new NotImplementedException(); }
        }

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
                    yield return new ColumnInspector(mapping.ContainedEntityType, column);
            }
        }
    }
}