using BenchmarkDotNet.Attributes;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using NUnit.Experimental.Hardcoded.TestAdapter.Tests;
using Rocks;
using System.Collections.Generic;

namespace NUnit.Experimental.Hardcoded.TestAdapter.Performance
{
	[MemoryDiagnoser]
	public class ExecutorTests
	{
		private readonly IFrameworkHandle frameworkHandle =
			Rock.Make<IFrameworkHandle>().Instance();
		private readonly IRunContext runContext =
			Rock.Make<IRunContext>().Instance();
		private readonly IEnumerable<string> sources;

		public ExecutorTests()
		{
			var testSource = typeof(TimedTests).Assembly.Location;
			this.sources = new string[] { testSource };
		}

		[Benchmark(Baseline = true)]
		public void HardCodedExecution() =>
			new HardCodedTestExecutor().RunTests(this.sources, this.runContext, this.frameworkHandle); 

		[Benchmark]
		public void ReflectionExecution() =>
			new ReflectionTestExecutor().RunTests(this.sources, this.runContext, this.frameworkHandle);
	}
}