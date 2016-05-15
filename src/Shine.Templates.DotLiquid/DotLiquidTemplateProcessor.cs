using System;
using System.Linq;
using System.Reflection;
using DotLiquid;
using DotLiquid.FileSystems;
using DotLiquid.NamingConventions;
using Shine.Templating;

namespace Shine.Templates.DotLiquid
{
    public class DotLiquidTemplateProcessor : ITemplateProcessor
    {
        public DotLiquidTemplateProcessor(string templatesPath)
        {
            RegisterTag<Tags.StaticTag>("static");
            Template.FileSystem = new DebugFileSystem(templatesPath);
            Template.NamingConvention = new CSharpNamingConvention();
            RegisterFilters(typeof(Filters));
        }

        public DotLiquidTemplateProcessor(Assembly assembly, string root)
        {
            RegisterTag<Tags.StaticTag>("static");
            Template.FileSystem = new EmbeddedFileSystem(assembly, root);
            Template.NamingConvention = new CSharpNamingConvention();
            RegisterFilters(typeof(Filters));
        }

        public string Render(string templatePath, dynamic context)
        {
            var tpl = Template.Parse(LoadTemplate(templatePath));
            return tpl.Render(Hash.FromAnonymousObject(context));
        }

        public void RegisterTag<T>(string name)
            where T : Tag, new()
        {
            Template.RegisterTag<T>(name);
        }

        public void RegisterFilters(Type t)
        {
            Template.RegisterFilter(t);
        }

        public void RegisterSafeType<T>()
        {
            var allowedProperties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(p => p.Name).ToArray();
            Template.RegisterSafeType(typeof(T), allowedProperties);
        }

        public void RegisterSafeType<T>(string[] allowedProperties)
        {
            Template.RegisterSafeType(typeof(T), allowedProperties);
        }
        
        private static string LoadTemplate(string templatePath)
        {
            const string tmpKey = "EntryPointTemplate";
            var ctx = new global::DotLiquid.Context { [tmpKey] = templatePath };
            return Template.FileSystem.ReadTemplateFile(ctx, tmpKey);
        }
    }
}