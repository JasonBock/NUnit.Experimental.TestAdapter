using Microsoft.CodeAnalysis;

namespace NUnit.Experimental.SourceGenerator.TestAdapter;

internal static class INamespaceSymbolExtensions
{
	internal static bool Contains(this INamespaceSymbol self, INamespaceSymbol other) =>
		self.GetName().Contains(other.GetName());

	internal static string GetName(this INamespaceSymbol? self) =>
		self?.ToDisplayString(SymbolDisplayFormat.CSharpErrorMessageFormat) ?? string.Empty;
}