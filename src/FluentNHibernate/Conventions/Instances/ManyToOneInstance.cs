using System;
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

        public void ColumnName(string columnName)
        {
            if (mapping.Columns.UserDefined.Count() > 0)
                return;

            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : ColumnMapping.BaseOn(originalColumn);

            column.Name = columnName;

            mapping.ClearColumns();
            mapping.AddColumn(column);
        }

        public void Class<T>()
        {
            if (!mapping.IsSpecified(x => x.Class))
                mapping.Class = new TypeReference(typeof(T));
        }

        public void Class(Type type)
        {
            if (!mapping.IsSpecified(x => x.Class))
                mapping.Class = new TypeReference(type);
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

        public new ICascadeInstance Cascade
        {
            get
            {
                return new CascadeInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.Cascade))
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
                    if (!mapping.IsSpecified(x => x.Fetch))
                        mapping.Fetch = value;
                });
            }
        }

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
                    if (!mapping.IsSpecified(x => x.NotFound))
                        mapping.NotFound = value;
                });
            }
        }

        public new IOuterJoinInstance OuterJoin
        {
            get
            {
                return new OuterJoinInstance(value =>
                {
                    if (!mapping.IsSpecified(x => x.OuterJoin))
                        mapping.OuterJoin = value;
                });
            }
        }

        public void Index(string index)
        {
            if (mapping.Columns.First().IsSpecified(x => x.Index))
                return;

            foreach (var column in Columns)
                column.Index = index;
        }

        public void Insert()
        {
            if (mapping.IsSpecified(x => x.Insert))
                return;

            mapping.Insert = nextBool;
            nextBool = true;
        }

        public void LazyLoad()
        {
            if (mapping.IsSpecified(x => x.Lazy))
                return;

            mapping.Lazy = nextBool ? Laziness.Proxy : Laziness.False;
            nextBool = true;
        }

        public void Nullable()
        {
            if (mapping.Columns.First().IsSpecified(x => x.NotNull))
                return;

            foreach (var column in Columns)
                column.NotNull = !nextBool;

            nextBool = true;
        }

        public void PropertyRef(string property)
        {
            if (!mapping.IsSpecified(x => x.PropertyRef))
                mapping.PropertyRef = property;
        }

        public void ReadOnly()
        {
            if (mapping.IsSpecified(x => x.Insert) || mapping.IsSpecified(x => x.Update))
                return;

            mapping.Insert = nextBool;
            mapping.Update = nextBool;
            nextBool = true;
        }

        public void Unique()
        {
            if (mapping.Columns.First().IsSpecified(x => x.Unique))
                return;

            foreach (var column in Columns)
                column.Unique = nextBool;

            nextBool = true;
        }

        public void UniqueKey(string key)
        {
            if (mapping.Columns.First().IsSpecified(x => x.UniqueKey))
                return;

            foreach (var column in Columns)
                column.UniqueKey = key;
        }

        public void Update()
        {
            if (mapping.IsSpecified(x => x.Update))
                return;

            mapping.Update = nextBool;
            nextBool = true;
        }

        public void ForeignKey(string key)
        {
            if (!mapping.IsSpecified(x => x.ForeignKey))
                mapping.ForeignKey = key;
        }
    }
}