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

        [BlazorJSInteropMethod("empty")]
        ValueTask Empty(string s);
    }
}