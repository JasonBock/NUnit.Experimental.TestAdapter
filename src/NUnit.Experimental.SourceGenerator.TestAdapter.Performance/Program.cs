using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using NUnit.Experimental.SourceGenerator.TestAdapter.Performance;

//new GeneratorTests().Generate();
BenchmarkRunner.Run<GeneratorTests>(
	ManualConfig.Create(DefaultConfig.Instance)
		.WithOptions(ConfigOptions.DisableOptimizationsValidator));