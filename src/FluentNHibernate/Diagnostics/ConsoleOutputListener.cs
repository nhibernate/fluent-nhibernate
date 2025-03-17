using System;
using System.IO;

namespace FluentNHibernate.Diagnostics;

public class ConsoleOutputListener(IDiagnosticResultsFormatter formatter) : IDiagnosticListener
{
    public ConsoleOutputListener()
        : this(new DefaultOutputFormatter())
    {}

    public void Receive(DiagnosticResults results)
    {
        var output = formatter.Format(results);

        Console.WriteLine(output);
    }
}

public class FileOutputListener(IDiagnosticResultsFormatter formatter, string outputPath) : IDiagnosticListener
{
    public FileOutputListener(string outputPath)
        : this(new DefaultOutputFormatter(), outputPath)
    {}

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
