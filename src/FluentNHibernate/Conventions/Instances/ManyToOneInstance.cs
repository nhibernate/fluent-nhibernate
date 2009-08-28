using System;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Instances
{
    public class ManyToOneInstance : ManyToOneInspector, IManyToOneInstance
    {
        private readonly ManyToOneMapping mapping;
        private bool nextBool = true;

        public ManyToOneInstance(ManyToOneMapping mapping)
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

        public void CustomClass<T>()
        {
            if (!mapping.IsSpecified("Class"))
                mapping.Class = new TypeReference(typeof(T));
        }

        public void CustomClass(Type type)
        {
            if (!mapping.IsSpecified("Class"))
                mapping.Class = new TypeReference(type);
        }

        public new IAccessInstance Access
        {
            get
            {
                return new AccessInstance(value =>
                {
                    if (!mapping.IsSpecified("Access"))
                        mapping.Access = value;
                });
            }
        }

        public new ICascadeInstance Cascade
        {
            get
            {
                return new CascadeInstance(value =>
                {
                    if (!mapping.IsSpecified("Cascade"))
                        mapping.Cascade = value;
                });
            }
        }

        public IFetchInstance Fetch
        {
            get
            {
                return new FetchInstance(value =>
                {
                    if (!mapping.IsSpecified("Fetch"))
                        mapping.Fetch = value;
                });
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public IManyToOneInstance Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public INotFoundInstance NotFound
        {
            get
            {
                return new NotFoundInstance(value =>
                {
                    if (!mapping.IsSpecified("NotFound"))
                        mapping.NotFound = value;
                });
            }
        }

        public void Index(string index)
        {
            if (mapping.Columns.First().IsSpecified("Index"))
                return;

            foreach (var column in mapping.Columns)
                column.Index = index;
        }

        public new void Insert()
        {
            if (mapping.IsSpecified("Insert"))
                return;

            mapping.Insert = nextBool;
            nextBool = true;
        }

        public new void LazyLoad()
        {
            if (mapping.IsSpecified("Lazy"))
                return;

            mapping.Lazy = nextBool;
            nextBool = true;
        }

        public void Nullable()
        {
            if (mapping.Columns.First().IsSpecified("NotNull"))
                return;

            foreach (var column in mapping.Columns)
                column.NotNull = !nextBool;

            nextBool = true;
        }

        public new void PropertyRef(string property)
        {
            if (!mapping.IsSpecified("PropertyRef"))
                mapping.PropertyRef = property;
        }

        public void ReadOnly()
        {
            if (mapping.IsSpecified("Insert") || mapping.IsSpecified("Update"))
                return;

            mapping.Insert = !nextBool;
            mapping.Update = !nextBool;
            nextBool = true;
        }

        public void Unique()
        {
            if (mapping.Columns.First().IsSpecified("Unique"))
                return;

            foreach (var column in mapping.Columns)
                column.Unique = nextBool;

            nextBool = true;
        }

        public void UniqueKey(string key)
        {
            if (mapping.Columns.First().IsSpecified("UniqueKey"))
                return;

            foreach (var column in mapping.Columns)
                column.UniqueKey = key;
        }

        public new void Update()
        {
            if (mapping.IsSpecified("Update"))
                return;

            mapping.Update = nextBool;
            nextBool = true;
        }

        public new void ForeignKey(string key)
        {
            if (!mapping.IsSpecified("ForeignKey"))
                mapping.ForeignKey = key;
        }
    }
}