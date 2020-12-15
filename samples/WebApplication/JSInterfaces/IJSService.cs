using System.Threading.Tasks;
using BlazorJSInterop.SourceGenerator.Attributes;

namespace WebApplication.JSInterfaces
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