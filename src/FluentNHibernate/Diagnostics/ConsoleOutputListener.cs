using System;
using System.IO;

namespace FluentNHibernate.Diagnostics
{
    public class ConsoleOutputListener : IDiagnosticListener
    {
        readonly IDiagnosticResultsFormatter formatter;

        public ConsoleOutputListener()
            : this(new DefaultOutputFormatter())
        {}

        public ConsoleOutputListener(IDiagnosticResultsFormatter formatter)
        {
            this.formatter = formatter;
        }

        public void Receive(DiagnosticResults results)
        {
            var output = formatter.Format(results);

            Console.WriteLine(output);
        }
    }

    public class FileOutputListener : IDiagnosticListener
    {
        readonly IDiagnosticResultsFormatter formatter;
        readonly string outputPath;

        public FileOutputListener(string outputPath)
            : this(new DefaultOutputFormatter(), outputPath)
        {}

        public FileOutputListener(IDiagnosticResultsFormatter formatter, string outputPath)
        {
            this.formatter = formatter;
            this.outputPath = outputPath;
        }

        public void Receive(DiagnosticResults results)
        {
            var output = formatter.Format(results);
            var outputDirectory = Path.GetDirectoryName(outputPath);

            if (string.IsNullOrEmpty(outputDirectory))
                throw new ArgumentException("Output directory is invalid", "outputPath");

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            File.WriteAllText(outputPath, output);
        }
    }
}