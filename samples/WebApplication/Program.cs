using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using BlazorJSInterop.SourceGenerator.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.AddJSInteropImplementations(Assembly.GetExecutingAssembly());
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient
                {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

            await builder.Build().RunAsync();
        }
    }
}