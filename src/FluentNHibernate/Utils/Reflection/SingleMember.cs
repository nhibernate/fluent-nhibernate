using System;
using System.Linq.Expressions;

namespace FluentNHibernate.Utils;

public class SingleMember : Accessor
{
    public SingleMember(Member member)
    {
        this.InnerMember = member;
    }

    #region Accessor Members

    public string FieldName => InnerMember.Name;

    public Type PropertyType => InnerMember.PropertyType;

    public Member InnerMember { get; }

    public Accessor GetChildAccessor<T>(Expression<Func<T, object>> expression)
    {
        var property = expression.ToMember();
        return new PropertyChain(new[] {InnerMember, property});
    }

    public string Name => InnerMember.Name;

    public void SetValue(object target, object propertyValue)
    {
        InnerMember.SetValue(target, propertyValue);
    }

    public object GetValue(object target)
    {
        return InnerMember.GetValue(target);
    }

    #endregion

    public static SingleMember Build<T>(Expression<Func<T, object>> expression)
    {
        var m = expression.ToMember();
        return new SingleMember(m);
    }

    public static SingleMember Build<T>(string propertyName)
    {
        var m = typeof(T).GetProperty(propertyName).ToMember();
        return new SingleMember(m);
    }
}
