using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace NUnit.Experimental.TestAdapter.Tests;

[DefaultExecutorUri("executor://NUnitExperimentalTestAdapter")]
public sealed class HardCodedTestDiscovery
	: ITestDiscoverer
{
	public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext,
		IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
	{
		logger.SendMessage(TestMessageLevel.Informational, $"{nameof(HardCodedTestDiscovery)} - discovery started...");

		foreach(var source in sources)
		{
			logger.SendMessage(TestMessageLevel.Informational, $"{nameof(source)} is {source}");
		}
	}
}
