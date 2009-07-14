using System;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IGeneratorInstance : IGeneratorInspector
    {
        void Increment();
        void Increment(Action<ParamBuilder> paramValue);
        void Identity();
        void Identity(Action<ParamBuilder> paramValue);
        void Sequence(string sequenceName);
        void Sequence(string sequenceName, Action<ParamBuilder> paramValues);
        void HiLo(string talbe, string column, string maxLo);
        void HiLo(string talbe, string column, string maxLo, Action<ParamBuilder> paramValues);
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
        void Foreign(string property);
        void Foreign(string property, Action<ParamBuilder> paramValues);
    }
}