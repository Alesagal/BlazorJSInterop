using System.Linq;
using System.Text;
using BlazorJSInterop.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;

namespace BlazorJSInterop.SourceGenerator
{
    internal class SourceCodeBuilder
    {
        private const string ClassTemplate = @"using Microsoft.JSInterop;
using System.Threading.Tasks;
using BlazorJSInterop.SourceGenerator.Attributes;

namespace {0}
{{
    [GeneratedImplementation(typeof({2}))]
    public class {1} : {2}
    {{
        private readonly IJSRuntime _jsRuntime;

        public {1}(IJSRuntime jsRuntime)
        {{
            _jsRuntime = jsRuntime;
        }}{3}
    }}
}}";

        private const string MethodWithNoParamsAndNoReturnTemplate = @"

        public ValueTask {0}()
        {{
            return _jsRuntime.InvokeVoidAsync(""{1}"");
        }}";

        private const string MethodWithParamsAndNoReturnTemplate = @"

        public ValueTask {0}({1})
        {{
            return _jsRuntime.InvokeVoidAsync(""{2}"", {3});
        }}";


        private const string MethodWithNoParamsAndReturnTemplate = @"

        public ValueTask<0> {1}()
        {{
            return _jsRuntime.InvokeAsync<{0}>(""{2}"");
        }}";

        private const string MethodWithParamsAndReturnTemplate = @"

        public ValueTask<{0}> {1}({2})
        {{
            return _jsRuntime.InvokeAsync<{0}>(""{3}"", {4});
        }}";

        private readonly INamedTypeSymbol _methodAttributeSymbol;

        internal SourceCodeBuilder(INamedTypeSymbol methodAttributeSymbol)
        {
            _methodAttributeSymbol = methodAttributeSymbol;
        }

        internal string BuildSourceCode(ValidInterfaceInfo validInterfaceInfo)
        {
            var methodsCodeStringBuilder = new StringBuilder();

            var interfaceNamespace = $"{validInterfaceInfo.InterfaceNamespace}.Generated";
            var interfaceFullName = $"{validInterfaceInfo.InterfaceNamespace}.{validInterfaceInfo.InterfaceName}";
            var className = string.Concat(validInterfaceInfo.InterfaceName, "__Generated");

            foreach (var methodSymbol in validInterfaceInfo.MethodSymbols)
            {
                var jsMethodName = GetJavaScriptFunctionName(methodSymbol);

                var parametersTypeNameTuples = methodSymbol.Parameters
                    .Select(p => ($"{p.Type.ContainingNamespace}.{p.Type.Name}", p.Name))
                    .ToList();

                if (methodSymbol.ReturnType.IsVoidValueTaskType())
                {
                    if (parametersTypeNameTuples.Any())
                        methodsCodeStringBuilder.AppendFormat(
                            MethodWithParamsAndNoReturnTemplate,
                            methodSymbol.Name,
                            string.Join(", ", parametersTypeNameTuples.Select(p => $"{p.Item1} {p.Item2}")),
                            jsMethodName,
                            string.Join(", ", parametersTypeNameTuples.Select(p => p.Item2)));
                    else
                        methodsCodeStringBuilder.AppendFormat(
                            MethodWithNoParamsAndNoReturnTemplate,
                            methodSymbol.Name,
                            jsMethodName);
                }
                else
                {
                    var returnTypeFullName = methodSymbol.ReturnType.GetValueTaskGenericReturnTypeFullName();

                    if (parametersTypeNameTuples.Any())
                        methodsCodeStringBuilder.AppendFormat(
                            MethodWithParamsAndReturnTemplate,
                            returnTypeFullName,
                            methodSymbol.Name,
                            string.Join(", ", parametersTypeNameTuples.Select(p => $"{p.Item1} {p.Item2}")),
                            jsMethodName,
                            string.Join(", ", parametersTypeNameTuples.Select(p => p.Item2)));
                    else
                        methodsCodeStringBuilder.AppendFormat(
                            MethodWithNoParamsAndReturnTemplate,
                            returnTypeFullName,
                            methodSymbol.Name,
                            jsMethodName);
                }
            }

            return string.Format(ClassTemplate, interfaceNamespace, className, interfaceFullName, methodsCodeStringBuilder);
        }

        private string GetJavaScriptFunctionName(IMethodSymbol methodSymbol)
        {
            var attributeData = methodSymbol.GetAttributes().Single(ad =>
                ad.AttributeClass!.Equals(_methodAttributeSymbol, SymbolEqualityComparer.Default));

            var functionName = attributeData.ConstructorArguments[0].Value?.ToString();
            return functionName;
        }
    }
}