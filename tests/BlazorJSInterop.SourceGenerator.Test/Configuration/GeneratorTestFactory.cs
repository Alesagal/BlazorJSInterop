using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using BlazorJSInterop.SourceGenerator.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace BlazorJSInterop.SourceGenerator.Test.Configuration
{
    // Based on: https://github.com/codecentric/net_automatic_interface/blob/master/AutomaticInterface/Tests/GeneratorFactory.cs
    internal class GeneratorTestFactory
    {
        private static SyntaxTree GetSyntaxTree(string source) => CSharpSyntaxTree.ParseText(SourceText.From(source, Encoding.UTF8));

        private static Compilation GetCompilation(string source)
        {
            var syntaxTree = GetSyntaxTree(source);

            var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Debug)
                .WithGeneralDiagnosticOption(ReportDiagnostic.Default);

            var references = new MetadataReference[]
            {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(BlazorJSInteropSourceAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.1.0.0").Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime, Version=4.2.2.0").Location)
            };

            var compilation = CSharpCompilation.Create("testgenerator", new[] { syntaxTree }, references, compilationOptions)
                .WithReferences(references);

            return compilation;
        }

        internal static ImmutableArray<Diagnostic> RunGenerator(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(SourceText.From(source, Encoding.UTF8));
            var compilation = GetCompilation(source);

            var generator = new InterfaceImplementationGenerator();
            var parseOptions = (CSharpParseOptions) syntaxTree.Options;

            GeneratorDriver driver = new CSharpGeneratorDriver(
                parseOptions,
                ImmutableArray.Create<ISourceGenerator>(generator),
                null,
                ImmutableArray<AdditionalText>.Empty);

            driver.RunFullGeneration(compilation, out var outputCompilation, out var generatorDiagnostics);

            return generatorDiagnostics;
        }
    }
}