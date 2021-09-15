using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

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
			var methodSymbol = model.GetDeclaredSymbol(syntaxNode) as IMethodSymbol;

			if(methodSymbol is not null)
			{
				foreach (var methodAttribute in methodSymbol.GetAttributes())
				{
					var methodAttributeType = methodAttribute.AttributeClass!;

					if(methodAttributeType.Name == "TestAttribute" &&
						methodAttributeType.ContainingNamespace.GetName() == "NUnit.Framework" &&
						methodAttributeType.ContainingAssembly.Name == "nunit.framework")
					{
						this.Targets.Add(methodSymbol);
					}
				}
			}
		}
	}

	public List<IMethodSymbol> Targets { get; } = new();
}