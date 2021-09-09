using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NUnit.Experimental.Hardcoded.TestAdapter.Tests;

namespace NUnit.Experimental.Hardcoded.TestAdapter;

[ExtensionUri(Shared.ExecutorUri)]
public sealed class HardCodedTestExecutor
	: ITestExecutor
{
	public void Cancel() => throw new NotImplementedException();

	private void RunTests(IRunContext runContext, IFrameworkHandle frameworkHandle)
	{
		frameworkHandle.SendMessage(TestMessageLevel.Informational, "RunTests started...");

		var location = typeof(TimedTests).Assembly.Location;
		TimedTests.TestOf500ms();
		frameworkHandle.RecordResult(new TestResult(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf500ms", new Uri(Shared.ExecutorUri), location)) { Outcome = TestOutcome.Passed });
		TimedTests.TestOf1000ms();
		frameworkHandle.RecordResult(new TestResult(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf1000ms", new Uri(Shared.ExecutorUri), location)) { Outcome = TestOutcome.Passed });
		TimedTests.TestOf1500ms();
		frameworkHandle.RecordResult(new TestResult(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf1500ms", new Uri(Shared.ExecutorUri), location)) { Outcome = TestOutcome.Passed });
		TimedTests.TestOf2000ms();
		frameworkHandle.RecordResult(new TestResult(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf2000ms", new Uri(Shared.ExecutorUri), location)) { Outcome = TestOutcome.Passed });

		frameworkHandle.SendMessage(TestMessageLevel.Informational, "RunTests finished.");
	}

	public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle) =>
		this.RunTests(runContext, frameworkHandle);

	public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle) =>
		this.RunTests(runContext, frameworkHandle);
}