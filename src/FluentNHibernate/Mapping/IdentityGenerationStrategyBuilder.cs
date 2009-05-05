using System;

namespace FluentNHibernate.Mapping
{
    public class IdentityGenerationStrategyBuilder
	{
        private readonly IIdentityPart parent;

        public IdentityGenerationStrategyBuilder(IIdentityPart parent)
		{
			this.parent = parent;
		}

		private void SetGenerator(string generator)
		{
			parent.SetGeneratorClass(generator);
		}

		private void AddGeneratorParam(string name, string innerXml)
		{
			parent.AddGeneratorParam(name, innerXml);
		}

		private void EnsureIntegralIdenityType()
		{
			if (!IsIntegralType(parent.IdentityType)) throw new InvalidOperationException("Identity type must be integral (int, long, uint, ulong)");
		}

		private void EnsureGuidIdentityType()
		{
			if (parent.IdentityType != typeof(Guid) && parent.IdentityType != typeof(Guid?)) throw new InvalidOperationException("Identity type must be Guid");
		}

		private void EnsureStringIdentityType()
		{
			if (parent.IdentityType != typeof(string)) throw new InvalidOperationException("Identity type must be string");
		}

		private static bool IsIntegralType(Type t)
		{
			// do we think we'll encounter more?
            return t == typeof(int) || t == typeof(int?) 
                || t == typeof(long) || t == typeof(long?)
                || t == typeof(uint) || t == typeof(uint?)
                || t == typeof(ulong) || t == typeof(ulong?)
                || t == typeof(byte) || t == typeof(byte?)
                || t == typeof(sbyte) || t == typeof(sbyte?)
                || t == typeof(short) || t == typeof(short?)
                || t == typeof(ushort) || t == typeof(ushort?);
		}

		/// <summary>
		/// generates identifiers of any integral type that are unique only when no other 
		/// process is inserting data into the same table. Do not use in a cluster.
		/// </summary>
		/// <returns></returns>
        public IIdentityPart Increment()
		{
			EnsureIntegralIdenityType();
			SetGenerator("increment");
			return parent;
		}

		/// <summary>
		/// supports identity columns in DB2, MySQL, MS SQL Server and Sybase.
		/// The identifier returned by the database is converted to the property type using 
		/// Convert.ChangeType. Any integral property type is thus supported.
		/// </summary>
		/// <returns></returns>
        public IIdentityPart Identity()
		{
			EnsureIntegralIdenityType();
			SetGenerator("identity");
			return parent;
		}

		/// <summary>
		/// uses a sequence in DB2, PostgreSQL, Oracle or a generator in Firebird.
		/// The identifier returned by the database is converted to the property type 
		/// using Convert.ChangeType. Any integral property type is thus supported.
		/// </summary>
		/// <param name="sequenceName"></param>
		/// <returns></returns>
        public IIdentityPart Sequence(string sequenceName)
		{
			EnsureIntegralIdenityType();
			SetGenerator("sequence");
			AddGeneratorParam("sequence", sequenceName);
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
        public IIdentityPart HiLo(string table, string column, string maxLo)
		{
			AddGeneratorParam("table", table);
			AddGeneratorParam("column", column);
			return HiLo(maxLo);
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
        public IIdentityPart HiLo(string maxLo)
		{
			EnsureIntegralIdenityType();
			SetGenerator("hilo");
			AddGeneratorParam("max_lo", maxLo);
			return parent;
		}

		/// <summary>
		/// uses an Oracle-style sequence (where supported)
		/// </summary>
		/// <param name="sequence"></param>
		/// <param name="maxLo"></param>
		/// <returns></returns>
        public IIdentityPart SeqHiLo(string sequence, string maxLo)
		{
			EnsureIntegralIdenityType();
			SetGenerator("seqhilo");
			AddGeneratorParam("sequence", sequence);
			AddGeneratorParam("max_lo", maxLo);
			return parent;
		}

		/// <summary>
		/// uses System.Guid and its ToString(string format) method to generate identifiers
		/// of type string. The length of the string returned depends on the configured format. 
		/// </summary>
		/// <param name="format">http://msdn.microsoft.com/en-us/library/97af8hh4.aspx</param>
		/// <returns></returns>
        public IIdentityPart UuidHex(string format)
		{
			EnsureStringIdentityType();
			SetGenerator("uuid.hex");
			AddGeneratorParam("format", format);
			return parent;
		}

		/// <summary>
		/// uses a new System.Guid to create a byte[] that is converted to a string.  
		/// </summary>
		/// <returns></returns>
        public IIdentityPart UuidString()
		{
			EnsureStringIdentityType();
			SetGenerator("uuid.string");
			return parent;
		}

		/// <summary>
		/// uses a new System.Guid as the identifier. 
		/// </summary>
		/// <returns></returns>
        public IIdentityPart Guid()
		{
			EnsureGuidIdentityType();
			SetGenerator("guid");
			return parent;
		}

		/// <summary>
		/// Recommended for Guid identifiers!
		/// uses the algorithm to generate a new System.Guid described by Jimmy Nilsson 
		/// in the article http://www.informit.com/articles/article.asp?p=25862. 
		/// </summary>
		/// <returns></returns>
        public IIdentityPart GuidComb()
		{
			EnsureGuidIdentityType();
			SetGenerator("guid.comb");
			return parent;
		}

		/// <summary>
		/// lets the application to assign an identifier to the object before Save() is called. 
		/// </summary>
		/// <returns></returns>
        public IIdentityPart Assigned()
		{
			SetGenerator("assigned");
			return parent;
		}

		/// <summary>
		/// picks identity, sequence or hilo depending upon the capabilities of the underlying database. 
		/// </summary>
		/// <returns></returns>
        public IIdentityPart Native()
		{
			EnsureIntegralIdenityType();
			SetGenerator("native");
			return parent;
		}

		/// <summary>
		/// uses the identifier of another associated object. Usually used in conjunction with a one-to-one primary key association. 
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
        public IIdentityPart Foreign(string property)
		{
			SetGenerator("foreign");
			AddGeneratorParam("property", property);
			return parent;
		}
	}
}
