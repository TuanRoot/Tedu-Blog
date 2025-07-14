using Microsoft.AspNetCore.Mvc;

namespace TeduBlog.WebApp.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            // You can pass any model to the view if needed
            return await Task.FromResult(View("Default"));
        }
    }
}
