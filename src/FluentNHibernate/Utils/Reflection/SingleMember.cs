using System;
using System.Linq.Expressions;

namespace FluentNHibernate.Utils;

public class SingleMember : Accessor
{
    private readonly Member member;

    public SingleMember(Member member)
    {
        this.member = member;
    }

    #region Accessor Members

    public string FieldName
    {
        get { return member.Name; }
    }

    public Type PropertyType
    {
        get { return member.PropertyType; }
    }

    public Member InnerMember
    {
        get { return member; }
    }

    public Accessor GetChildAccessor<T>(Expression<Func<T, object>> expression)
    {
        var property = expression.ToMember();
        return new PropertyChain(new[] {member, property});
    }

    public string Name
    {
        get { return member.Name; }
    }

    public void SetValue(object target, object propertyValue)
    {
        member.SetValue(target, propertyValue);
    }

    public object GetValue(object target)
    {
        return member.GetValue(target);
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
