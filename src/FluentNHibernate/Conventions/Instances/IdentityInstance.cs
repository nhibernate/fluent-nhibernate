using System;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Instances
{
    public class IdentityInstance : IdentityInspector, IIdentityInstance
    {
        private readonly IdMapping mapping;

        public IdentityInstance(IdMapping mapping)
            : base(mapping)
        {
            this.mapping = mapping;
        }

        public void Column(string columnName)
        {
            if (mapping.Columns.UserDefined.Count() > 0)
                return;

            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : ColumnMapping.BaseOn(originalColumn);

            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }

        public new void UnsavedValue(string unsavedValue)
        {
            if (!mapping.IsSpecified(x => x.UnsavedValue))
                mapping.UnsavedValue = unsavedValue;
        }

        public new void Length(int length)
        {
            if (!mapping.IsSpecified(x => x.Length))
                mapping.Length = length;
        }

        public new void Type(Type type)
        {
            if (!mapping.IsSpecified(x => x.Type))
                mapping.Type = new TypeReference(type);
        }

        public new void Type<T>()
        {
            Type(typeof(T));
        }

        public new IAccessInstance Access
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

        public IGeneratorInstance GeneratedBy
        {
            get
            {
                if (!mapping.IsSpecified(x => x.Generator))
                    mapping.Generator = new GeneratorMapping();
                
                return new GeneratorInstance(mapping.Generator, mapping.Type.GetUnderlyingSystemType());
            }
        }
    }
}