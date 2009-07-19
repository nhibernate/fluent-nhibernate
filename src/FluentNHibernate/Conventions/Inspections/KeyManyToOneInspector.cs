using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Inspections
{
    public class KeyManyToOneInspector : IKeyManyToOneInspector
    {
        private readonly KeyManyToOneMapping mapping;

        public KeyManyToOneInspector(KeyManyToOneMapping mapping)
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

        public Access Access
        {
            get { return Access.FromString(mapping.Access); }
        }

        public TypeReference Class
        {
            get { return mapping.Class; }
        }

        public string ForeignKey
        {
            get { return mapping.ForeignKey; }
        }

        public Laziness LazyLoad
        {
            get { return mapping.Lazy; }
        }

        public string Name
        {
            get { return mapping.Name; }
        }

        public string NotFound
        {
            get { return mapping.NotFound; }
        }

        public IEnumerable<IColumnInspector> Columns
        {
            get
            {
                return mapping.Columns
                    .Select(x => new ColumnInspector(mapping.ContainingEntityType, x))
                    .Cast<IColumnInspector>();
            }
        }
    }
}