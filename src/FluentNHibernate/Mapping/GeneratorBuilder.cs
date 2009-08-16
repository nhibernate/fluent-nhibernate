using System;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Mapping
{
    public class GeneratorBuilder
    {
        private readonly Type identityType;
        private readonly GeneratorMapping mapping;

        public GeneratorBuilder(GeneratorMapping mapping, Type identityType)
        {
            this.mapping = mapping;
            this.identityType = identityType;
        }

        private void SetGenerator(string generator)
        {
            mapping.Class = generator;
        }

        private void AddGeneratorParam(string name, string value)
        {
            mapping.Params.Add(name, value);
        }

        private void EnsureIntegralIdenityType()
        {
            if (!IsIntegralType(identityType)) throw new InvalidOperationException("Identity type must be integral (int, long, uint, ulong)");
        }

        private void EnsureGuidIdentityType()
        {
            if (identityType != typeof(Guid) && identityType != typeof(Guid?)) throw new InvalidOperationException("Identity type must be Guid");
        }

        private void EnsureStringIdentityType()
        {
            if (identityType != typeof(string)) throw new InvalidOperationException("Identity type must be string");
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

        public void Increment()
        {
            EnsureIntegralIdenityType();
            SetGenerator("increment");
        }

        public void Increment(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            Increment();
        }

        /// <summary>
        /// supports identity columns in DB2, MySQL, MS SQL Server and Sybase.
        /// The identifier returned by the database is converted to the property type using 
        /// Convert.ChangeType. Any integral property type is thus supported.
        /// </summary>
        /// <returns></returns>
        public void Identity()
        {
            EnsureIntegralIdenityType();
            SetGenerator("identity");
        }

        /// <summary>
        /// supports identity columns in DB2, MySQL, MS SQL Server and Sybase.
        /// The identifier returned by the database is converted to the property type using 
        /// Convert.ChangeType. Any integral property type is thus supported.
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void Identity(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            Identity();
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
            EnsureIntegralIdenityType();
            SetGenerator("sequence");
            AddGeneratorParam("sequence", sequenceName);
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
            paramValues(new ParamBuilder(mapping.Params));
            Sequence(sequenceName);
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
            AddGeneratorParam("table", table);
            AddGeneratorParam("column", column);
            HiLo(maxLo);
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
            paramValues(new ParamBuilder(mapping.Params));
            HiLo(table, column, maxLo);
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
            EnsureIntegralIdenityType();
            SetGenerator("hilo");
            AddGeneratorParam("max_lo", maxLo);
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
            paramValues(new ParamBuilder(mapping.Params));
            HiLo(maxLo);
        }

        /// <summary>
        /// uses an Oracle-style sequence (where supported)
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="maxLo"></param>
        /// <returns></returns>
        public void SeqHiLo(string sequence, string maxLo)
        {
            EnsureIntegralIdenityType();
            SetGenerator("seqhilo");
            AddGeneratorParam("sequence", sequence);
            AddGeneratorParam("max_lo", maxLo);
        }

        /// <summary>
        /// uses an Oracle-style sequence (where supported)
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="maxLo"></param>
        /// <param name="paramValues">Params configuration</param>
        public void SeqHiLo(string sequence, string maxLo, Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            SeqHiLo(sequence, maxLo);
        }

        /// <summary>
        /// uses System.Guid and its ToString(string format) method to generate identifiers
        /// of type string. The length of the string returned depends on the configured format. 
        /// </summary>
        /// <param name="format">http://msdn.microsoft.com/en-us/library/97af8hh4.aspx</param>
        /// <returns></returns>
        public void UuidHex(string format)
        {
            EnsureStringIdentityType();
            SetGenerator("uuid.hex");
            AddGeneratorParam("format", format);
        }

        /// <summary>
        /// uses System.Guid and its ToString(string format) method to generate identifiers
        /// of type string. The length of the string returned depends on the configured format. 
        /// </summary>
        /// <param name="format">http://msdn.microsoft.com/en-us/library/97af8hh4.aspx</param>
        /// <param name="paramValues">Params configuration</param>
        public void UuidHex(string format, Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            UuidHex(format);
        }

        /// <summary>
        /// uses a new System.Guid to create a byte[] that is converted to a string.  
        /// </summary>
        /// <returns></returns>
        public void UuidString()
        {
            EnsureStringIdentityType();
            SetGenerator("uuid.string");
        }

        /// <summary>
        /// uses a new System.Guid to create a byte[] that is converted to a string.  
        /// </summary>
        /// <param name="paramValues">Params configuration</param>
        public void UuidString(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            UuidString();
        }

        /// <summary>
        /// uses a new System.Guid as the identifier. 
        /// </summary>
        /// <returns></returns>
        public void Guid()
        {
            EnsureGuidIdentityType();
            SetGenerator("guid");
        }

        public void Guid(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            Guid();
        }

        public void GuidComb()
        {
            EnsureGuidIdentityType();
            SetGenerator("guid.comb");
        }

        public void GuidComb(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            GuidComb();
        }

        public void Assigned()
        {
            SetGenerator("assigned");
        }

        public void Assigned(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            Assigned();
        }

        public void Native()
        {
            EnsureIntegralIdenityType();
            SetGenerator("native");
        }

        public void Native(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            Native();
        }

        public void Foreign(string property)
        {
            SetGenerator("foreign");
            AddGeneratorParam("property", property);
        }

        public void Foreign(string property, Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            Foreign(property);
        }
    }
}