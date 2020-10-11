using System.Threading.Tasks;
using BlazorJSInterop.SourceGenerator.Attributes;

namespace WebApplication.Interfaces
{
    [BlazorJSInteropSource]
    public interface IValidInterface
    {
        [BlazorJSInteropMethod("invokeAsync")]
        ValueTask<TestObject> InvokeAsync(TestObject testObject);

        [BlazorJSInteropMethod("invokeVoidAsync")]
        ValueTask InvokeVoidAsync(TestObject testObject);

        [BlazorJSInteropMethod] // Method name will default to "empty".
        ValueTask Empty(string s);
    }
}