using Microsoft.CodeAnalysis;

namespace NUnit.Experimental.SourceGenerator.TestAdapter;

[Generator]
public sealed class SourceGeneratorTestGenerator
	: ISourceGenerator
{
	public void Execute(GeneratorExecutionContext context)
	{
		if (context.SyntaxContextReceiver is SourceGeneratorTestReceiver receiver &&
			receiver.Targets.Count > 0)
		{
			var targetsByType = receiver.Targets.GroupBy(_ => _.ContainingType, SymbolEqualityComparer.Default).ToList();
			var discovererBuilder = new DiscovererBuilder(targetsByType);
			context.AddSource($"{Shared.DiscovererName}.cs", discovererBuilder.Text);
			var executorBuilder = new ExecutorBuilder(targetsByType);
			context.AddSource($"{Shared.ExecutorName}.cs", executorBuilder.Text);
		}
	}

	public void Initialize(GeneratorInitializationContext context) =>
		context.RegisterForSyntaxNotifications(() => new SourceGeneratorTestReceiver());
}