using System;
using System.Threading.Tasks;

namespace BlazorJSInterop.SourceGenerator.Exceptions
{
    internal class InterfaceMethodDoesNotHaveValidReturnType : Exception
    {
        public InterfaceMethodDoesNotHaveValidReturnType(string interfaceFullName, string methodName) : base(
            $"Method{methodName} in interface {interfaceFullName} does not have valid return type. It should be {nameof(Task)} or {nameof(Task<object>)}")
        {
        }
    }
}