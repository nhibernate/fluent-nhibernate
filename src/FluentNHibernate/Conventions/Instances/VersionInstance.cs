using System;
using System.Reflection;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class VersionInstance : IVersionInstance
    {
        private readonly VersionMapping mapping;

        public VersionInstance(VersionMapping mapping)
        {
            this.mapping = mapping;
        }

        public Type EntityType
        {
            get { return mapping.ContainingEntityType; }
        }

        public string StringIdentifierForModel
        {
            get { return mapping.Name; }
        }

        public bool IsSet(PropertyInfo property)
        {
            throw new NotImplementedException();
        }

        public IAccessInstance Access
        {
            get
            {
                return new AccessInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Access))
                        mapping.Access = value;
                });
            }
        }

        public IGeneratedInstance Generated
        {
            get
            {
                return new GeneratedInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Generated))
                        mapping.Generated = value;
                });
            }
        }

        public void ColumnName(string columnName)
        {
            if (!mapping.IsSpecified(x => x.Column))
                mapping.Column = columnName;
        }

        public void UnsavedValue(string unsavedValue)
        {
            if (!mapping.IsSpecified(x => x.UnsavedValue))
                mapping.UnsavedValue = unsavedValue;
        }
    }
}