using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NUnit.Experimental.SourceGenerator.TestAdapter;

internal sealed class DiscovererBuilder
{
	internal DiscovererBuilder(List<IGrouping<ISymbol?, IMethodSymbol>> targets)
	{
		var namespaces = new NamespaceGatherer();
		namespaces.Add("Microsoft.VisualStudio.TestPlatform.ObjectModel");
		namespaces.Add("Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter");
		namespaces.Add("Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging");
		namespaces.Add(typeof(IEnumerable<>));
		namespaces.Add(typeof(Uri));

		foreach(var targetType in targets)
		{
			namespaces.Add(targetType.Key!.ContainingNamespace);
		}

		using var writer = new StringWriter();
		using var indentWriter = new IndentedTextWriter(writer, "\t");

		foreach(var @namespace in namespaces.Values)
		{
			indentWriter.WriteLine($"using {@namespace};");
		}

		indentWriter.WriteLine();
		indentWriter.WriteLine("#nullable enable");

		indentWriter.WriteLine();
		indentWriter.WriteLine($"namespace {this.GetType().Namespace}");

		indentWriter.WriteLine("{");
		indentWriter.Indent++;

		indentWriter.WriteLine($"[DefaultExecutorUri(\"{Shared.ExecutorUri}\")]");
		indentWriter.WriteLine($"public sealed class {Shared.DiscovererName}");
		indentWriter.Indent++;
		indentWriter.WriteLine($": ITestDiscoverer");
		indentWriter.Indent--;

		indentWriter.WriteLine("{");
		indentWriter.Indent++;

		indentWriter.WriteLine("public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext,");
		indentWriter.Indent++;
		indentWriter.WriteLine("IMessageLogger logger, ITestCaseDiscoverySink discoverySink)");
		indentWriter.Indent--;

		indentWriter.WriteLine("{");
		indentWriter.Indent++;

		indentWriter.WriteLine($"var executorUri = new Uri(\"{Shared.ExecutorUri}\");");
		indentWriter.WriteLine($"var location = typeof({targets[0].Key!.Name}).Assembly.Location;");

		foreach (var targetType in targets)
		{
			var typeName = $"{targetType.Key!.ContainingNamespace.GetName()}.{targetType.Key!.Name}";

			foreach(var targetMethod in targetType)
			{
				indentWriter.WriteLine($"discoverySink.SendTestCase(new TestCase(\"{typeName}::{targetMethod.Name}\", executorUri, location));");
			}
		}

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		this.Text = SourceText.From(writer.ToString(), Encoding.UTF8);
	}

	internal SourceText Text { get; }
}