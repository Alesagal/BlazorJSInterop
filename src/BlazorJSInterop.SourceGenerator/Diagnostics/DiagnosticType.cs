using System;

namespace BlazorJSInterop.SourceGenerator.Diagnostics
{
    internal enum DiagnosticType
    {
        MethodWithoutAttribute,
        MethodInvalidReturnType
    }

    internal static class DiagnosticTypeExtensions
    {
        internal static string GetErrorMessage(this DiagnosticType diagnosticType)
        {
            return diagnosticType switch
            {
                DiagnosticType.MethodWithoutAttribute => "Method '{0}' in interface '{1}' does not have attribute '{2}'",
                DiagnosticType.MethodInvalidReturnType => "Method '{0}' in interface '{1}' does not have valid return type. It should be '{2}'",
                _ => throw new ArgumentOutOfRangeException(nameof(diagnosticType), diagnosticType, null)
            };
        }
    }
}