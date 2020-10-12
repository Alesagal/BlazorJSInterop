using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace BlazorJSInterop.SourceGenerator.Diagnostics
{
    internal class DiagnosticReporter
    {
        private readonly SourceGeneratorContext _context;

        internal DiagnosticReporter(SourceGeneratorContext context)
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
            _context.ReportDiagnostic(Diagnostic.Create(
                "CSBLJS0001",
                "Usage",
                string.Format(diagnosticType.GetErrorMessage(), methodName, interfaceFullName, issuedTypeName),
                DiagnosticSeverity.Error,
                DiagnosticSeverity.Error,
                true,
                0,
                location: locations[0]
            ));
        }
    }
}