using FluentNHibernate.Diagnostics;
using FluentNHibernate.Testing.Utils;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentNHibernate.Testing.Diagnostics
{
    [TestFixture]
    public class DiagnosticConfigurationTests
    {
        [Test]
        public void enabling_should_set_logger_to_default_impl()
        {
            IDiagnosticLogger logger = null;

            new DiagnosticsConfiguration(null, l => logger = l)
                .Enable();

            logger.ShouldBeOfType<DefaultDiagnosticLogger>();
        }

        [Test]
        public void disabling_should_set_logger_to_null_impl()
        {
            IDiagnosticLogger logger = null;

            new DiagnosticsConfiguration(null, l => logger = l)
                .Disable();

            logger.ShouldBeOfType<NullDiagnosticsLogger>();
        }

        [Test]
        public void adding_listener_should_add_listener_to_underlying_despatcher()
        {
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();
            var listener = Stub<IDiagnosticListener>.Create();

            new DiagnosticsConfiguration(despatcher, l => { })
                .RegisterListener(listener);

            despatcher.AssertWasCalled(x => x.RegisterListener(listener));
        }

        [Test]
        public void output_to_console_should_register_console_listener()
        {
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();

            new DiagnosticsConfiguration(despatcher, l => { })
                .OutputToConsole();

            despatcher.AssertWasCalled(x => x.RegisterListener(Arg<ConsoleOutputListener>.Is.TypeOf));
        }

        [Test]
        public void output_to_file_should_register_console_listener()
        {
            var despatcher = Mock<IDiagnosticMessageDespatcher>.Create();

            new DiagnosticsConfiguration(despatcher, l => { })
                .OutputToFile("path");

            despatcher.AssertWasCalled(x => x.RegisterListener(Arg<FileOutputListener>.Is.TypeOf));
        }
    }
}
