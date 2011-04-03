using System;
using System.Diagnostics;
using System.Linq;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
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
            var column = originalColumn == null ? new ColumnMapping() : originalColumn.Clone();

            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }
        
        public new void Formula(string formula)
        {
            if (!mapping.IsSpecified("Formula"))
            {
                mapping.Formula = formula;
                mapping.ClearColumns();
            }
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

        new public IFetchInstance Fetch
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

        public new INotFoundInstance NotFound
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
            if (!mapping.IsSpecified("Insert"))
                mapping.Insert = nextBool;
            nextBool = true;
        }

        public new void OptimisticLock()
        {
            if (!mapping.IsSpecified("OptimisticLock"))
                mapping.OptimisticLock = nextBool;
            nextBool = true;
        }

        public new void LazyLoad()
        {
            if (!mapping.IsSpecified("Lazy"))
            {
                if (nextBool)
                    LazyLoad(Laziness.Proxy);
                else
                    LazyLoad(Laziness.False);
            }
            nextBool = true;
        }

        public new void LazyLoad(Laziness laziness)
        {
            mapping.Lazy = laziness.ToString();
            nextBool = true;
        }

        public new void Nullable()
        {
            if (!mapping.Columns.First().IsSpecified("NotNull"))
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
            if (!mapping.IsSpecified("Insert") && !mapping.IsSpecified("Update"))
            {
                mapping.Insert = !nextBool;
                mapping.Update = !nextBool;
            }
            nextBool = true;
        }

        public void Unique()
        {
            if (!mapping.Columns.First().IsSpecified("Unique"))
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
            if (!mapping.IsSpecified("Update"))
                mapping.Update = nextBool;
            nextBool = true;
        }

        public new void ForeignKey(string key)
        {
            if (!mapping.IsSpecified("ForeignKey"))
                mapping.ForeignKey = key;
        }

        public void OverrideInferredClass(Type type)
        {
            mapping.Class = new TypeReference(type);
        }
    }
}