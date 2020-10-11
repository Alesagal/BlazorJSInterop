using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace BlazorJSInterop.SourceGenerator
{
    internal class ValidInterfaceInfo
    {
        internal ValidInterfaceInfo(string interfaceName, string interfaceNamespace, List<IMethodSymbol> methodSymbols)
        {
            InterfaceName = interfaceName;
            InterfaceNamespace = interfaceNamespace;
            MethodSymbols = methodSymbols;
        }

        internal string InterfaceName { get; }

        internal string InterfaceNamespace { get; }

        internal List<IMethodSymbol> MethodSymbols { get; }
    }
}