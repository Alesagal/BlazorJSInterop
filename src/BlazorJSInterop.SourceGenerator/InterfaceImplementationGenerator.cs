using System.Runtime.CompilerServices;
using System.Text;
using BlazorJSInterop.SourceGenerator.Attributes;
using BlazorJSInterop.SourceGenerator.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

[assembly: InternalsVisibleTo("BlazorJSInterop.SourceGenerator.Test")]
namespace BlazorJSInterop.SourceGenerator
{
    [Generator]
    internal class InterfaceImplementationGenerator : ISourceGenerator
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

            var candidateInterfacesProcessor = new CandidateInterfacesProcessor(
                context.Compilation,
                syntaxReceiver.CandidateInterfaces,
                interfaceAttributeSymbol,
                methodAttributeSymbol,
                diagnosticReporter);

            var validInterfaceInfoList = candidateInterfacesProcessor.GetValidInterfaceInfoList();

            diagnosticReporter.ThrowIfReported();

            var sourceCodeBuilder = new SourceCodeBuilder(methodAttributeSymbol);

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