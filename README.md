# BlazorJSInterop
This is a Blazor library that uses .NET source generators in order to create implementations of JS Service files and include them into the dependency injection collection.

## Using the library
1. Include the source generator as a dependency into your Blazor project file.

```text
dotnet add package BlazorJSInterop.SourceGenerator --version 6.0.6
```

2. Prototype your service interfaces using the ```BlazorJSInteropSource``` attribute in the interface and the ```BlazorJSInteropMethod``` attributes in all its methods.
All methods in the interfaces must return ```ValueTask``` or ```ValueTask<T>``` and all the method attributes must include the JS name function as a parameter.

For example:
```c#
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
```

3. In your ```Program.cs``` file include the following using statement:
```c#
using BlazorJSInterop.SourceGenerator.Extensions;
```

4. In your Main method in ```Program.cs``` include the following line ```builder.AddJSInteropImplementations(Assembly.GetExecutingAssembly());``` such as:
```c#
public static async Task Main(string[] args)
{
    var builder = WebAssemblyHostBuilder.CreateDefault(args);

    // Add this line to register the generated code into the dependency injection collection.
    builder.AddJSInteropImplementations(Assembly.GetExecutingAssembly());

    builder.RootComponents.Add<App>("app");

    builder.Services.AddTransient(sp => new HttpClient
        {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

    await builder.Build().RunAsync();
}
```

5. Inject your interface into any of your files as you would do with any other service.

## Testing the library locally as a project reference
1. If you want to test this project locally as a project reference, it is important to keep ```ReferenceOutputAssembly``` to true as there are some dependencies of this assembly that will be needed in your project.
```xml
<ProjectReference Include="..\..\src\BlazorJSInterop.SourceGenerator\BlazorJSInterop.SourceGenerator.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="true" />
```
