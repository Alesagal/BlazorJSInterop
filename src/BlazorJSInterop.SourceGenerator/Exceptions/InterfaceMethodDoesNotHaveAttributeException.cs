using System;
using BlazorJSInterop.SourceGenerator.Attributes;

namespace BlazorJSInterop.SourceGenerator.Exceptions
{
    internal class InterfaceMethodDoesNotHaveAttributeException : Exception
    {
        public InterfaceMethodDoesNotHaveAttributeException(string interfaceFullName) : base(
            $"A method in interface {interfaceFullName} does not have {nameof(BlazorJSInteropMethodAttribute)}")
        {
        }
    }
}