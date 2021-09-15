using Microsoft.CodeAnalysis.Testing;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace NUnit.Experimental.SourceGenerator.TestAdapter.Tests;

public static class SourceGeneratorTestGeneratorTests
{
	[Test]
	public static async Task GenerateAsync()
	{
		var code =
@"using NUnit.Framework;

namespace TargetTests
{
	public static class MyTests
	{
		[Test]
		public static void Foo() { }
	}
}";

		var discovererGeneratedCode =
@"using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using TargetTests;

#nullable enable

namespace NUnit.Experimental.SourceGenerator.TestAdapter
{
	[DefaultExecutorUri(""executor://NUnitExperimentalSourceGeneratorTestAdapter"")]
	public sealed class SourceGeneratorTestDiscoverer
		: ITestDiscoverer
	{
		public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext,
			IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
		{
			var executorUri = new Uri(""executor://NUnitExperimentalSourceGeneratorTestAdapter"");
			var location = typeof(MyTests).Assembly.Location;
			discoverySink.SendTestCase(new TestCase(""TargetTests.MyTests::Foo"", executorUri, location));
		}
	}
}
";
		var executorGeneratedCode =
@"using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System;
using System.Collections.Generic;
using TargetTests;

#nullable enable

namespace NUnit.Experimental.SourceGenerator.TestAdapter
{
	[ExtensionUri(""executor://NUnitExperimentalSourceGeneratorTestAdapter"")]
	public sealed class SourceGeneratorTestExecutor
		: ITestExecutor
	{
		public void Cancel() => throw new NotImplementedException();
		
		private static void RunTests(IFrameworkHandle frameworkHandle)
		{
			var location = typeof(MyTests).Assembly.Location;
			var executorUri = new Uri(""executor://NUnitExperimentalSourceGeneratorTestAdapter"");
			MyTests.Foo();
			frameworkHandle.RecordResult(new TestResult(new TestCase(""TargetTests.MyTests::Foo"", executorUri, location)) { Outcome = TestOutcome.Passed });
		}
		
		public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle) =>
			SourceGeneratorTestExecutor.RunTests(frameworkHandle);
		
		public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle) =>
			SourceGeneratorTestExecutor.RunTests(frameworkHandle);
	}
}
";

		await TestAssistants.RunAsync(code,
			new[] 
			{ 
				(typeof(SourceGeneratorTestGenerator), $"{Shared.DiscovererName}.cs", discovererGeneratedCode),
				(typeof(SourceGeneratorTestGenerator), $"{Shared.ExecutorName}.cs", executorGeneratedCode),
			},
			Enumerable.Empty<DiagnosticResult>());
	}
}