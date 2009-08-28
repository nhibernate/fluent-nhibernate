using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Conventions.Inspections
{
    public abstract class ColumnBasedInspector
    {
        private readonly IEnumerable<ColumnMapping> columns;

        protected ColumnBasedInspector(IEnumerable<ColumnMapping> columns)
        {
            this.columns = columns;
        }

        /// <summary>
        /// Gets the requested value off the first column, as all columns are (currently) created equal
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private T GetValueFromColumns<T>(Func<ColumnMapping, object> property)
        {
            var column = columns.FirstOrDefault();

            if (column != null)
                return (T)property(column);

            return default(T);
        }

        public int Length
        {
            get { return GetValueFromColumns<int>(x => x.Length); }
        }

        public bool Nullable
        {
            get { return !GetValueFromColumns<bool>(x => x.NotNull); }
        }

        public string SqlType
        {
            get { return GetValueFromColumns<string>(x => x.SqlType); }
        }

        public bool Unique
        {
            get { return GetValueFromColumns<bool>(x => x.Unique); }
        }

        public string UniqueKey
        {
            get { return GetValueFromColumns<string>(x => x.UniqueKey); }
        }

        public string Index
        {
            get { return GetValueFromColumns<string>(x => x.Index); }
        }

        public string Check
        {
            get { return GetValueFromColumns<string>(x => x.Check); }
        }

        public string Default
        {
            get { return GetValueFromColumns<string>(x => x.Default); }
        }

        public int Precision
        {
            get { return GetValueFromColumns<int>(x => x.Precision); }
        }

        public int Scale
        {
            get { return GetValueFromColumns<int>(x => x.Scale); }
        }
    }
}