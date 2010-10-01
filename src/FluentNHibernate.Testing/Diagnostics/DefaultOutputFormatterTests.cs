using System;
using FluentNHibernate.Diagnostics;
using NUnit.Framework;

namespace FluentNHibernate.Testing.Diagnostics
{
    [TestFixture]
    public class DefaultOutputFormatterTests
    {
        [Test]
        public void should_produce_simple_format()
        {
            var formatter = new DefaultOutputFormatter();
            var results = new DiagnosticResults(new[]
                {
                    new ScannedSource
                    {
                        Identifier = typeof(One).Assembly.GetName().FullName,
                        Phase = ScanPhase.FluentMappings
                    },
                    new ScannedSource
                    {
                        Identifier = typeof(One).Assembly.GetName().FullName,
                        Phase = ScanPhase.Conventions
                    }
                },
                new[] { typeof(Two), typeof(One) },
                new[] { typeof(Two), typeof(One) },
                new[]
                {
                    new SkippedAutomappingType
                    {
                        Type = typeof(One),
                        Reason = "first reason"
                    },
                    new SkippedAutomappingType
                    {
                        Type = typeof(Two),
                        Reason = "second reason"
                    },
                },
                new[] { typeof(Two), typeof(One) },
                new[]
                {
                    new AutomappingType
                    {
                        Type = typeof(One)
                    },
                    new AutomappingType
                    {
                        Type = typeof(Two)
                    },
                });
            var output = formatter.Format(results);

            output.ShouldEqual(
                "Fluent Mappings\r\n" +
                "---------------\r\n\r\n" +
                "Sources scanned:\r\n\r\n" +
                "  " + typeof(One).Assembly.GetName().FullName + "\r\n" +
                "\r\n" +
                "Mappings discovered:\r\n\r\n" +
                "  " + typeof(One).Name + " | " + typeof(One).AssemblyQualifiedName + "\r\n" +
                "  " + typeof(Two).Name + " | " + typeof(Two).AssemblyQualifiedName + "\r\n" +
                "\r\n" +
                "Conventions\r\n" +
                "-----------\r\n\r\n" +
                "Sources scanned:\r\n\r\n" +
                "  " + typeof(One).Assembly.GetName().FullName + "\r\n" +
                "\r\n" +
                "Conventions discovered:\r\n\r\n" +
                "  " + typeof(One).Name + " | " + typeof(One).AssemblyQualifiedName + "\r\n" +
                "  " + typeof(Two).Name + " | " + typeof(Two).AssemblyQualifiedName + "\r\n" +
                "\r\n" +
                "Automapping\r\n" +
                "-----------\r\n\r\n" +
                "Skipped types:\r\n\r\n" + 
                "  " + typeof(One).Name + " | first reason  | " + typeof(One).AssemblyQualifiedName + "\r\n" +
                "  " + typeof(Two).Name + " | second reason | " + typeof(Two).AssemblyQualifiedName + "\r\n" +
                "\r\n" +
                "Candidate types:\r\n\r\n" +
                "  " + typeof(One).Name + " | " + typeof(One).AssemblyQualifiedName + "\r\n" +
                "  " + typeof(Two).Name + " | " + typeof(Two).AssemblyQualifiedName + "\r\n" +
                "\r\n" + 
                "Mapped types:\r\n\r\n" +
                "  " + typeof(One).Name + " | " + typeof(One).AssemblyQualifiedName + "\r\n" +
                "  " + typeof(Two).Name + " | " + typeof(Two).AssemblyQualifiedName + "\r\n"
            );
        }

        class One { }
        class Two { }
    }
}