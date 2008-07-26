using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StoryTeller.FixtureModel
{
    /// <remarks>
    /// Imported from StoryTeller project, as this was the only dependency there.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class ExampleAttribute : Attribute
    {
        private readonly string _example;

        public ExampleAttribute(string example)
        {
            _example = example;
        }


        public string Example
        {
            get { return _example; }
        }

        public static string GetExample(MethodInfo method, string defaultText)
        {
            ExampleAttribute attribute = Attribute.GetCustomAttribute(method, typeof (ExampleAttribute)) as ExampleAttribute;
            return attribute == null ? defaultText : attribute.Example;
        }
    }
}
