using System;
using System.Reflection;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class VersionInstance : VersionInspector, IVersionInstance
    {
        private readonly VersionMapping mapping;

        public VersionInstance(VersionMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
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

        public new void Column(string columnName)
        {
            if (!mapping.IsSpecified(x => x.Column))
                mapping.Column = columnName;
        }

        public new void UnsavedValue(string unsavedValue)
        {
            if (!mapping.IsSpecified(x => x.UnsavedValue))
                mapping.UnsavedValue = unsavedValue;
        }
    }
}