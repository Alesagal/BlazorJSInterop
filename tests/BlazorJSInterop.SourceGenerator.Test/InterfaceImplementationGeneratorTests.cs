using System.Linq;
using BlazorJSInterop.SourceGenerator.Test.Configuration;
using Microsoft.CodeAnalysis;
using Shouldly;
using Xunit;

namespace BlazorJSInterop.SourceGenerator.Test
{
    public class InterfaceImplementationGeneratorTests
    {
        [Fact]
        public void ShouldCreateValidInterface()
        {
            var source = TestData.ValidSource;

            var generatorDiagnostics = GeneratorTestFactory.RunGenerator(source);

            generatorDiagnostics.Any(x => x.Severity == DiagnosticSeverity.Error).ShouldBeFalse();
        }

        [Fact]
        public void ShouldFailGeneratorWithCode_CSBLJS0001()
        {
            var source = TestData.InvalidSourceMissingMethodAttribute;

            var generatorDiagnostics = GeneratorTestFactory.RunGenerator(source);

            generatorDiagnostics.Any(diagnostic =>
            {
                return diagnostic.Id == "CSBLJS0001" &&
                       diagnostic.GetMessage() ==
                       "Method 'ShowHelloWorldAlert' in interface 'BlazorJSInterop.SourceGenerator.Test.TestData.IJSService' does not have attribute 'BlazorJSInterop.SourceGenerator.Attributes.BlazorJSInteropMethodAttribute'";
            }).ShouldBeTrue();generatorDiagnostics.Any(diagnostic => diagnostic.Id == "CSBLJS0001").ShouldBeTrue();
        }

        [Fact]
        public void ShouldFailGeneratorWithCode_CSBLJS0002_BecauseTask()
        {
            var source = TestData.InvalidSourceWrongReturnType;

            var generatorDiagnostics = GeneratorTestFactory.RunGenerator(source);

            generatorDiagnostics.Any(diagnostic =>
            {
                return diagnostic.Id == "CSBLJS0002" &&
                       diagnostic.GetMessage() ==
                       "Method 'ShowHelloWorldAlert' in interface 'BlazorJSInterop.SourceGenerator.Test.TestData.IJSService' does not have valid return type. It should be 'System.Threading.Tasks.ValueTask'";
            }).ShouldBeTrue();
        }

        [Fact]
        public void ShouldFailGeneratorWithCode_CSBLJS0002_BecauseTaskT()
        {
            var source = TestData.InvalidSourceWrongReturnTypeT;

            var generatorDiagnostics = GeneratorTestFactory.RunGenerator(source);

            generatorDiagnostics.Any(diagnostic =>
            {
                return diagnostic.Id == "CSBLJS0002" &&
                       diagnostic.GetMessage() ==
                       "Method 'ShowNamePrompt' in interface 'BlazorJSInterop.SourceGenerator.Test.TestData.IJSService' does not have valid return type. It should be 'System.Threading.Tasks.ValueTask'";
            }).ShouldBeTrue();
        }
    }
}