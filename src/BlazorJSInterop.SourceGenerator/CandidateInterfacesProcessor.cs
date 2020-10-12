using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorJSInterop.SourceGenerator.Attributes;
using BlazorJSInterop.SourceGenerator.Diagnostics;
using BlazorJSInterop.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlazorJSInterop.SourceGenerator
{
    internal class CandidateInterfacesProcessor
    {
        private readonly SourceGeneratorContext _context;
        private readonly List<InterfaceDeclarationSyntax> _interfaceDeclarationSyntaxes;
        private readonly INamedTypeSymbol _interfaceAttributeSymbol;
        private readonly INamedTypeSymbol _methodAttributeSymbol;
        private readonly DiagnosticReporter _diagnosticReporter;

        internal CandidateInterfacesProcessor(
            SourceGeneratorContext context,
            List<InterfaceDeclarationSyntax> interfaceDeclarationSyntaxes,
            INamedTypeSymbol interfaceAttributeSymbol,
            INamedTypeSymbol methodAttributeSymbol,
            DiagnosticReporter diagnosticReporter)
        {
            _context = context;
            _interfaceDeclarationSyntaxes = interfaceDeclarationSyntaxes;
            _interfaceAttributeSymbol = interfaceAttributeSymbol;
            _methodAttributeSymbol = methodAttributeSymbol;
            _diagnosticReporter = diagnosticReporter;
        }

        internal bool IsCandidateInterface(INamedTypeSymbol interfaceSymbol) =>
            interfaceSymbol.GetAttributes().Any(attributeData =>
                attributeData.AttributeClass.Equals(_interfaceAttributeSymbol, SymbolEqualityComparer.Default));

        internal List<ValidInterfaceInfo> GetValidInterfaceInfoList()
        {
            var validInterfaceInfoList = new List<ValidInterfaceInfo>();

            foreach (var interfaceDeclarationSyntax in _interfaceDeclarationSyntaxes)
            {
                var model = _context.Compilation.GetSemanticModel(interfaceDeclarationSyntax.SyntaxTree);
                var interfaceSymbol =
                    (INamedTypeSymbol) ModelExtensions.GetDeclaredSymbol(model, interfaceDeclarationSyntax);

                if (!IsCandidateInterface(interfaceSymbol))
                    continue;

                validInterfaceInfoList.Add(GetValidInterfaceInfo(interfaceDeclarationSyntax, interfaceSymbol, model));
            }

            return validInterfaceInfoList;
        }

        private ValidInterfaceInfo GetValidInterfaceInfo(
            InterfaceDeclarationSyntax interfaceDeclarationSyntax,
            INamedTypeSymbol interfaceSymbol,
            SemanticModel model)
        {
            var methodSymbols = new List<IMethodSymbol>();

            foreach (var method in interfaceDeclarationSyntax.Members)
            {
                var methodSymbol = model.GetDeclaredSymbol(method) as IMethodSymbol;
                var interfaceFullName = $"{interfaceSymbol.ContainingNamespace}.{interfaceSymbol.Name}";
                var methodName = methodSymbol.Name;

                if (methodSymbol.GetAttributes().Any(attributeData =>
                    attributeData.AttributeClass.Equals(_methodAttributeSymbol, SymbolEqualityComparer.Default)))
                {
                    methodSymbols.Add(methodSymbol);
                }
                else
                {
                    _diagnosticReporter.ReportMethodDiagnostic(
                        DiagnosticType.MethodWithoutAttribute,
                        methodName,
                        interfaceFullName,
                        typeof(BlazorJSInteropMethodAttribute).FullName,
                        methodSymbol.Locations);
                }

                if (!methodSymbol.ReturnType.IsValueTaskType())
                {
                    _diagnosticReporter.ReportMethodDiagnostic(
                        DiagnosticType.MethodInvalidReturnType,
                        methodName,
                        interfaceFullName,
                        typeof(ValueTask).FullName,
                        methodSymbol.Locations);
                }
            }

            return new ValidInterfaceInfo(interfaceSymbol.Name, interfaceSymbol.ContainingNamespace.ToString(),
                methodSymbols);
        }
    }
}