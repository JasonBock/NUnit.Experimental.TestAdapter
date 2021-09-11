using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NUnit.Experimental.Hardcoded.TestAdapter.Tests;

namespace NUnit.Experimental.Hardcoded.TestAdapter;

[DefaultExecutorUri(Shared.ExecutorUri)]
public sealed class HardCodedTestDiscovery
	: ITestDiscoverer
{
	public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext,
		IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
	{
		// The idea is that all of the methods would be found and this would be "hard-coded"
		// via a source generator
		// I'm setting source to null as I really don't need it...at least I don't think I will.

		var executorUri = new Uri(Shared.ExecutorUri);

		var location = typeof(TimedTests).Assembly.Location;
		discoverySink.SendTestCase(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf500ms", executorUri, location));
		discoverySink.SendTestCase(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf1000ms", executorUri, location));
		discoverySink.SendTestCase(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf1500ms", executorUri, location));
		discoverySink.SendTestCase(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf2000ms", executorUri, location));
	}
}