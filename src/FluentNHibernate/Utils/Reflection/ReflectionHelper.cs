﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Automapping;

namespace FluentNHibernate.Utils.Reflection;

public static class ReflectionHelper
{
    public static Type AutomappingTypeForEntityType(Type entityType)
    {
        return typeof(AutoMapping<>).MakeGenericType(entityType);
    }

    public static Member GetMember<TModel, TReturn>(Expression<Func<TModel, TReturn>> expression)
    {
        return GetMember(expression.Body);
    }

    public static Member GetMember<TModel>(Expression<Func<TModel, object>> expression)
    {
        return GetMember(expression.Body);
    }

    public static Accessor GetAccessor<MODEL>(Expression<Func<MODEL, object>> expression)
    {
        MemberExpression memberExpression = GetMemberExpression(expression.Body);

        return getAccessor(memberExpression);
    }

    public static Accessor GetAccessor<MODEL, T>(Expression<Func<MODEL, T>> expression)
    {
        MemberExpression memberExpression = GetMemberExpression(expression.Body);

        return getAccessor(memberExpression);
    }

    static bool IsIndexedPropertyAccess(Expression expression)
    {
        return IsMethodExpression(expression) && expression.ToString().Contains("get_Item");
    }

    static bool IsMethodExpression(Expression expression)
    {
        return expression is MethodCallExpression || (expression is UnaryExpression && IsMethodExpression((expression as UnaryExpression).Operand));
    }

    static Member GetMember(Expression expression)
    {
        if (IsIndexedPropertyAccess(expression))
            return GetDynamicComponentProperty(expression).ToMember();
        if (IsMethodExpression(expression))
            return ((MethodCallExpression)expression).Method.ToMember();

        var memberExpression = GetMemberExpression(expression);

        return memberExpression.Member.ToMember();
    }

    static PropertyInfo GetDynamicComponentProperty(Expression expression)
    {
        Type desiredConversionType = null;
        MethodCallExpression methodCallExpression = null;
        var nextOperand = expression;

        while (nextOperand is not null)
        {
            if (nextOperand.NodeType == ExpressionType.Call)
            {
                methodCallExpression = nextOperand as MethodCallExpression;
                desiredConversionType = desiredConversionType ?? methodCallExpression.Method.ReturnType;
                break;
            }

            if (nextOperand.NodeType != ExpressionType.Convert)
                throw new ArgumentException("Expression not supported", nameof(expression));
	            
            var unaryExpression = (UnaryExpression)nextOperand;
            desiredConversionType = unaryExpression.Type;
            nextOperand = unaryExpression.Operand;
        }
                
        var constExpression = methodCallExpression.Arguments[0] as ConstantExpression;
                
        return new DummyPropertyInfo((string)constExpression.Value, desiredConversionType);
    }

    static MemberExpression GetMemberExpression(Expression expression)
    {
        return GetMemberExpression(expression, true);
    }

    static MemberExpression GetMemberExpression(Expression expression, bool enforceCheck)
    {
        MemberExpression memberExpression = null;
        if (expression.NodeType == ExpressionType.Convert)
        {
            var body = (UnaryExpression) expression;
            memberExpression = body.Operand as MemberExpression;
        }
        else if (expression.NodeType == ExpressionType.MemberAccess)
        {
            memberExpression = expression as MemberExpression;
        }

        if (enforceCheck && memberExpression is null)
        {
            throw new ArgumentException("Not a member access", nameof(expression));
        }

        return memberExpression;
    }

    static Accessor getAccessor(MemberExpression memberExpression)
    {
        var list = new List<Member>();

        while (memberExpression is not null)
        {
            list.Add(memberExpression.Member.ToMember());
            memberExpression = memberExpression.Expression as MemberExpression;
        }

        if (list.Count == 1)
        {
            return new SingleMember(list[0]);
        }

        list.Reverse();
        return new PropertyChain(list.ToArray());
    }
}
