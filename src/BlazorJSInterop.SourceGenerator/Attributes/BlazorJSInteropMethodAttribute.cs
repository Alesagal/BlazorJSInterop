using System;

namespace BlazorJSInterop.SourceGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class BlazorJSInteropMethodAttribute : Attribute
    {
        public string FunctionName { get; }

        public BlazorJSInteropMethodAttribute(string functionName)
        {
            FunctionName = functionName;
        }
    }
}