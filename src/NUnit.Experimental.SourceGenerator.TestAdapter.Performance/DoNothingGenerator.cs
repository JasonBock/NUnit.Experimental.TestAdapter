using Microsoft.CodeAnalysis;

namespace NUnit.Experimental.SourceGenerator.TestAdapter.Performance
{
	[Generator]
	public sealed class DoNothingGenerator
		: ISourceGenerator
	{
		public void Execute(GeneratorExecutionContext context) { }
		public void Initialize(GeneratorInitializationContext context) { }
	}
}