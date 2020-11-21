using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace BlazorJSInterop.SourceGenerator.Diagnostics
{
    internal class DiagnosticReporter
    {
        public bool HasReported { get; private set; } = false;

        private readonly GeneratorExecutionContext _context;

        internal DiagnosticReporter(GeneratorExecutionContext context)
        {
            _context = context;
        }

        internal void ReportMethodDiagnostic(
            DiagnosticType diagnosticType,
            string methodName,
            string interfaceFullName,
            string issuedTypeName,
            ImmutableArray<Location> locations)
        {
            HasReported = true;
            var (code, message) = diagnosticType.GetErrorCodeMessageTuple();
            _context.ReportDiagnostic(Diagnostic.Create(
                code,
                "Usage",
                string.Format(message, methodName, interfaceFullName, issuedTypeName),
                DiagnosticSeverity.Error,
                DiagnosticSeverity.Error,
                true,
                0,
                location: locations[0]
            ));
        }
    }
}