using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NUnit.Experimental.Hardcoded.TestAdapter.Tests;

namespace NUnit.Experimental.Hardcoded.TestAdapter;

[ExtensionUri(Shared.ExecutorUri)]
public sealed class HardCodedTestExecutor
	: ITestExecutor
{
	public void Cancel() => throw new NotImplementedException();

	private static void RunTests(IFrameworkHandle frameworkHandle)
	{
		var location = typeof(TimedTests).Assembly.Location;
		var executorUri = new Uri(Shared.ExecutorUri);

		TimedTests.TestOf10ms();
		frameworkHandle.RecordResult(new TestResult(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf10ms", executorUri, location)) { Outcome = TestOutcome.Passed });
		TimedTests.TestOf500ms();
		frameworkHandle.RecordResult(new TestResult(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf500ms", executorUri, location)) { Outcome = TestOutcome.Passed });
		TimedTests.TestOf1000ms();
		frameworkHandle.RecordResult(new TestResult(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf1000ms", executorUri, location)) { Outcome = TestOutcome.Passed });
		TimedTests.TestOf1500ms();
		frameworkHandle.RecordResult(new TestResult(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf1500ms", executorUri, location)) { Outcome = TestOutcome.Passed });
		TimedTests.TestOf2000ms();
		frameworkHandle.RecordResult(new TestResult(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf2000ms", executorUri, location)) { Outcome = TestOutcome.Passed });
	}

	public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle) =>
		HardCodedTestExecutor.RunTests(frameworkHandle);

	public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle) =>
		HardCodedTestExecutor.RunTests(frameworkHandle);
}