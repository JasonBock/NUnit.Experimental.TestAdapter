using BenchmarkDotNet.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace NUnit.Experimental.SourceGenerator.TestAdapter.Performance;

[MemoryDiagnoser]
public class GeneratorTests
{
	private readonly CSharpCompilation compilation;
	private readonly CSharpGeneratorDriver generatorDriver;
	private readonly CSharpGeneratorDriver doNothingDriver;

	public GeneratorTests()
	{
		var source =
@"using NUnit.Framework;

namespace TargetTests
{
	public static class MyTests
	{
		[Test]
		public static void Foo() { }
	}
}";
		var syntaxTree = CSharpSyntaxTree.ParseText(source);
		var references = AppDomain.CurrentDomain.GetAssemblies()
			.Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
			.Select(_ => MetadataReference.CreateFromFile(_.Location))
			.Concat(new[] 
			{ 
				MetadataReference.CreateFromFile(typeof(SourceGeneratorTestGenerator).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(TestAttribute).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(DefaultExecutorUriAttribute).Assembly.Location),
			});
		this.compilation = CSharpCompilation.Create("generator",
			new SyntaxTree[] { syntaxTree },
			references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
		this.generatorDriver = CSharpGeneratorDriver.Create(ImmutableArray.Create<ISourceGenerator>(new SourceGeneratorTestGenerator()));
		this.doNothingDriver = CSharpGeneratorDriver.Create(ImmutableArray.Create<ISourceGenerator>(new DoNothingGenerator()));
	}

	[Benchmark]
	public void Generate() => 
		this.generatorDriver.RunGeneratorsAndUpdateCompilation(this.compilation, out _, out _);

	[Benchmark]
	public void GenerateDoNothing() =>
		this.doNothingDriver.RunGeneratorsAndUpdateCompilation(this.compilation, out _, out _);
}