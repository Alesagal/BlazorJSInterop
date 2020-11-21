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

        internal const string ValidGeneratedSourceCodeText = @"using Microsoft.JSInterop;
using System.Threading.Tasks;
using BlazorJSInterop.SourceGenerator.Attributes;

namespace BlazorJSInterop.SourceGenerator.Test.TestData.Generated
{
    [GeneratedImplementation(typeof(BlazorJSInterop.SourceGenerator.Test.TestData.IJSService))]
    public class IJSService__Generated : BlazorJSInterop.SourceGenerator.Test.TestData.IJSService
    {
        private readonly IJSRuntime _jsRuntime;

        public IJSService__Generated(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public ValueTask ShowHelloWorldAlert()
        {
            return _jsRuntime.InvokeVoidAsync(""showHelloWorldAlert"");
        }

        public ValueTask<string> ShowNamePrompt(System.String title)
        {
            return _jsRuntime.InvokeAsync<string>(""showNamePrompt"", title);
        }

        public ValueTask ShowNameAlert(System.String name)
        {
            return _jsRuntime.InvokeVoidAsync(""showNameAlert"", name);
        }
    }
}";
    }
}