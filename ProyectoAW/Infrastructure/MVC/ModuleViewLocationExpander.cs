
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace ProyectoAW.Infraestructure.MVC
{
    public class ModuleViewLocationExpander : IViewLocationExpander
    {
        public void PopulateValues(ViewLocationExpanderContext context)
        {
            var controllerDescriptor =
                context.ActionContext.ActionDescriptor
                as ControllerActionDescriptor;

            var controllerNamespace =
                controllerDescriptor?.ControllerTypeInfo.Namespace;

            const string marker = ".Modules.";
            const string suffix = ".Controllers";

            if (string.IsNullOrEmpty(controllerNamespace))
                return;

            var start = controllerNamespace.IndexOf(marker);

            if (start < 0)
                return;

            start += marker.Length;

            var end = controllerNamespace.IndexOf(suffix, start);

            if (end <= start)
                return;

            var module = controllerNamespace.Substring(
                start,
                end - start
            ).Replace('.', '/');

            context.Values["viewModule"] = module;
        }

        public IEnumerable<string> ExpandViewLocations(
            ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue("viewModule", out var viewModule))
            {
                yield return $"/Modules/{viewModule}/Views/{{0}}.cshtml";
            }

            foreach (var location in viewLocations)
            {
                yield return location;
            }
        }
    }
}
