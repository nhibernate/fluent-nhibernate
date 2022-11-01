using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using static FluentNHibernate.Infrastructure.Serialization;

namespace FluentNHibernate.Infrastructure;

/// <summary>
/// Reference Via: https://github.com/CXuesong/BotBuilder.Standard/blob/netcore20%2Bnet45/CSharp/Library/Microsoft.Bot.Builder/Fibers/NetStandardSerialization.cs
/// </summary>
public static class NetStandardSerialization
{
    public sealed class TypeSerializationSurrogate : ISurrogateProvider
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            var type = (Type)obj;
            // BinaryFormatter in .NET Core 2.0 cannot persist types in System.Private.CoreLib.dll
            // that are not forwareded to mscorlib, including System.RuntimeType
            info.SetType(typeof(TypeReference));
            info.AddValue("AssemblyName", type.Assembly.FullName);
            info.AddValue("FullName", type.FullName);
        }

        public object SetObjectData(
            object obj, SerializationInfo info,
            StreamingContext context,
            ISurrogateSelector selector) => throw new NotSupportedException();

        public bool Handles(Type type, StreamingContext context) => typeof(Type).IsAssignableFrom(type);

        [Serializable]
        internal sealed class TypeReference : IObjectReference
        {
            private readonly string AssemblyName;

            private readonly string FullName;

            public TypeReference(Type type)
            {
                if (type == null)
                    throw new ArgumentNullException(nameof(type));

                AssemblyName = type.Assembly.FullName;
                FullName = type.FullName;
            }

            public object GetRealObject(StreamingContext context)
            {
                var assembly = Assembly.Load(AssemblyName);
                return assembly.GetType(FullName, true);
            }
        }
    }

    public sealed class MemberInfoSerializationSurrogate : ISurrogateProvider
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context) =>
            MemberInfoReference.GetObjectData((MemberInfo)obj, info, context);

        public object SetObjectData(
            object obj,
            SerializationInfo info,
            StreamingContext context,
            ISurrogateSelector selector) =>
            throw new NotSupportedException();

        public bool Handles(Type type, StreamingContext context) => typeof(MemberInfo).IsAssignableFrom(type);

        [Serializable]
        private sealed class MemberInfoReference : IObjectReference
        {
            private readonly Type DeclaringType = null;
            private readonly string Name = null;
            private readonly MemberTypes MemberType = default(MemberTypes);
            private readonly BindingFlags BindingAttr = default(BindingFlags);
            private readonly Type[] GenericParameters = null;
            private readonly Type[] Parameters = null;

            private static BindingFlags GetBindingAttr(MemberInfo member)
            {
                if (member == null) throw new ArgumentNullException(nameof(member));
                var bindingFlags = default(BindingFlags);
                switch (member)
                {
                    case MethodBase method:
                        bindingFlags |= method.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
                        bindingFlags |= method.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
                        break;

                    case FieldInfo field:
                        bindingFlags |= field.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
                        bindingFlags |= field.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
                        break;

                    default:
                        return BindingFlags.Public | BindingFlags.NonPublic
                                                   | BindingFlags.Static | BindingFlags.Instance;
                }
                return bindingFlags;
            }

            public static void GetObjectData(MemberInfo member, SerializationInfo info, StreamingContext context)
            {
                info.SetType(typeof(MemberInfoReference));
                info.AddValue(nameof(DeclaringType), new TypeSerializationSurrogate.TypeReference(member.DeclaringType));
                info.AddValue(nameof(Name), member.Name);
                info.AddValue(nameof(MemberType), member.MemberType);
                info.AddValue(nameof(BindingAttr), GetBindingAttr(member));
                if (member is MethodBase method)
                {
                    info.AddValue(nameof(GenericParameters),
                        method.GetGenericArguments().ToArray());
                    info.AddValue(nameof(Parameters),
                        method.GetParameters().Select(p => p.ParameterType).ToArray());
                }
            }

            private bool MatchMethodSignature(MethodBase method)
            {
                Debug.Assert(method.Name == Name);
                var gpa = method.GetGenericArguments();
                if (gpa.Length != GenericParameters.Length) return false;
                var pa = method.GetParameters();
                if (pa.Length != Parameters.Length) return false;
                if (gpa.Length > 0)
                {
                    var genericMethod = ((MethodInfo)method).MakeGenericMethod(GenericParameters);
                    pa = genericMethod.GetParameters();
                }
                for (int i = 0; i < pa.Length; i++)
                {
                    if (pa[i].ParameterType != Parameters[i]) return false;
                }
                return true;
            }

            public object GetRealObject(StreamingContext context)
            {
                if (MemberType == MemberTypes.Method || MemberType == MemberTypes.Constructor)
                {
                    var methods = DeclaringType.GetMember(Name, MemberType, BindingAttr | BindingFlags.DeclaredOnly);
                    if (Parameters.Any(t => t == null))
                        throw new ArgumentException("Detected null argument type in the method signature.",
                            nameof(Parameters));
                    if (Parameters.Any(t => t.IsGenericParameter))
                        throw new ArgumentException("Detected generic parameter in the method signature.",
                            nameof(Parameters));
                    try
                    {
                        return methods.Cast<MethodBase>().First(MatchMethodSignature);
                    }
                    catch (InvalidOperationException)
                    {
                        throw new MissingMethodException(DeclaringType.FullName, Name);
                    }
                }
                var members = DeclaringType.GetMember(Name, MemberType, BindingAttr | BindingFlags.DeclaredOnly);
                if (members.Length == 0) throw new MissingMemberException(DeclaringType.FullName, Name);
                if (members.Length > 1)
                    throw new AmbiguousMatchException($"Found multiple \"{Name}\" in \"{DeclaringType}\".");
                return members[0];
            }
        }
    }

    public sealed class SurrogateSelector : ISurrogateSelector
    {
        private readonly ISurrogateProvider _typeSerializationProvider;
        private readonly ISurrogateProvider _memberInfoSerializationProvider;

        public SurrogateSelector()
        {
            _typeSerializationProvider = new TypeSerializationSurrogate();
            _memberInfoSerializationProvider = new MemberInfoSerializationSurrogate();
        }

        void ISurrogateSelector.ChainSelector(ISurrogateSelector selector) => throw new NotImplementedException();

        ISurrogateSelector ISurrogateSelector.GetNextSelector() => throw new NotImplementedException();

        ISerializationSurrogate ISurrogateSelector.GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector)
        {
            if (_typeSerializationProvider.Handles(type, context))
            {
                selector = this;
                return _typeSerializationProvider;
            }

            if (_memberInfoSerializationProvider.Handles(type, context))
            {
                selector = this;
                return _memberInfoSerializationProvider;
            }

            selector = null;
            return null;
        }
    }
}
