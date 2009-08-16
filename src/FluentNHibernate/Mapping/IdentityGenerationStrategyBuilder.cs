using System;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping
{
    public class IdentityGenerationStrategyBuilder<TParent>
	{
        private readonly TParent parent;
        private readonly Type entity;
        private readonly GeneratorMapping mapping = new GeneratorMapping();
        private readonly GeneratorBuilder builder;

        public IdentityGenerationStrategyBuilder(TParent parent, Type identityType, Type entity)
        {
            this.parent = parent;
            this.entity = entity;

            builder = new GeneratorBuilder(mapping, identityType);
        }

        internal bool IsDirty { get; private set; }

        public GeneratorMapping GetGeneratorMapping()
        {
            mapping.ContainingEntityType = entity;

            return mapping;
        }

		/// <summary>
		/// generates identifiers of any integral type that are unique only when no other 
		/// process is inserting data into the same table. Do not use in a cluster.
		/// </summary>
		/// <returns></returns>
        public TParent Increment()
		{
			builder.Increment();
		    IsDirty = true;
			return parent;
		}

        /// <summary>
        /// generates identifiers of any integral type that are unique only when no other 
        /// process is inserting data into the same table. Do not use in a cluster.
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public TParent Increment(Action<ParamBuilder> paramValues)
        {
            builder.Increment(paramValues);
            IsDirty = true;
            return parent;
        }

		/// <summary>
		/// supports identity columns in DB2, MySQL, MS SQL Server and Sybase.
		/// The identifier returned by the database is converted to the property type using 
		/// Convert.ChangeType. Any integral property type is thus supported.
		/// </summary>
		/// <returns></returns>
        public TParent Identity()
		{
			builder.Identity();
            IsDirty = true;
            return parent;
		}

        /// <summary>
        /// supports identity columns in DB2, MySQL, MS SQL Server and Sybase.
        /// The identifier returned by the database is converted to the property type using 
        /// Convert.ChangeType. Any integral property type is thus supported.
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public TParent Identity(Action<ParamBuilder> paramValues)
        {
            builder.Identity(paramValues);
            IsDirty = true;
            return parent;
        }

		/// <summary>
		/// uses a sequence in DB2, PostgreSQL, Oracle or a generator in Firebird.
		/// The identifier returned by the database is converted to the property type 
		/// using Convert.ChangeType. Any integral property type is thus supported.
		/// </summary>
		/// <param name="sequenceName"></param>
		/// <returns></returns>
        public TParent Sequence(string sequenceName)
		{
			builder.Sequence(sequenceName);
            IsDirty = true;
            return parent;
		}

        /// <summary>
        /// uses a sequence in DB2, PostgreSQL, Oracle or a generator in Firebird.
        /// The identifier returned by the database is converted to the property type 
        /// using Convert.ChangeType. Any integral property type is thus supported.
        /// </summary>
        /// <param name="sequenceName"></param>
        /// <param name="paramValues">Params configuration</param>
        public TParent Sequence(string sequenceName, Action<ParamBuilder> paramValues)
        {
            builder.Sequence(sequenceName, paramValues);
            IsDirty = true;
            return parent;
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
        public TParent HiLo(string table, string column, string maxLo)
		{
			builder.HiLo(table, column, maxLo);
            IsDirty = true;
            return parent;
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
        public TParent HiLo(string table, string column, string maxLo, Action<ParamBuilder> paramValues)
        {
            builder.HiLo(table, column, maxLo, paramValues);
            IsDirty = true;
            return parent;
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
        public TParent HiLo(string maxLo)
		{
			builder.HiLo(maxLo);
            IsDirty = true;
            return parent;
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
        public TParent HiLo(string maxLo, Action<ParamBuilder> paramValues)
        {
            builder.HiLo(maxLo, paramValues);
            IsDirty = true;
            return parent;
        }

		/// <summary>
		/// uses an Oracle-style sequence (where supported)
		/// </summary>
		/// <param name="sequence"></param>
		/// <param name="maxLo"></param>
		/// <returns></returns>
        public TParent SeqHiLo(string sequence, string maxLo)
		{
			builder.SeqHiLo(sequence, maxLo);
            IsDirty = true;
            return parent;
		}

        /// <summary>
        /// uses an Oracle-style sequence (where supported)
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="maxLo"></param>
        /// <param name="paramValues">Params configuration</param>
        public TParent SeqHiLo(string sequence, string maxLo, Action<ParamBuilder> paramValues)
        {
            builder.SeqHiLo(sequence, maxLo, paramValues);
            IsDirty = true;
            return parent;
        }

		/// <summary>
		/// uses System.Guid and its ToString(string format) method to generate identifiers
		/// of type string. The length of the string returned depends on the configured format. 
		/// </summary>
		/// <param name="format">http://msdn.microsoft.com/en-us/library/97af8hh4.aspx</param>
		/// <returns></returns>
        public TParent UuidHex(string format)
		{
			builder.UuidHex(format);
            IsDirty = true;
            return parent;
		}

        /// <summary>
        /// uses System.Guid and its ToString(string format) method to generate identifiers
        /// of type string. The length of the string returned depends on the configured format. 
        /// </summary>
        /// <param name="format">http://msdn.microsoft.com/en-us/library/97af8hh4.aspx</param>
        /// <param name="paramValues">Params configuration</param>
        public TParent UuidHex(string format, Action<ParamBuilder> paramValues)
        {
            builder.UuidHex(format, paramValues);
            IsDirty = true;
            return parent;
        }

		/// <summary>
		/// uses a new System.Guid to create a byte[] that is converted to a string.  
		/// </summary>
		/// <returns></returns>
        public TParent UuidString()
		{
			builder.UuidString();
            IsDirty = true;
            return parent;
		}

        /// <summary>
        /// uses a new System.Guid to create a byte[] that is converted to a string.  
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public TParent UuidString(Action<ParamBuilder> paramValues)
        {
            builder.UuidString(paramValues);
            IsDirty = true;
            return parent;
        }

		/// <summary>
		/// uses a new System.Guid as the identifier. 
		/// </summary>
		/// <returns></returns>
        public TParent Guid()
		{
			builder.Guid();
            IsDirty = true;
            return parent;
		}

        /// <summary>
        /// uses a new System.Guid as the identifier. 
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public TParent Guid(Action<ParamBuilder> paramValues)
        {
            builder.Guid(paramValues);
            IsDirty = true;
            return parent;
        }

		/// <summary>
		/// Recommended for Guid identifiers!
		/// uses the algorithm to generate a new System.Guid described by Jimmy Nilsson 
		/// in the article http://www.informit.com/articles/article.asp?p=25862. 
		/// </summary>
		/// <returns></returns>
        public TParent GuidComb()
		{
			builder.GuidComb();
            IsDirty = true;
            return parent;
		}

        /// <summary>
        /// Recommended for Guid identifiers!
        /// uses the algorithm to generate a new System.Guid described by Jimmy Nilsson 
        /// in the article http://www.informit.com/articles/article.asp?p=25862. 
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public TParent GuidComb(Action<ParamBuilder> paramValues)
        {
            builder.GuidComb(paramValues);
            IsDirty = true;
            return parent;
        }

		/// <summary>
		/// lets the application to assign an identifier to the object before Save() is called. 
		/// </summary>
		/// <returns></returns>
        public TParent Assigned()
		{
			builder.Assigned();
            IsDirty = true;
            return parent;
		}

        /// <summary>
        /// lets the application to assign an identifier to the object before Save() is called. 
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public TParent Assigned(Action<ParamBuilder> paramValues)
        {
            builder.Assigned(paramValues);
            IsDirty = true;
            return parent;
        }

		/// <summary>
		/// picks identity, sequence or hilo depending upon the capabilities of the underlying database. 
		/// </summary>
		/// <returns></returns>
        public TParent Native()
		{
			builder.Native();
            IsDirty = true;
            return parent;
		}

        /// <summary>
        /// picks identity, sequence or hilo depending upon the capabilities of the underlying database. 
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public TParent Native(Action<ParamBuilder> paramValues)
        {
            builder.Native(paramValues);
            IsDirty = true;
            return parent;
        }

		/// <summary>
		/// uses the identifier of another associated object. Usually used in conjunction with a one-to-one primary key association. 
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
        public TParent Foreign(string property)
		{
			builder.Foreign(property);
            IsDirty = true;
            return parent;
		}

        /// <summary>
        /// uses the identifier of another associated object. Usually used in conjunction with a one-to-one primary key association. 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="paramValues">Params configuration</param>
        public TParent Foreign(string property, Action<ParamBuilder> paramValues)
        {
            builder.Foreign(property, paramValues);
            IsDirty = true;
            return parent;
        }
	}
}
