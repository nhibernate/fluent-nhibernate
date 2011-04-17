using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Id;

namespace FluentNHibernate.Conventions.Instances
{
    public class GeneratorInstance : GeneratorInspector, IGeneratorInstance
    {
        private readonly GeneratorBuilder builder;

        public GeneratorInstance(GeneratorMapping mapping, Type type)
            : base(mapping)
        {
            builder = new GeneratorBuilder(mapping, type, Layer.Conventions);
        }

		/// <summary>
		/// generates identifiers of any integral type that are unique only when no other 
		/// process is inserting data into the same table. Do not use in a cluster.
		/// </summary>
		/// <returns></returns>
        public void Increment()
		{
			builder.Increment();
		}

        /// <summary>
        /// generates identifiers of any integral type that are unique only when no other 
        /// process is inserting data into the same table. Do not use in a cluster.
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void Increment(Action<ParamBuilder> paramValues)
        {
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
            builder.Sequence(sequenceName, paramValues);
        }

        /// <summary>
        /// uses a hi/lo algorithm to efficiently generate identifiers of any integral type,
        /// given a table and column (by default hibernate_unique_key and next_hi respectively)
        /// as a source of hi values. The hi/lo algorithm generates identifiers that are unique
        /// only for a particular database. Do not use this generator with a user-supplied connection.
        /// requires a "special" database table to hold the next available "hi" value
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="column">The column.</param>
        /// <param name="maxLo">The max lo.</param>
        /// <param name="where">The where.</param>
        public void HiLo(string table, string column, string maxLo, string where)
        {
            builder.HiLo(table, column, maxLo, where);
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
            builder.UuidHex(format, paramValues);
        }

		/// <summary>
		/// uses a new System.Guid to create a byte[] that is converted to a string.  
		/// </summary>
		/// <returns></returns>
        public void UuidString()
		{
            builder.UuidString();
		}

        /// <summary>
        /// uses a new System.Guid to create a byte[] that is converted to a string.  
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void UuidString(Action<ParamBuilder> paramValues)
        {
            builder.UuidString(paramValues);
        }

		/// <summary>
		/// uses a new System.Guid as the identifier. 
		/// </summary>
		/// <returns></returns>
        public void Guid()
		{
            builder.Guid();
		}

        /// <summary>
        /// uses a new System.Guid as the identifier. 
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void Guid(Action<ParamBuilder> paramValues)
        {
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
            builder.GuidComb(paramValues);
        }

        /// <summary>
        /// Generator that uses the RDBMS native function to generate a GUID.
        /// The behavior is similar to the “sequence” generator. When a new
        /// object is saved NH run two queries: the first to retrieve the GUID
        /// value and the second to insert the entity using the Guid retrieved
        /// from the RDBMS. Your entity Id must be System.Guid and the SQLType
        /// will depend on the dialect (RAW(16) in Oracle, UniqueIdentifier in
        /// MsSQL for example).
        /// </summary>
        public void GuidNative()
        {
            builder.GuidNative();
        }

        /// <summary>
        /// Generator that uses the RDBMS native function to generate a GUID.
        /// The behavior is similar to the “sequence” generator. When a new
        /// object is saved NH run two queries: the first to retrieve the GUID
        /// value and the second to insert the entity using the Guid retrieved
        /// from the RDBMS. Your entity Id must be System.Guid and the SQLType
        /// will depend on the dialect (RAW(16) in Oracle, UniqueIdentifier in
        /// MsSQL for example).
        /// </summary>
        /// <example>
        ///     GuidNative(x =>
        ///     {
        ///       x.AddParam("key", "value");
        ///     });
        /// </example>
        /// <param name="paramValues">Parameter builder closure</param>
        public void GuidNative(Action<ParamBuilder> paramValues)
        {
            builder.GuidNative(paramValues);
        }

        /// <summary>
        /// A deviation of the trigger-identity. This generator works
        /// together with the <see cref="ClassMap{T}.NaturalId"/> feature.
        /// The difference with trigger-identity is that the POID value
        /// is retrieved by a SELECT using the natural-id fields as filter.
        /// </summary>
        public void Select()
        {
            builder.Select();
        }

        /// <summary>
        /// A deviation of the trigger-identity. This generator works
        /// together with the <see cref="ClassMap{T}.NaturalId"/> feature.
        /// The difference with trigger-identity is that the POID value
        /// is retrieved by a SELECT using the natural-id fields as filter.
        /// </summary>
        /// <example>
        ///     Select(x =>
        ///     {
        ///       x.AddParam("key", "value");
        ///     });
        /// </example>
        /// <param name="paramValues">Parameter builder closure</param>
        public void Select(Action<ParamBuilder> paramValues)
        {
            builder.Select(paramValues);
        }

        /// <summary>
        /// Based on sequence but works like an identity. The POID
        /// value is retrieved with an INSERT query. Your entity Id must
        /// be an integral type.
        /// "hibernate_sequence" is the default name for the sequence, unless
        /// another is provided.
        /// </summary>
        public void SequenceIdentity()
        {
            builder.SequenceIdentity();
        }

