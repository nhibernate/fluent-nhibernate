using System;

namespace FluentNHibernate.Mapping
{
	public class IdentityGenerationStrategyBuilder
	{
		private readonly IdentityPart _parent;

		public IdentityGenerationStrategyBuilder(IdentityPart parent)
		{
			_parent = parent;
		}

		private void setGenerator(string generator)
		{
			_parent.SetGeneratorClass(generator);
		}

		private void addGeneratorParam(string name, string innerXml)
		{
			_parent.AddGeneratorParam(name, innerXml);
		}

		private void ensureIntegralIdenityType()
		{
			if (!isIntegralType(_parent.IdentityType)) throw new InvalidOperationException("Identity type must be integral (int, long, uint, ulong)");
		}

		private void ensureGuidIdentityType()
		{
			if (_parent.IdentityType != typeof(Guid) && _parent.IdentityType != typeof(Guid?)) throw new InvalidOperationException("Identity type must be Guid");
		}

		private void ensureStringIdentityType()
		{
			if (_parent.IdentityType != typeof(string)) throw new InvalidOperationException("Identity type must be string");
		}

		private static bool isIntegralType(Type t)
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
		public IdentityPart Increment()
		{
			ensureIntegralIdenityType();
			setGenerator("increment");
			return _parent;
		}

		/// <summary>
		/// supports identity columns in DB2, MySQL, MS SQL Server and Sybase.
		/// The identifier returned by the database is converted to the property type using 
		/// Convert.ChangeType. Any integral property type is thus supported.
		/// </summary>
		/// <returns></returns>
		public IdentityPart Identity()
		{
			ensureIntegralIdenityType();
			setGenerator("identity");
			return _parent;
		}

		/// <summary>
		/// uses a sequence in DB2, PostgreSQL, Oracle or a generator in Firebird.
		/// The identifier returned by the database is converted to the property type 
		/// using Convert.ChangeType. Any integral property type is thus supported.
		/// </summary>
		/// <param name="sequenceName"></param>
		/// <returns></returns>
		public IdentityPart Sequence(string sequenceName)
		{
			ensureIntegralIdenityType();
			setGenerator("sequence");
			addGeneratorParam("sequence", sequenceName);
			return _parent;
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
		/// <param name="max_lo"></param>
		/// <returns></returns>
		public IdentityPart HiLo(string table, string column, string max_lo)
		{
			addGeneratorParam("table", table);
			addGeneratorParam("column", column);
			return HiLo(max_lo);
		}

		/// <summary>
		/// uses a hi/lo algorithm to efficiently generate identifiers of any integral type, 
		/// given a table and column (by default hibernate_unique_key and next_hi respectively) 
		/// as a source of hi values. The hi/lo algorithm generates identifiers that are unique 
		/// only for a particular database. Do not use this generator with a user-supplied connection.
		/// requires a "special" database table to hold the next available "hi" value
		/// </summary>
		/// <param name="max_lo"></param>
		/// <returns></returns>
		public IdentityPart HiLo(string max_lo)
		{
			ensureIntegralIdenityType();
			setGenerator("hilo");
			addGeneratorParam("max_lo", max_lo);
			return _parent;
		}

		/// <summary>
		/// uses an Oracle-style sequence (where supported)
		/// </summary>
		/// <param name="sequence"></param>
		/// <param name="max_lo"></param>
		/// <returns></returns>
		public IdentityPart SeqHiLo(string sequence, string max_lo)
		{
			ensureIntegralIdenityType();
			setGenerator("seqhilo");
			addGeneratorParam("sequence", sequence);
			addGeneratorParam("max_lo", max_lo);
			return _parent;
		}

		/// <summary>
		/// uses System.Guid and its ToString(string format) method to generate identifiers
		/// of type string. The length of the string returned depends on the configured format. 
		/// </summary>
		/// <param name="format">http://msdn.microsoft.com/en-us/library/97af8hh4.aspx</param>
		/// <returns></returns>
		public IdentityPart UuidHex(string format)
		{
			ensureStringIdentityType();
			setGenerator("uuid.hex");
			addGeneratorParam("format", format);
			return _parent;
		}

		/// <summary>
		/// uses a new System.Guid to create a byte[] that is converted to a string.  
		/// </summary>
		/// <returns></returns>
		public IdentityPart UuidString()
		{
			ensureStringIdentityType();
			setGenerator("uuid.string");
			return _parent;
		}

		/// <summary>
		/// uses a new System.Guid as the identifier. 
		/// </summary>
		/// <returns></returns>
		public IdentityPart Guid()
		{
			ensureGuidIdentityType();
			setGenerator("guid");
			return _parent;
		}

		/// <summary>
		/// Recommended for Guid identifiers!
		/// uses the algorithm to generate a new System.Guid described by Jimmy Nilsson 
		/// in the article http://www.informit.com/articles/article.asp?p=25862. 
		/// </summary>
		/// <returns></returns>
		public IdentityPart GuidComb()
		{
			ensureGuidIdentityType();
			setGenerator("guid.comb");
			return _parent;
		}

		/// <summary>
		/// lets the application to assign an identifier to the object before Save() is called. 
		/// </summary>
		/// <returns></returns>
		public IdentityPart Assigned()
		{
			setGenerator("assigned");
			return _parent;
		}

		/// <summary>
		/// picks identity, sequence or hilo depending upon the capabilities of the underlying database. 
		/// </summary>
		/// <returns></returns>
		public IdentityPart Native()
		{
			ensureIntegralIdenityType();
			setGenerator("native");
			return _parent;
		}

		/// <summary>
		/// uses the identifier of another associated object. Usually used in conjunction with a one-to-one primary key association. 
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public IdentityPart Foreign(string property)
		{
			setGenerator("foreign");
			addGeneratorParam("property", property);
			return _parent;
		}
	}
}
