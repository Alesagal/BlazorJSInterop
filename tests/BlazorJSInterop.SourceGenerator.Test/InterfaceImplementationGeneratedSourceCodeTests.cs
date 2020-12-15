using BlazorJSInterop.SourceGenerator.Test.Configuration;
using Shouldly;
using Xunit;

namespace BlazorJSInterop.SourceGenerator.Test
{
    public class InterfaceImplementationGeneratedSourceCodeTests
    {
        [Fact]
        public void ShouldCreateValidInterface()
        {
            var source = TestData.ValidSource;

            var generatedSourceCodeText = GeneratorTestFactory.GetGeneratedSourceCodeText(source);

            generatedSourceCodeText.ShouldBe(TestData.ValidGeneratedSourceCodeText);
        }
    }
}