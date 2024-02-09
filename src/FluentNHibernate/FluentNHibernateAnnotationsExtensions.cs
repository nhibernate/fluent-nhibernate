using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using NHibernate;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Id;

namespace FluentNHibernate;

public static class FluentNHibernateAnnotationsExtensions
{
    private static INHibernateLogger Log { get; set; }

    private static void UsingLog(Action<INHibernateLogger> log)
    {
        var currentLog = Log;
        if (currentLog == null)
        {
            currentLog = NHibernateLogger.For(typeof(FluentNHibernateAnnotationsExtensions));
            if (currentLog != null)
            {
                Log = currentLog;
                log?.Invoke(currentLog);
            }
        }
        else
        {
            log?.Invoke(currentLog);
        }
    }

    private static void Debug(string format, params object[] args)
    {
        UsingLog(log => log.Debug(format, args));
    }

    /// <summary>
    /// Try apply an attribute to target.
    /// </summary>
    /// <typeparam name="TTarget">Target type</typeparam>
    /// <typeparam name="TAttribute">Attribute type</typeparam>
    /// <param name="target">Target object</param>
    /// <param name="attribute">Attribute object</param>
    /// <param name="apply">Change the target object based on an attribute.
    /// [Apply] function should return true, only when an attribute be applied or the target object be changed.
    /// </param>
    /// <returns>
    /// Returns false where any arguments is null, otherwise return [Apply] function's result.
    /// </returns>
    public static bool TryApply<TTarget, TAttribute>(
        TTarget target,
        TAttribute attribute,
        Func<TTarget, TAttribute, bool> apply
    )
        where TAttribute : Attribute
    {
        if (target == null || attribute == null || apply == null)
        {
            return false;
        }
        return apply.Invoke(target, attribute);
    }

    /// <summary>
    /// Set ColumnMapping.NotNull when [Required] present.
    /// <see cref="RequiredAttribute"/>
    /// <see cref="ColumnMapping.NotNull"/>
    /// </summary>
    /// <param name="mapping">ColumnMapping</param>
    /// <param name="attribute">RequiredAttribute</param>
    /// <returns></returns>
    public static bool TryApply(this ColumnMapping mapping, RequiredAttribute attribute)
    {
        return TryApply(
            mapping,
            attribute,
            (mapping, attribute) =>
            {
                Debug(
                    "[{0}] TryApply(mapping={1}, attribute={2}) Set(NotNull=true)",
                    typeof(FluentNHibernateAnnotationsExtensions),
                    mapping,
                    attribute
                );

                mapping.Set(x => x.NotNull, Layer.Defaults, true);
                return true;
            }
        );
    }

    /// <summary>
    /// Set ColumnMapping.Length when [MaxLength] present.
    /// </summary>
    /// <see cref="ColumnMapping.Length"/>
    /// <param name="mapping">ColumnMapping</param>
    /// <param name="attribute">MaxLengthAttribute</param>
    /// <returns></returns>
    public static bool TryApply(this ColumnMapping mapping, MaxLengthAttribute attribute)
    {
        return TryApply(
            mapping,
            attribute,
            (mapping, attribute) =>
            {
                Debug(
                    "[{0}] TryApply(mapping={1}, attribute={2}) Set(Length={3})",
                    typeof(FluentNHibernateAnnotationsExtensions),
                    mapping,
                    attribute,
                    attribute.Length
                );

                mapping.Set(x => x.Length, Layer.Defaults, attribute.Length);
                return true;
            }
        );
    }

    /// <summary>
    /// Configure GeneratorMapping.Class to [assinged] when [DatabaseGenerated(DatabaseGeneratedOption.None)] present.
    /// Configure GeneratorMapping.Class to [identity] when [DatabaseGenerated(DatabaseGeneratedOption.Identity)] present.
    /// </summary>
    /// <see cref="GeneratorMapping.Class"/>
    /// <see cref="DatabaseGeneratedAttribute"/>
    /// <see cref="DatabaseGeneratedOption"/>
    /// <param name="mapping">GeneratorMapping</param>
    /// <param name="attribute">DatabaseGeneratedAttribute</param>
    /// <returns></returns>
    public static bool TryApply(this GeneratorMapping mapping, DatabaseGeneratedAttribute attribute)
    {
        return TryApply(
            mapping,
            attribute,
            (mapping, attribute) =>
            {
                var option = attribute.DatabaseGeneratedOption;
                if (option == DatabaseGeneratedOption.None)
                {
                    Debug(
                        "[{0}] TryApply(mapping={1}, attribute={2}) Set(Class={3})",
                        typeof(FluentNHibernateAnnotationsExtensions),
                        mapping,
                        attribute,
                        "assigned"
                    );

                    mapping.Set(x => x.Class, Layer.Defaults, "assigned");
                    return true;
                }
                if (option == DatabaseGeneratedOption.Identity)
                {
                    Debug(
                        "[{0}] TryApply(mapping={1}, attribute={2}) Set(Class={3})",
                        typeof(FluentNHibernateAnnotationsExtensions),
                        mapping,
                        attribute,
                        "identity"
                    );

                    mapping.Set(x => x.Class, Layer.Defaults, "identity");
                    return true;
                }
                return false;
            }
        );
    }

    public static void TryApplyAttributesFrom(
        this ColumnMapping mapping,
        MemberInfo memberInfo,
        bool isIdColumn
    )
    {
        //Prefer RequiredAttribute.
        bool requiredApplied = mapping.TryApply(memberInfo.GetCustomAttribute<RequiredAttribute>());
        //GetType().IsNullable.
        if ((!requiredApplied) && (!memberInfo.GetType().IsNullable()))
        {
            RequiredAttribute requiredAttribute = new ();
            Debug(
                "[{0}] TryApply(mapping={1}, attribute={2}) Sender=TryApplyAttributesFrom Cause=!GetType().IsNullable()",
                typeof(FluentNHibernateAnnotationsExtensions),
                mapping,
                requiredAttribute
            );
            mapping.TryApply(requiredAttribute);
        }
        mapping.TryApply(memberInfo.GetCustomAttribute<MaxLengthAttribute>());
    }
}