        /// <summary>
        /// Based on sequence but works like an identity. The POID
        /// value is retrieved with an INSERT query. Your entity Id must
        /// be an integral type.
        /// "hibernate_sequence" is the default name for the sequence, unless
        /// another is provided.
        /// </summary>
        /// <param name="sequence">Custom sequence name</param>
        public void SequenceIdentity(string sequence)
        {
            builder.SequenceIdentity(sequence);
        }

        /// <summary>
        /// Based on sequence but works like an identity. The POID
        /// value is retrieved with an INSERT query. Your entity Id must
        /// be an integral type.
        /// "hibernate_sequence" is the default name for the sequence, unless
        /// another is provided.
        /// </summary>
        /// <param name="paramValues">Parameter builder closure</param>
        public void SequenceIdentity(Action<ParamBuilder> paramValues)
        {
            builder.SequenceIdentity(paramValues);
        }

        /// <summary>
        /// Based on sequence but works like an identity. The POID
        /// value is retrieved with an INSERT query. Your entity Id must
        /// be an integral type.
        /// "hibernate_sequence" is the default name for the sequence, unless
        /// another is provided.
        /// </summary>
        /// <param name="sequence">Custom sequence name</param>
        /// <param name="paramValues">Parameter builder closure</param>
        public void SequenceIdentity(string sequence, Action<ParamBuilder> paramValues)
        {
            builder.SequenceIdentity(sequence, paramValues);
        }


        /// <summary>
        /// trigger-identity is a NHibernate specific feature where the POID
        /// is generated by the RDBMS with an INSERT query through a
        /// BEFORE INSERT trigger. In this case you can use any supported type,
        /// including a custom type, with the limitation of a single column usage.
        /// </summary>
        public void TriggerIdentity()
        {
            builder.TriggerIdentity();
        }

        /// <summary>
        /// trigger-identity is a NHibernate specific feature where the POID
        /// is generated by the RDBMS with an INSERT query through a
        /// BEFORE INSERT trigger. In this case you can use any supported type,
        /// including a custom type, with the limitation of a single column usage.
        /// </summary>
        /// <param name="paramValues">Parameter builder closure</param>
        public void TriggerIdentity(Action<ParamBuilder> paramValues)
        {
            builder.TriggerIdentity(paramValues);
        }

		/// <summary>
		/// lets the application to assign an identifier to the object before Save() is called. 
		/// </summary>
		/// <returns></returns>
        public void Assigned()
		{
            builder.Assigned();
		}

        /// <summary>
        /// lets the application to assign an identifier to the object before Save() is called. 
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void Assigned(Action<ParamBuilder> paramValues)
        {
            builder.Assigned(paramValues);
        }

		/// <summary>
		/// picks identity, sequence or hilo depending upon the capabilities of the underlying database. 
		/// </summary>
		/// <returns></returns>
        public void Native()
		{
            builder.Native();
		}

        /// <summary>
        /// picks identity, sequence or hilo depending upon the capabilities of the underlying database. 
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void Native(Action<ParamBuilder> paramValues)
        {
            builder.Native(paramValues);
        }

        /// <summary>
        /// picks identity, sequence or hilo depending upon the capabilities of the underlying database. 
        /// </summary>
        public void Native(string sequenceName)
        {
            builder.Native(sequenceName);
        }

        /// <summary>
        /// picks identity, sequence or hilo depending upon the capabilities of the underlying database. 
        /// </summary>
        public void Native(string sequenceName, Action<ParamBuilder> paramValues)
        {
            builder.Native(sequenceName, paramValues);
        }

		/// <summary>
		/// uses the identifier of another associated object. Usually used in conjunction with a one-to-one primary key association. 
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
        public void Foreign(string property)
		{
            builder.Foreign(property);
		}

        /// <summary>
        /// uses the identifier of another associated object. Usually used in conjunction with a one-to-one primary key association. 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="paramValues">Params configuration</param>
        public void Foreign(string property, Action<ParamBuilder> paramValues)
        {
            builder.Foreign(property, paramValues);
        }

        public void Custom<T>() where T : IIdentifierGenerator
        {
            Custom(typeof(T));
        }

        public void Custom(Type generator)
        {
            Custom(generator.AssemblyQualifiedName);
        }

        public void Custom(string generator)
        {
            builder.Custom(generator);
        }

        public void Custom<T>(Action<ParamBuilder> paramValues) where T : IIdentifierGenerator
        {
            Custom(typeof(T), paramValues);
        }

        public void Custom(Type generator, Action<ParamBuilder> paramValues)
        {
            Custom(generator.AssemblyQualifiedName, paramValues);
        }

        public void Custom(string generator, Action<ParamBuilder> paramValues)
        {
            builder.Custom(generator, paramValues);
        }
    }
}