using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Api.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "northwind-id")]
    public class NorthwindImageTagHelper : TagHelper
    {
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (int.TryParse(context.AllAttributes["northwind-id"].Value.ToString(), out int id))
            {
                output.Attributes.Clear();
                output.Attributes.Add("href", $"/images/{id}");
            }

            return base.ProcessAsync(context, output);
        }
    }
}
