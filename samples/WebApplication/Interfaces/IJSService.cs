using System.Threading.Tasks;
using BlazorJSInterop.SourceGenerator.Attributes;

namespace WebApplication.Interfaces
{
    [BlazorJSInteropSource]
    public interface IJSService
    {
        [BlazorJSInteropMethod("showHelloWorldAlert")]
        ValueTask ShowHelloWorldAlert();

        [BlazorJSInteropMethod("showNamePrompt")]
        ValueTask<string> ShowNamePrompt(string title);

        [BlazorJSInteropMethod("showNameAlert")]
        ValueTask ShowNameAlert(string name);
    }
}