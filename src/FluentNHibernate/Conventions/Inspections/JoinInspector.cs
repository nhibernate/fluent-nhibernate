using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public class JoinInspector : IJoinInspector
    {
        private readonly JoinMapping mapping;

        public JoinInspector(JoinMapping mapping)
        {
            this.mapping = mapping;
        }

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.TableName; }
        }

        public bool IsSet(PropertyInfo property)
        {
            throw new NotImplementedException();
        }
    }
}