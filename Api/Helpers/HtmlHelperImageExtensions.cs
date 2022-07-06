using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Api.Helpers
{
    public static class HtmlHelperImageExtensions
    {
        public static IHtmlContent NorthwindImageLink(this IHtmlHelper helper, int id, string text)
        {
            return new HtmlString($"<a href=\"/Images/{id}\">{text}</a>");
        }
    }
}
