using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public class ColumnPart
    {
        private ColumnMapping columnMapping;
        private bool nextBool = true;

        public ColumnPart(ColumnMapping columnMapping)
        {
            this.columnMapping = columnMapping;
        }

        /// <summary>
        /// Inverts the next boolean
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public ColumnPart Not
        {
            get
            {
                nextBool = !nextBool;
                return this;
            }
        }

        public ColumnPart Name(string columnName)
        {
            columnMapping.Name = columnName;
            return this;
        }

        public ColumnPart Length(int length)
        {
            columnMapping.Length = length;
            return this;
        }

        public ColumnPart Nullable()
        {
            columnMapping.NotNull = !nextBool;
            nextBool = true;
            return this;
        }

        public ColumnPart Unique()
        {
            columnMapping.Unique = nextBool;
            nextBool = true;
            return this;
        }

        public ColumnPart UniqueKey(string key1)
        {
            columnMapping.UniqueKey = key1;
            return this;
        }

        public ColumnPart SqlType(string sqlType)
        {
            columnMapping.SqlType = sqlType;
            return this;
        }

        public ColumnPart Index(string index)
        {
            columnMapping.Index = index;
            return this;
        }
    }
}
