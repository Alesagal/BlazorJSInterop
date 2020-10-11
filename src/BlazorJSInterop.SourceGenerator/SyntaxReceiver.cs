using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlazorJSInterop.SourceGenerator
{
    internal class SyntaxReceiver : ISyntaxReceiver
    {
        internal List<InterfaceDeclarationSyntax> CandidateInterfaces { get; } = new List<InterfaceDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is InterfaceDeclarationSyntax interfaceDeclarationSyntax
                && interfaceDeclarationSyntax.AttributeLists.Count > 0)
            {
                CandidateInterfaces.Add(interfaceDeclarationSyntax);
            }
        }
    }
}