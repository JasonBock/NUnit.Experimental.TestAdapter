using Microsoft.CodeAnalysis;

namespace NUnit.Experimental.SourceGenerator.TestAdapter;

[Generator]
public sealed class SourceGeneratorTestDiscovererGenerator
	: ISourceGenerator
{
	public void Execute(GeneratorExecutionContext context)
	{
		if (context.SyntaxContextReceiver is SourceGeneratorTestDiscovererReceiver receiver &&
			receiver.Targets.Count > 0)
		{
			var discovererBuilder = new DiscovererBuilder(receiver.Targets);
			context.AddSource($"{Shared.DiscovererName}.cs", discovererBuilder.Text);
		}
	}

	public void Initialize(GeneratorInitializationContext context) =>
		context.RegisterForSyntaxNotifications(() => new SourceGeneratorTestDiscovererReceiver());
}