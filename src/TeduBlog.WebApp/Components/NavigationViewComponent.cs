using Microsoft.AspNetCore.Mvc;
using TeduBlog.Core.Repositories;
using TeduBlog.Core.SeedWorks;
using TeduBlog.WebApp.Models;

namespace TeduBlog.WebApp.Components
{
    public class NavigationViewComponent : ViewComponent
    {

        private readonly IUnitOfWork _unitOfWork;
        public NavigationViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await _unitOfWork.PostCategories.GetAllAsync();

            var navItems = model.Select(x => new NavigationItemViewModel()
            {
                Slug = x.Slug,
                Name = x.Name,
                Children = model.Where(y => y.ParentId == x.Id).Select(y => new NavigationItemViewModel()
                {
                    Slug = y.Slug,
                    Name = y.Name
                }).ToList()
            }).ToList();
            // You can pass any model to the view if needed
            return await Task.FromResult(View(navItems));
        }
    }
}
