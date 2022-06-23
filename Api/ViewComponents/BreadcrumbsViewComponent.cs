using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Api.ViewComponents
{
    public class BreadcrumbsViewComponent : ViewComponent
    {
        readonly Dictionary<string, string> breadcrumbs;

        public BreadcrumbsViewComponent()
        {
            breadcrumbs = new Dictionary<string, string>();
        }

        public IViewComponentResult Invoke(string path)
        {
            var values = path.Split("/", StringSplitOptions.RemoveEmptyEntries);
            var temp = "";
            for (int i = 0; i < values.Length; i++)
            {
                temp += $"/{values[i]}";
                breadcrumbs.TryAdd(temp, values[i]);
            }

            return View(breadcrumbs);
        }
    }
}
