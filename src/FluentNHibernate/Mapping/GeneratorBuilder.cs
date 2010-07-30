using System;
using FluentNHibernate.MappingModel.Identity;
using NHibernate.Id;

namespace FluentNHibernate.Mapping
{
    internal class GeneratorBuilder
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

        public void Identity()
        {
            EnsureIntegralIdenityType();
            SetGenerator("identity");
        }

        public void Identity(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            Identity();
        }

        public void Sequence(string sequenceName)
        {
            EnsureIntegralIdenityType();
            SetGenerator("sequence");
            AddGeneratorParam("sequence", sequenceName);
        }

        public void Sequence(string sequenceName, Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            Sequence(sequenceName);
        }

        public void HiLo(string table, string column, string maxLo, string where)
        {
            AddGeneratorParam("table", table);
            AddGeneratorParam("column", column);
            AddGeneratorParam("where", where);
            HiLo(maxLo);
        }

        public void HiLo(string table, string column, string maxLo)
        {
            AddGeneratorParam("table", table);
            AddGeneratorParam("column", column);
            HiLo(maxLo);
        }

        public void HiLo(string table, string column, string maxLo, Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            HiLo(table, column, maxLo);
        }

        public void HiLo(string maxLo)
        {
            EnsureIntegralIdenityType();
            SetGenerator("hilo");
            AddGeneratorParam("max_lo", maxLo);
        }

        public void HiLo(string maxLo, Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            HiLo(maxLo);
        }

        public void SeqHiLo(string sequence, string maxLo)
        {
            EnsureIntegralIdenityType();
            SetGenerator("seqhilo");
            AddGeneratorParam("sequence", sequence);
            AddGeneratorParam("max_lo", maxLo);
        }

        public void SeqHiLo(string sequence, string maxLo, Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            SeqHiLo(sequence, maxLo);
        }

        public void UuidHex(string format)
        {
            EnsureStringIdentityType();
            SetGenerator("uuid.hex");
            AddGeneratorParam("format", format);
        }

        public void UuidHex(string format, Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            UuidHex(format);
        }

        public void UuidString()
        {
            EnsureStringIdentityType();
            SetGenerator("uuid.string");
        }

        public void UuidString(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            UuidString();
        }

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

        public void GuidNative()
        {
            EnsureGuidIdentityType();
            SetGenerator("guid.native");
        }

        public void GuidNative(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
        }

        public void Select()
        {
            SetGenerator("select");
        }

        public void Select(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            Select();
        }

        public void SequenceIdentity()
        {
            EnsureIntegralIdenityType();
            SetGenerator("sequence-identity");
        }

        public void SequenceIdentity(string sequence)
        {
            AddGeneratorParam("sequence", sequence);
            SequenceIdentity();
        }

        public void SequenceIdentity(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            SequenceIdentity();
        }

        public void SequenceIdentity(string sequence, Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            SequenceIdentity(sequence);
        }

        public void TriggerIdentity()
        {
            SetGenerator("trigger-identity");
        }

        public void TriggerIdentity(Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            TriggerIdentity();
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

        public void Native(string sequenceName)
        {
            EnsureIntegralIdenityType();
            SetGenerator("native");
            AddGeneratorParam("sequence", sequenceName);
        }

        public void Native(string sequenceName, Action<ParamBuilder> paramValues)
        {
            paramValues(new ParamBuilder(mapping.Params));
            Native(sequenceName);
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
            SetGenerator(generator);
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
            paramValues(new ParamBuilder(mapping.Params));
            SetGenerator(generator);
        }
    }
}