using BenchmarkDotNet.Attributes;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using NUnit.Experimental.Hardcoded.TestAdapter.Tests;
using Rocks;
using System.Collections.Generic;

namespace NUnit.Experimental.Hardcoded.TestAdapter.Performance
{
	[MemoryDiagnoser]
	public class DiscovererTests
	{
		private readonly IDiscoveryContext discoveryContext =
			Rock.Make<IDiscoveryContext>().Instance();
		private readonly ITestCaseDiscoverySink discoverySink =
			Rock.Make<ITestCaseDiscoverySink>().Instance();
		private readonly IMessageLogger messageLogger =
			Rock.Make<IMessageLogger>().Instance();
		private readonly IEnumerable<string> sources;

		public DiscovererTests()
		{
			var testSource = typeof(TimedTests).Assembly.Location;
			this.sources = new string[] { testSource };
		}

		[Benchmark(Baseline = true)]
		public void HardCodedDiscovery() => 
			new HardCodedTestDiscovery().DiscoverTests(this.sources, this.discoveryContext, this.messageLogger, this.discoverySink);

		[Benchmark]
		public void ReflectionDiscovery() =>
			new ReflectionTestDiscovery().DiscoverTests(this.sources, this.discoveryContext, this.messageLogger, this.discoverySink);
	}
}