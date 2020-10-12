using System.Collections.Generic;
using System.Text;
using BlazorJSInterop.SourceGenerator.Attributes;
using BlazorJSInterop.SourceGenerator.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace BlazorJSInterop.SourceGenerator
{
    [Generator]
    internal class ImplementationGenerator : ISourceGenerator
    {
        public void Execute(SourceGeneratorContext context)
        {
            if (!(context.SyntaxReceiver is SyntaxReceiver syntaxReceiver))
                return;

            var interfaceAttributeSymbol =
                context.Compilation.GetTypeByMetadataName(typeof(BlazorJSInteropSourceAttribute).FullName);
            var methodAttributeSymbol =
                context.Compilation.GetTypeByMetadataName(typeof(BlazorJSInteropMethodAttribute).FullName);

            var diagnosticReporter = new DiagnosticReporter(context);

            var candidateInterfacesProcessor =
                new CandidateInterfacesProcessor(interfaceAttributeSymbol, methodAttributeSymbol, diagnosticReporter);

            var validInterfaceInfoList = new List<ValidInterfaceInfo>();
            foreach (var interfaceDeclarationSyntax in syntaxReceiver.CandidateInterfaces)
            {
                var model = context.Compilation.GetSemanticModel(interfaceDeclarationSyntax.SyntaxTree);
                var interfaceSymbol =
                    (INamedTypeSymbol) ModelExtensions.GetDeclaredSymbol(model, interfaceDeclarationSyntax);

                if (!candidateInterfacesProcessor.IsCandidateInterface(interfaceSymbol))
                    continue;

                validInterfaceInfoList.Add(
                    candidateInterfacesProcessor.GetValidInterfaceInfo(interfaceDeclarationSyntax, interfaceSymbol,
                        model));
            }

            var sourceCodeBuilder = new SourceCodeBuilder(interfaceAttributeSymbol);

            foreach (var validInterfaceInfo in validInterfaceInfoList)
            {
                var sourceCode = sourceCodeBuilder.BuildSourceCode(validInterfaceInfo);
                context.AddSource($"{validInterfaceInfo.InterfaceNamespace}.{validInterfaceInfo.InterfaceName}",
                    SourceText.From(sourceCode, Encoding.UTF8));
            }
        }

        public void Initialize(InitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }
    }
}