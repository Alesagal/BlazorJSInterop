using System.Linq;
using BlazorJSInterop.SourceGenerator.Diagnostics;
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

            var generatorDiagnostics = GeneratorTestFactory.GetDiagnostics(source);

            generatorDiagnostics.Any(x => x.Severity == DiagnosticSeverity.Error).ShouldBeFalse();
        }

        [Fact]
        public void ShouldFailGeneratorWithCode_CSBLJS0001()
        {
            var source = TestData.InvalidSourceMissingMethodAttribute;

            var generatorDiagnostics = GeneratorTestFactory.GetDiagnostics(source);

            var (expectedErrorCode, _) = DiagnosticTypesTuples.MethodWithoutAttributeErrorTuple;

            generatorDiagnostics.Any(diagnostic =>
            {
                return diagnostic.Id == expectedErrorCode &&
                       diagnostic.GetMessage() ==
                       "Method 'ShowHelloWorldAlert' in interface 'BlazorJSInterop.SourceGenerator.Test.TestData.IJSService' does not have attribute 'BlazorJSInterop.SourceGenerator.Attributes.BlazorJSInteropMethodAttribute'";
            }).ShouldBeTrue();
        }

        [Fact]
        public void ShouldFailGeneratorWithCode_CSBLJS0002_BecauseTask()
        {
            var source = TestData.InvalidSourceWrongReturnType;

            var generatorDiagnostics = GeneratorTestFactory.GetDiagnostics(source);

            var (expectedErrorCode, _) = DiagnosticTypesTuples.MethodInvalidReturnTypeErrorTuple;

            generatorDiagnostics.Any(diagnostic =>
            {
                return diagnostic.Id == expectedErrorCode &&
                       diagnostic.GetMessage() ==
                       "Method 'ShowHelloWorldAlert' in interface 'BlazorJSInterop.SourceGenerator.Test.TestData.IJSService' does not have valid return type. It should be 'System.Threading.Tasks.ValueTask'";
            }).ShouldBeTrue();
        }

        [Fact]
        public void ShouldFailGeneratorWithCode_CSBLJS0002_BecauseTaskWithGenericType()
        {
            var source = TestData.InvalidSourceWrongReturnTypeT;

            var generatorDiagnostics = GeneratorTestFactory.GetDiagnostics(source);

            var (expectedErrorCode, _) = DiagnosticTypesTuples.MethodInvalidReturnTypeErrorTuple;

            generatorDiagnostics.Any(diagnostic =>
            {
                return diagnostic.Id == expectedErrorCode &&
                       diagnostic.GetMessage() ==
                       "Method 'ShowNamePrompt' in interface 'BlazorJSInterop.SourceGenerator.Test.TestData.IJSService' does not have valid return type. It should be 'System.Threading.Tasks.ValueTask'";
            }).ShouldBeTrue();
        }
    }
}