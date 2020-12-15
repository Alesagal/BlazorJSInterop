using System.Collections.Immutable;
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

        private static GeneratorDriverRunResult RunGenerator(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(SourceText.From(source, Encoding.UTF8));
            var compilation = GetCompilation(source);

            var generator = new InterfaceImplementationGenerator();
            var parseOptions = (CSharpParseOptions) syntaxTree.Options;

            GeneratorDriver driver = CSharpGeneratorDriver.Create(new[] {generator}, parseOptions: parseOptions);
            driver = driver.RunGenerators(compilation);

            return driver.GetRunResult();
        }

        internal static ImmutableArray<Diagnostic> GetDiagnostics(string source) => RunGenerator(source).Diagnostics;

        internal static string GetGeneratedSourceCodeText(string source) =>
            RunGenerator(source).Results[0].GeneratedSources[0].SourceText.ToString();
    }
}