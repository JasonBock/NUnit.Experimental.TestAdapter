using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

namespace NUnit.Experimental.SourceGenerator.TestAdapter.Tests;

using GeneratorTest = CSharpSourceGeneratorTest<SourceGeneratorTestGenerator, NUnitVerifier>;

internal static class TestAssistants
{
	internal static async Task RunAsync(string code,
		IEnumerable<(Type, string, string)> generatedSources,
		IEnumerable<DiagnosticResult> expectedDiagnostics)
	{
		var test = new GeneratorTest
		{
			ReferenceAssemblies = ReferenceAssemblies.Net.Net50,
			TestState =
				{
					Sources = { code },
				},
		};

		foreach (var generatedSource in generatedSources)
		{
			test.TestState.GeneratedSources.Add(generatedSource);
		}

		test.TestState.AdditionalReferences.Add(typeof(SourceGeneratorTestGenerator).Assembly);
		test.TestState.AdditionalReferences.Add(typeof(TestAttribute).Assembly);
		test.TestState.AdditionalReferences.Add(typeof(DefaultExecutorUriAttribute).Assembly);
		test.TestState.ExpectedDiagnostics.AddRange(expectedDiagnostics);
		await test.RunAsync();
	}
}