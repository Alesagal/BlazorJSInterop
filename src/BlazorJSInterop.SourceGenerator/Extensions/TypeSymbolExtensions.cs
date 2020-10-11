using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace BlazorJSInterop.SourceGenerator.Extensions
{
    internal static class TypeSymbolExtensions
    {
        private static readonly string ValueTaskFullName = typeof(ValueTask).FullName;

        internal static bool IsTaskType(this ITypeSymbol typeSymbol) =>
            $"{typeSymbol.ContainingNamespace}.{typeSymbol.Name}" == ValueTaskFullName;

        internal static bool IsVoidValueTaskType(this ITypeSymbol typeSymbol) => typeSymbol.ToString() == ValueTaskFullName;

        internal static string GetValueTaskGenericReturnTypeFullName(this ITypeSymbol typeSymbol)
        {
            var typeFullName = typeSymbol.ToString();

            if (IsTaskType(typeSymbol) && !IsVoidValueTaskType(typeSymbol))
                return GetValueTaskGenericReturnTypeFullNameWithoutBraces(typeFullName);
            return null;
        }

        private static string GetValueTaskGenericReturnTypeFullNameWithoutBraces(string typeFullName) =>
            typeFullName.Substring(ValueTaskFullName.Length + 1, typeFullName.Length - ValueTaskFullName.Length - 2);
    }
}