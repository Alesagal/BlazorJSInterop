using System;

namespace BlazorJSInterop.SourceGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GeneratedImplementationAttribute : Attribute
    {
        public Type InterfaceType { get; }

        public GeneratedImplementationAttribute(Type interfaceType)
        {
            InterfaceType = interfaceType;
        }
    }
}