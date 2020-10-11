using System.Collections.Generic;
using System.Linq;
using BlazorJSInterop.SourceGenerator.Exceptions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlazorJSInterop.SourceGenerator
{
    internal class CandidateInterfacesProcessor
    {
        private readonly INamedTypeSymbol _interfaceAttributeSymbol;
        private readonly INamedTypeSymbol _methodAttributeSymbol;

        internal CandidateInterfacesProcessor(
            INamedTypeSymbol interfaceAttributeSymbol,
            INamedTypeSymbol methodAttributeSymbol)
        {
            _interfaceAttributeSymbol = interfaceAttributeSymbol;
            _methodAttributeSymbol = methodAttributeSymbol;
        }

        internal bool IsCandidateInterface(INamedTypeSymbol interfaceSymbol) =>
            interfaceSymbol.GetAttributes().Any(attributeData =>
                attributeData.AttributeClass.Equals(_interfaceAttributeSymbol, SymbolEqualityComparer.Default));

        internal ValidInterfaceInfo GetValidInterfaceInfo(
            InterfaceDeclarationSyntax interfaceDeclarationSyntax,
            INamedTypeSymbol interfaceSymbol,
            SemanticModel model)
        {
            var methodSymbols = new List<IMethodSymbol>();

            foreach (var method in interfaceDeclarationSyntax.Members)
            {
                var methodSymbol = model.GetDeclaredSymbol(method) as IMethodSymbol;

                if (methodSymbol.GetAttributes().Any(attributeData =>
                    attributeData.AttributeClass.Equals(_methodAttributeSymbol, SymbolEqualityComparer.Default)))
                {
                    methodSymbols.Add(methodSymbol);
                }
            }

            if (interfaceDeclarationSyntax.Members.Count != methodSymbols.Count)
                throw new InterfaceMethodDoesNotHaveAttributeException(
                    $"{interfaceSymbol.ContainingNamespace}.{interfaceSymbol.Name}");

            return new ValidInterfaceInfo(interfaceSymbol.Name, interfaceSymbol.ContainingNamespace.ToString(),
                methodSymbols);
        }
    }
}