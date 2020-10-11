using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlazorJSInterop.SourceGenerator.Attributes;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorJSInterop.SourceGenerator
{
    internal class DependencyInjector
    {
        internal void CreateImplementations(WebAssemblyHostBuilder builder, Assembly assembly)
        {
            var interfaceImplementationDictionary = GetInterfaceImplementationDictionary(assembly);
            foreach (var (interfaceType, generatedType) in interfaceImplementationDictionary)
            {
                builder.Services.AddTransient(interfaceType, generatedType);
            }
        }

        private Dictionary<Type, Type> GetInterfaceImplementationDictionary(Assembly assembly)
        {
            var interfaceImplementationDictionary = new Dictionary<Type, Type>();

            var generatedTypes = assembly.GetTypes()
                .Where(type => type.GetCustomAttributes(typeof(GeneratedImplementationAttribute), true).Any())
                .ToArray();

            foreach (var generatedType in generatedTypes)
            {
                var generatedImplementationAttribute =
                    (GeneratedImplementationAttribute) Attribute.GetCustomAttribute(generatedType,
                        typeof(GeneratedImplementationAttribute));
                var interfaceType = generatedImplementationAttribute.InterfaceType;
                interfaceImplementationDictionary.Add(interfaceType, generatedType);
            }

            return interfaceImplementationDictionary;
        }
    }
}