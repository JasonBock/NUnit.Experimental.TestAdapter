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
		logger.SendMessage(TestMessageLevel.Informational, $"{nameof(HardCodedTestDiscovery)} - discovery started...");

		// The idea is that all of the methods would be found and this would be "hard-coded"
		// via a source generator
		// I'm setting source to null as I really don't need it...at least I don't think I will.

		var location = typeof(TimedTests).Assembly.Location;
		discoverySink.SendTestCase(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf500ms", new Uri(Shared.ExecutorUri), location));
		discoverySink.SendTestCase(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf1000ms", new Uri(Shared.ExecutorUri), location));
		discoverySink.SendTestCase(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf1500ms", new Uri(Shared.ExecutorUri), location));
		discoverySink.SendTestCase(new TestCase("NUnit.Experimental.Hardcoded.TestAdapter.Tests.TimedTests::TestOf2000ms", new Uri(Shared.ExecutorUri), location));

		logger.SendMessage(TestMessageLevel.Informational, $"{nameof(HardCodedTestDiscovery)} - discovery finished.");
	}
}