using System;

namespace BlazorJSInterop.SourceGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BlazorJSInteropMethodAttribute : Attribute
    {
        public string MethodName { get; }

        public BlazorJSInteropMethodAttribute(string methodName = null)
        {
            MethodName = methodName;
        }
    }
}