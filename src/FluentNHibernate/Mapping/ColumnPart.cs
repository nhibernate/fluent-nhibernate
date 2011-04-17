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

        /// <summary>
        /// Specify the column name
        /// </summary>
        /// <param name="columnName">Column name</param>
        public ColumnPart Name(string columnName)
        {
            columnMapping.Set(x => x.Name, Layer.UserSupplied, columnName);
            return this;
        }

        /// <summary>
        /// Specify the column length
        /// </summary>
        /// <param name="length">Column length</param>
        public ColumnPart Length(int length)
        {
            columnMapping.Set(x => x.Length, Layer.UserSupplied, length);
            return this;
        }

        /// <summary>
        /// Specify the nullability of the column
        /// </summary>
        public ColumnPart Nullable()
        {
            columnMapping.Set(x => x.NotNull, Layer.UserSupplied, !nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify the uniquness of the column
        /// </summary>
        public ColumnPart Unique()
        {
            columnMapping.Set(x => x.Unique, Layer.UserSupplied, nextBool);
            nextBool = true;
            return this;
        }

        /// <summary>
        /// Specify the unique key constraint name
        /// </summary>
        /// <param name="key">Constraint name</param>
        public ColumnPart UniqueKey(string key)
        {
            columnMapping.Set(x => x.UniqueKey, Layer.UserSupplied, key);
            return this;
        }

        /// <summary>
        /// Specify the SQL type for the column
        /// </summary>
        /// <param name="sqlType">SQL type</param>
        public ColumnPart SqlType(string sqlType)
        {
            columnMapping.Set(x => x.SqlType, Layer.UserSupplied, sqlType);
            return this;
        }

        /// <summary>
        /// Specify the index name
        /// </summary>
        /// <param name="index">Index name</param>
        public ColumnPart Index(string index)
        {
            columnMapping.Set(x => x.Index, Layer.UserSupplied, index);
            return this;
        }
    }
}
