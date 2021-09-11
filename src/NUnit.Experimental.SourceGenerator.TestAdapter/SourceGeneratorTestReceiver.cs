using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace NUnit.Experimental.SourceGenerator.TestAdapter;

public sealed class SourceGeneratorTestReceiver
	: ISyntaxContextReceiver
{
	public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
	{
		var syntaxNode = context.Node;
		var model = context.SemanticModel;

		if (syntaxNode is MethodDeclarationSyntax)
		{
			var testAttributeSymbol = model.Compilation.GetTypeByMetadataName(typeof(TestAttribute).FullName!);
			var methodSymbol = model.GetDeclaredSymbol(syntaxNode) as IMethodSymbol;

			if(methodSymbol is not null)
			{
				foreach (var methodAttribute in methodSymbol.GetAttributes())
				{
					if (SymbolEqualityComparer.Default.Equals(methodAttribute.AttributeClass!, testAttributeSymbol) &&
						methodSymbol.IsStatic && methodSymbol.DeclaredAccessibility == Accessibility.Public &&
						methodSymbol.Parameters.Length == 0 && !methodSymbol.IsAsync)
					{
						this.Targets.Add(methodSymbol);
					}
				}
			}
		}
	}

	public List<IMethodSymbol> Targets { get; } = new();
}