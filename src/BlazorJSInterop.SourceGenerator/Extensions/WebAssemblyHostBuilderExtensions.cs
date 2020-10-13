using System.Reflection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorJSInterop.SourceGenerator.Extensions
{
    public static class WebAssemblyHostBuilderExtensions
    {
        public static void AddJSInteropImplementations(this WebAssemblyHostBuilder builder, Assembly assembly)
        {
            var dependencyInjector = new DependencyInjector();
            dependencyInjector.RegisterIntoDependencyInjection(builder, assembly);
        }
    }
}