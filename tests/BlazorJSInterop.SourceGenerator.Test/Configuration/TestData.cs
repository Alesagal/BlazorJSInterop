namespace BlazorJSInterop.SourceGenerator.Test.Configuration
{
    internal static class TestData
    {
        internal const string ValidSource = @"using System.Threading.Tasks;
using BlazorJSInterop.SourceGenerator.Attributes;

namespace BlazorJSInterop.SourceGenerator.Test.TestData
{
    [BlazorJSInteropSource]
    public interface IJSService
    {
        [BlazorJSInteropMethod(""showHelloWorldAlert"")]
        ValueTask ShowHelloWorldAlert();

        [BlazorJSInteropMethod(""showNamePrompt"")]
        ValueTask<string> ShowNamePrompt(string title);

        [BlazorJSInteropMethod(""showNameAlert"")]
        ValueTask ShowNameAlert(string name);
    }
}";

        internal const string InvalidSourceMissingMethodAttribute = @"using System.Threading.Tasks;
using BlazorJSInterop.SourceGenerator.Attributes;

namespace BlazorJSInterop.SourceGenerator.Test.TestData
{
    [BlazorJSInteropSource]
    public interface IJSService
    {
        ValueTask ShowHelloWorldAlert();

        [BlazorJSInteropMethod(""showNamePrompt"")]
        ValueTask<string> ShowNamePrompt(string title);

        [BlazorJSInteropMethod(""showNameAlert"")]
        ValueTask ShowNameAlert(string name);
    }
}";

        internal const string InvalidSourceWrongReturnType = @"using System.Threading.Tasks;
using BlazorJSInterop.SourceGenerator.Attributes;

namespace BlazorJSInterop.SourceGenerator.Test.TestData
{
    [BlazorJSInteropSource]
    public interface IJSService
    {
        [BlazorJSInteropMethod(""showHelloWorldAlert"")]
        Task ShowHelloWorldAlert();

        [BlazorJSInteropMethod(""showNamePrompt"")]
        ValueTask<string> ShowNamePrompt(string title);

        [BlazorJSInteropMethod(""showNameAlert"")]
        ValueTask ShowNameAlert(string name);
    }
}";

        internal const string InvalidSourceWrongReturnTypeT = @"using System.Threading.Tasks;
using BlazorJSInterop.SourceGenerator.Attributes;

namespace BlazorJSInterop.SourceGenerator.Test.TestData
{
    [BlazorJSInteropSource]
    public interface IJSService
    {
        [BlazorJSInteropMethod(""showHelloWorldAlert"")]
        Task ShowHelloWorldAlert();

        [BlazorJSInteropMethod(""showNamePrompt"")]
        Task<string> ShowNamePrompt(string title);

        [BlazorJSInteropMethod(""showNameAlert"")]
        ValueTask ShowNameAlert(string name);
    }
}";
    }
}