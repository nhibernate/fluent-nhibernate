using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using NHibernate.Id;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IGeneratorInstance : IGeneratorInspector
    {
        void Increment();
        void Increment(Action<ParamBuilder> paramValues);
        void Identity();
        void Identity(Action<ParamBuilder> paramValues);
        void Sequence(string sequenceName);
        void Sequence(string sequenceName, Action<ParamBuilder> paramValues);
        void HiLo(string table, string column, string maxLo, string where);
        void HiLo(string table, string column, string maxLo);
        void HiLo(string table, string column, string maxLo, Action<ParamBuilder> paramValues);
        void HiLo(string maxLo);
        void HiLo(string maxLo, Action<ParamBuilder> paramValues);
        void SeqHiLo(string sequence, string maxLo);
        void SeqHiLo(string sequence, string maxLo, Action<ParamBuilder> paramValues);
        void UuidHex(string format);
        void UuidHex(string format, Action<ParamBuilder> paramValues);
        void UuidString();
        void UuidString(Action<ParamBuilder> paramValues);
        void Guid();
        void Guid(Action<ParamBuilder> paramValues);
        void GuidComb();
        void GuidComb(Action<ParamBuilder> paramValues);
        void Assigned();
        void Assigned(Action<ParamBuilder> paramValues);
        void Native();
        void Native(Action<ParamBuilder> paramValues);
        void Native(string sequenceName);
        void Native(string sequenceName, Action<ParamBuilder> paramValues);
        void Foreign(string property);
        void Foreign(string property, Action<ParamBuilder> paramValues);
        void Custom<T>() where T : IIdentifierGenerator;
        void Custom(Type generator);
        void Custom(string generator);
        void Custom<T>(Action<ParamBuilder> paramValues) where T : IIdentifierGenerator;
        void Custom(Type generator, Action<ParamBuilder> paramValues);
        void Custom(string generator, Action<ParamBuilder> paramValues);
        void GuidNative();
        void GuidNative(Action<ParamBuilder> paramValues);
        void Select();
        void Select(Action<ParamBuilder> paramValues);
        void SequenceIdentity();
        void SequenceIdentity(string sequence);
        void SequenceIdentity(Action<ParamBuilder> paramValues);
        void SequenceIdentity(string sequence, Action<ParamBuilder> paramValues);
        void TriggerIdentity();
        void TriggerIdentity(Action<ParamBuilder> paramValues);
    }
}