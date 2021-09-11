using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System.CodeDom.Compiler;
using System.Text;

namespace NUnit.Experimental.SourceGenerator.TestAdapter;

internal sealed class ExecutorBuilder
{
	internal ExecutorBuilder(List<IGrouping<ISymbol?, IMethodSymbol>> targets)
	{
		var namespaces = new NamespaceGatherer();
		namespaces.Add(typeof(ExtensionUriAttribute));
		namespaces.Add(typeof(ITestExecutor));
		namespaces.Add(typeof(IFrameworkHandle));
		namespaces.Add(typeof(IEnumerable<>));
		namespaces.Add(typeof(TestCase));
		namespaces.Add(typeof(IRunContext));
		namespaces.Add(typeof(Uri));
		namespaces.Add(typeof(NotImplementedException));

		foreach (var targetType in targets)
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

		indentWriter.WriteLine($"[ExtensionUri(\"{Shared.ExecutorUri}\")]");
		indentWriter.WriteLine($"public sealed class {Shared.ExecutorName}");
		indentWriter.Indent++;
		indentWriter.WriteLine($": ITestExecutor");
		indentWriter.Indent--;

		indentWriter.WriteLine("{");
		indentWriter.Indent++;

		indentWriter.WriteLine("public void Cancel() => throw new NotImplementedException();");
		indentWriter.WriteLine();
		indentWriter.WriteLine("private static void RunTests(IFrameworkHandle frameworkHandle)");

		indentWriter.WriteLine("{");
		indentWriter.Indent++;

		indentWriter.WriteLine($"var location = typeof({targets[0].Key!.Name}).Assembly.Location;");
		indentWriter.WriteLine($"var executorUri = new Uri(\"{Shared.ExecutorUri}\");");

		foreach (var targetType in targets)
		{
			var typeName = $"{targetType.Key!.ContainingNamespace.GetName()}.{targetType.Key!.Name}";

			foreach (var targetMethod in targetType)
			{
				indentWriter.WriteLine($"{targetType.Key!.Name}.{targetMethod.Name}();");
				indentWriter.WriteLine($"frameworkHandle.RecordResult(new TestResult(new TestCase(\"{typeName}::{targetMethod.Name}\", executorUri, location)) {{ Outcome = TestOutcome.Passed }});");
			}
		}

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		indentWriter.WriteLine();

		indentWriter.WriteLine("public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle) =>");
		indentWriter.Indent++;
		indentWriter.WriteLine($"{Shared.ExecutorName}.RunTests(frameworkHandle);");
		indentWriter.Indent--;

		indentWriter.WriteLine();

		indentWriter.WriteLine("public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle) =>");
		indentWriter.Indent++;
		indentWriter.WriteLine($"{Shared.ExecutorName}.RunTests(frameworkHandle);");
		indentWriter.Indent--;

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		indentWriter.Indent--;
		indentWriter.WriteLine("}");

		this.Text = SourceText.From(writer.ToString(), Encoding.UTF8);
	}

	internal SourceText Text { get; }
}