using System;
using System.Reflection;

namespace ShadeTree.Testing
{
    public static class TestUtility
    {
        public static void FireEvent(object control, string eventName, params object[] args)
        {
            MethodInfo minfo =
                control.GetType().GetMethod("On" + eventName,
                                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            ParameterInfo[] param = minfo.GetParameters();
            Type parameterType = param[0].ParameterType;
            minfo.Invoke(control, new[] {Activator.CreateInstance(parameterType)});
        }
    }
}