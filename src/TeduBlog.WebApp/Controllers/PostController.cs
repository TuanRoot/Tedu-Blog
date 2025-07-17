using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using TeduBlog.Core.Models.Content;
using TeduBlog.Core.SeedWorks;
using TeduBlog.WebApp.Models;

namespace TeduBlog.WebApp.Controllers
{
    public class PostController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PostController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [Route("posts")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("posts/{categorySlug}")]
        public async Task<IActionResult> ListByCategory([FromRoute] string categorySlug, [FromQuery] int page = 1)
        {
            var posts = await _unitOfWork.Posts.GetPostByCategoryPaging(categorySlug, page, 10);
            var categories = await _unitOfWork.PostCategories.GetBySlug(categorySlug);
            return View( new PostListByCategoryViewModel()
            {
                Category = categories,
                Posts =  posts
            });
        }

        [Route("tag/{tagSlug}")]
        public IActionResult ListByTag([FromRoute] string tagSlug, [FromQuery] int? page = 1)
        {
            return View();
        }

        [Route("post/{slug}")]
        public async Task<IActionResult> Detail([FromRoute] string slug)
        {
            var post = await _unitOfWork.Posts.GetBySlug(slug);
            var category = await _unitOfWork.PostCategories.GetBySlug(post.CategorySlug);
            return View(new PostDetailViewModel()
            {
                Post = post,
                Category = category
            });

        }
    }
}
