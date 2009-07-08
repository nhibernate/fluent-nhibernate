using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Collections;

namespace FluentNHibernate.Conventions.Inspections
{
    public class ManyToManyInspector : IManyToManyInspector
    {
        private readonly ManyToManyMapping mapping;

        public ManyToManyInspector(ManyToManyMapping mapping)
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

        public IDefaultableEnumerable<IColumnInspector> Columns
        {
            get
            {
                var items = new DefaultableList<IColumnInspector>();

                foreach (var column in mapping.Columns)
                {
                    items.Add(new ColumnInspector(mapping.ParentType, column));
                }

                return items;
            }
        }
    }
}