using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TeduBlog.Core.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TeduBlog.WebApp.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(PageResultBase result)
        {
            return await Task.FromResult(View(result));
        }
    }
}
