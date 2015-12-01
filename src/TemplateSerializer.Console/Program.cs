using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NServiceBus.ServiceIgnition;

namespace TemplateSerializer.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("Missing base path for TemplateSerializer.Console.");
            }

            var basePath = args[0];

            var busConfigurationPath = basePath + @"MethodTemplates\";
            var classTemplatePath = basePath + @"ClassDefinitions\";

            SerializeBusMethods(busConfigurationPath);
            SerializeClassTemplates(classTemplatePath);
        }

        static void SerializeClassTemplates(string basePath)
        {
            var templateDictionary = new Dictionary<string, string>();

            Action<string> addClassTemplate = csharpText =>
            {
                var className = GetClassName(csharpText);
                templateDictionary.Add(className, csharpText);
            };

            ActOnCSharpFiles(basePath, addClassTemplate);

            var textToSave = CreateDictionaryClass(ClassDefinitionClassName, templateDictionary);

            var fileSavePath = basePath + "_" + ClassDefinitionClassName + ".cs";

            File.WriteAllText(fileSavePath, textToSave);
        }

        private static string GetClassName(string csharpText)
        {
            return Regex.Match(csharpText, @" class ([^\s]+)").Groups[1].Value;
        }

        const string BusMethodClassName = "BusMethodTemplates";
        const string ClassDefinitionClassName = "ClassDefinitionTemplates";

        static void SerializeBusMethods(string basePath)
        {
            var templateMetadata = new List<TemplateMethodMetadata>();

            Action<string> addMetadataToFinalList = csharpText =>
            {
                var classMetadata = GrabTemplateMetadata(csharpText);
                templateMetadata.AddRange(classMetadata);
            };

            ActOnCSharpFiles(basePath, addMetadataToFinalList);

            var metadataDictionary =
                templateMetadata
                .ToDictionary(i => i.ClassName + "." + i.MethodName, i => i.MethodBody);

            var textToSave = CreateDictionaryClass(BusMethodClassName, metadataDictionary);

            var fileSavePath = basePath + "_" + BusMethodClassName + ".cs";

            File.WriteAllText(fileSavePath, textToSave);
        }

        private static List<TemplateMethodMetadata> GrabTemplateMetadata(string csharpText)
        {
            var className = GetClassName(csharpText);

            var methodPattern = new Regex(@"public static void ([^\(]+)\([^\)]*\)[^\{]*\{([^\}]*)\}");

            var methodCaptureGroups = methodPattern.Matches(csharpText);

            var metadata = new List<TemplateMethodMetadata>();

            foreach (Match match in methodCaptureGroups)
            {
                var methodName = match.Groups[1].Value;
                var methodBody = match.Groups[2].Value;

                methodBody = ReplaceFirstOccurrence(methodBody, Environment.NewLine, "");
                methodBody = ReplaceLastOccurrence(methodBody, Environment.NewLine, "");
                methodBody = methodBody.Trim();

                metadata.Add(new TemplateMethodMetadata()
                {
                    ClassName = className,
                    MethodBody = methodBody,
                    MethodName = methodName,
                });
            }

            return metadata;
        }

        private static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
        {
            var place = Source.IndexOf(Find);
            var result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }

        private static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            var place = Source.LastIndexOf(Find);
            var result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }

        private static bool ShouldExcludeFile(string fileName)
        {
            return
                !fileName.EndsWith(".cs")
                || fileName.Contains(BusMethodClassName)
                || fileName.Contains(ClassDefinitionClassName);
        }

        private static void ActOnCSharpFiles(string folderPath, Action<string> action)
        {
            var csharpFiles = Directory.GetFiles(folderPath).Where(f => !ShouldExcludeFile(f));
            var folders = Directory.GetDirectories(folderPath);

            foreach (var filePath in csharpFiles)
            {
                var file = File.ReadAllText(filePath);
                action(file);
            }

            foreach (var child in folders)
            {
                ActOnCSharpFiles(child, action);
            }
        }

        static string CreateDictionaryClass(string name, Dictionary<string, string> templateDictionary)
        {
            var entry = @"        { ""{{key}}"", @""{{value}}"" },";

            var entries =
                templateDictionary
                    .Select(kvp => entry.Replace("{{key}}", kvp.Key).Replace("{{value}}", kvp.Value.Replace("\"", "\"\"").Replace("//# ", "")));

            var classDefinition =
                templateDictionaryClass
                    .Replace("TemplateDictionaryClass", name)
                    .Replace("//# {{templates}}", string.Join(Environment.NewLine, entries));

            return classDefinition;
        }

        const string templateDictionaryClass = @"// This file is auto-generated, any changes you make to this file will be overwritten

using System.Collections.Generic;

public static class TemplateDictionaryClass
{
    public static Dictionary<string, string> Dictionary = new Dictionary<string, string>()
    {
//# {{templates}}
    }; 
}";
    }
}
