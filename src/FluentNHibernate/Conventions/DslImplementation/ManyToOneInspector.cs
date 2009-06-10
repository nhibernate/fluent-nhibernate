using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.DslImplementation
{
    public class ManyToOneInspector : IManyToOneInspector
    {
        private readonly ManyToOneMapping mapping;

        public ManyToOneInspector(ManyToOneMapping mapping)
        {
            this.mapping = mapping;
        }

        public Access Access
        {
            get { throw new NotImplementedException(); }
        }
        public Cascade Cascade
        {
            get { throw new NotImplementedException(); }
        }
        public OuterJoin OuterJoin
        {
            get { throw new NotImplementedException(); }
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

        public PropertyInfo Property
        {
            get { return mapping.PropertyInfo; }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public TypeReference Class
        {
            get { return mapping.Class; }
        }

        public IEnumerable<IColumnInspector> Columns
        {
            get
            {
                foreach (var column in mapping.Columns)
                {
                    yield return new ColumnInspector(mapping.ContainedEntityType, column);
                }
            }
        }
    }
}