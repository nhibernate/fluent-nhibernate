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
            var originalColumn = mapping.Columns.FirstOrDefault();
            var column = originalColumn == null ? new ColumnMapping() : originalColumn.Clone();

            column.Set(x => x.Name, Layer.Conventions, columnName);

            mapping.AddColumn(Layer.Conventions, column);
        }
        
        public new void Formula(string formula)
        {
            mapping.Set(x => x.Formula, Layer.Conventions, formula);
            mapping.MakeColumnsEmpty(Layer.UserSupplied);
        }

        public void CustomClass<T>()
        {
            CustomClass(typeof(T));
        }

        public void CustomClass(Type type)
        {
            mapping.Set(x => x.Class, Layer.Conventions, new TypeReference(type));
        }

        public new IAccessInstance Access
        {
            get { return new AccessInstance(value => mapping.Set(x => x.Access, Layer.Conventions, value)); }
        }

        public new ICascadeInstance Cascade
        {
            get { return new CascadeInstance(value => mapping.Set(x => x.Cascade, Layer.Conventions, value)); }
        }

        new public IFetchInstance Fetch
        {
            get { return new FetchInstance(value => mapping.Set(x => x.Fetch, Layer.Conventions, value)); }
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
            get { return new NotFoundInstance(value => mapping.Set(x => x.NotFound, Layer.Conventions, value)); }
        }

        public void Index(string index)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Index, Layer.Conventions, index);
        }

        public new void Insert()
        {
            mapping.Set(x => x.Insert, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void OptimisticLock()
        {
            mapping.Set(x => x.OptimisticLock, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void LazyLoad()
        {
            LazyLoad(nextBool ? Laziness.Proxy : Laziness.False);
            nextBool = true;
        }

        public new void LazyLoad(Laziness laziness)
        {
            mapping.Set(x => x.Lazy, Layer.Conventions, laziness.ToString());
            nextBool = true;
        }

        public new void Nullable()
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.NotNull, Layer.Conventions, !nextBool);

            nextBool = true;
        }

        public new void PropertyRef(string property)
        {
            mapping.Set(x => x.PropertyRef, Layer.Conventions, property);
        }

        public void ReadOnly()
        {
            mapping.Set(x => x.Insert, Layer.Conventions, !nextBool);
            mapping.Set(x => x.Update, Layer.Conventions, !nextBool);
            nextBool = true;
        }

        public void Unique()
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.Unique, Layer.Conventions, nextBool);

            nextBool = true;
        }

        public void UniqueKey(string key)
        {
            foreach (var column in mapping.Columns)
                column.Set(x => x.UniqueKey, Layer.Conventions, key);
        }

        public new void Update()
        {
            mapping.Set(x => x.Update, Layer.Conventions, nextBool);
            nextBool = true;
        }

        public new void ForeignKey(string key)
        {
            mapping.Set(x => x.ForeignKey, Layer.Conventions, key);
        }

        public void OverrideInferredClass(Type type)
        {
            mapping.Set(x => x.Class, Layer.Conventions, new TypeReference(type));
        }
    }
}