using System;
using System.Collections.Generic;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Conventions.Instances
{
    public class GeneratorInstance : GeneratorInspector, IGeneratorInstance
    {
        private readonly GeneratorMapping mapping;
        private readonly GeneratorBuilder builder;

        public GeneratorInstance(GeneratorMapping mapping, Type type)
            : base(mapping)
        {
            this.mapping = mapping;
            builder = new GeneratorBuilder(mapping, type);
        }

		/// <summary>
		/// generates identifiers of any integral type that are unique only when no other 
		/// process is inserting data into the same table. Do not use in a cluster.
		/// </summary>
		/// <returns></returns>
        public void Increment()
		{
            if (!mapping.IsSpecified(x => x.Class))
			    builder.Increment();
		}

        /// <summary>
        /// generates identifiers of any integral type that are unique only when no other 
        /// process is inserting data into the same table. Do not use in a cluster.
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void Increment(Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.Increment(paramValues);
        }

		/// <summary>
		/// supports identity columns in DB2, MySQL, MS SQL Server and Sybase.
		/// The identifier returned by the database is converted to the property type using 
		/// Convert.ChangeType. Any integral property type is thus supported.
		/// </summary>
		/// <returns></returns>
        public void Identity()
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.Identity();
		}

        /// <summary>
        /// supports identity columns in DB2, MySQL, MS SQL Server and Sybase.
        /// The identifier returned by the database is converted to the property type using 
        /// Convert.ChangeType. Any integral property type is thus supported.
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void Identity(Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.Identity(paramValues);
        }

		/// <summary>
		/// uses a sequence in DB2, PostgreSQL, Oracle or a generator in Firebird.
		/// The identifier returned by the database is converted to the property type 
		/// using Convert.ChangeType. Any integral property type is thus supported.
		/// </summary>
		/// <param name="sequenceName"></param>
		/// <returns></returns>
        public void Sequence(string sequenceName)
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.Sequence(sequenceName);
		}

        /// <summary>
        /// uses a sequence in DB2, PostgreSQL, Oracle or a generator in Firebird.
        /// The identifier returned by the database is converted to the property type 
        /// using Convert.ChangeType. Any integral property type is thus supported.
        /// </summary>
        /// <param name="sequenceName"></param>
        /// <param name="paramValues">Params configuration</param>
        public void Sequence(string sequenceName, Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.Sequence(sequenceName, paramValues);
        }

		/// <summary>
		/// uses a hi/lo algorithm to efficiently generate identifiers of any integral type, 
		/// given a table and column (by default hibernate_unique_key and next_hi respectively) 
		/// as a source of hi values. The hi/lo algorithm generates identifiers that are unique 
		/// only for a particular database. Do not use this generator with a user-supplied connection.
		/// requires a "special" database table to hold the next available "hi" value
		/// </summary>
		/// <param name="table"></param>
		/// <param name="column"></param>
		/// <param name="maxLo"></param>
		/// <returns></returns>
        public void HiLo(string table, string column, string maxLo)
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.HiLo(table, column, maxLo);
		}

        /// <summary>
        /// uses a hi/lo algorithm to efficiently generate identifiers of any integral type, 
        /// given a table and column (by default hibernate_unique_key and next_hi respectively) 
        /// as a source of hi values. The hi/lo algorithm generates identifiers that are unique 
        /// only for a particular database. Do not use this generator with a user-supplied connection.
        /// requires a "special" database table to hold the next available "hi" value
        /// </summary>
        /// <param name="table"></param>
        /// <param name="column"></param>
        /// <param name="maxLo"></param>
        /// <param name="paramValues">Params configuration</param>
        public void HiLo(string table, string column, string maxLo, Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.HiLo(table, column, maxLo, paramValues);
        }

		/// <summary>
		/// uses a hi/lo algorithm to efficiently generate identifiers of any integral type, 
		/// given a table and column (by default hibernate_unique_key and next_hi respectively) 
		/// as a source of hi values. The hi/lo algorithm generates identifiers that are unique 
		/// only for a particular database. Do not use this generator with a user-supplied connection.
		/// requires a "special" database table to hold the next available "hi" value
		/// </summary>
		/// <param name="maxLo"></param>
		/// <returns></returns>
        public void HiLo(string maxLo)
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.HiLo(maxLo);
		}

        /// <summary>
        /// uses a hi/lo algorithm to efficiently generate identifiers of any integral type, 
        /// given a table and column (by default hibernate_unique_key and next_hi respectively) 
        /// as a source of hi values. The hi/lo algorithm generates identifiers that are unique 
        /// only for a particular database. Do not use this generator with a user-supplied connection.
        /// requires a "special" database table to hold the next available "hi" value
        /// </summary>
        /// <param name="maxLo"></param>
        /// <param name="paramValues">Params configuration</param>
        public void HiLo(string maxLo, Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.HiLo(maxLo, paramValues);
        }

		/// <summary>
		/// uses an Oracle-style sequence (where supported)
		/// </summary>
		/// <param name="sequence"></param>
		/// <param name="maxLo"></param>
		/// <returns></returns>
        public void SeqHiLo(string sequence, string maxLo)
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.SeqHiLo(sequence, maxLo);
		}

        /// <summary>
        /// uses an Oracle-style sequence (where supported)
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="maxLo"></param>
        /// <param name="paramValues">Params configuration</param>
        public void SeqHiLo(string sequence, string maxLo, Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.SeqHiLo(sequence, maxLo, paramValues);
        }

		/// <summary>
		/// uses System.Guid and its ToString(string format) method to generate identifiers
		/// of type string. The length of the string returned depends on the configured format. 
		/// </summary>
		/// <param name="format">http://msdn.microsoft.com/en-us/library/97af8hh4.aspx</param>
		/// <returns></returns>
        public void UuidHex(string format)
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.UuidHex(format);
		}

        /// <summary>
        /// uses System.Guid and its ToString(string format) method to generate identifiers
        /// of type string. The length of the string returned depends on the configured format. 
        /// </summary>
        /// <param name="format">http://msdn.microsoft.com/en-us/library/97af8hh4.aspx</param>
        /// <param name="paramValues">Params configuration</param>
        public void UuidHex(string format, Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.UuidHex(format, paramValues);
        }

		/// <summary>
		/// uses a new System.Guid to create a byte[] that is converted to a string.  
		/// </summary>
		/// <returns></returns>
        public void UuidString()
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.UuidString();
		}

        /// <summary>
        /// uses a new System.Guid to create a byte[] that is converted to a string.  
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void UuidString(Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.UuidString(paramValues);
        }

		/// <summary>
		/// uses a new System.Guid as the identifier. 
		/// </summary>
		/// <returns></returns>
        public void Guid()
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.Guid();
		}

        /// <summary>
        /// uses a new System.Guid as the identifier. 
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void Guid(Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.Guid(paramValues);
        }

		/// <summary>
		/// Recommended for Guid identifiers!
		/// uses the algorithm to generate a new System.Guid described by Jimmy Nilsson 
		/// in the article http://www.informit.com/articles/article.asp?p=25862. 
		/// </summary>
		/// <returns></returns>
        public void GuidComb()
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.GuidComb();
		}

        /// <summary>
        /// Recommended for Guid identifiers!
        /// uses the algorithm to generate a new System.Guid described by Jimmy Nilsson 
        /// in the article http://www.informit.com/articles/article.asp?p=25862. 
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void GuidComb(Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.GuidComb(paramValues);
        }

		/// <summary>
		/// lets the application to assign an identifier to the object before Save() is called. 
		/// </summary>
		/// <returns></returns>
        public void Assigned()
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.Assigned();
		}

        /// <summary>
        /// lets the application to assign an identifier to the object before Save() is called. 
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void Assigned(Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.Assigned(paramValues);
        }

		/// <summary>
		/// picks identity, sequence or hilo depending upon the capabilities of the underlying database. 
		/// </summary>
		/// <returns></returns>
        public void Native()
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.Native();
		}

        /// <summary>
        /// picks identity, sequence or hilo depending upon the capabilities of the underlying database. 
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void Native(Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.Native(paramValues);
        }

		/// <summary>
		/// uses the identifier of another associated object. Usually used in conjunction with a one-to-one primary key association. 
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
        public void Foreign(string property)
		{
            if (!mapping.IsSpecified(x => x.Class))
                builder.Foreign(property);
		}

        /// <summary>
        /// uses the identifier of another associated object. Usually used in conjunction with a one-to-one primary key association. 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="paramValues">Params configuration</param>
        public void Foreign(string property, Action<ParamBuilder> paramValues)
        {
            if (!mapping.IsSpecified(x => x.Class))
                builder.Foreign(property, paramValues);
        }
    }
}