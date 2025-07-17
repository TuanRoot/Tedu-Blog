using TeduBlog.Core.Models;
using TeduBlog.Core.Models.Content;

namespace TeduBlog.WebApp.Models
{
    public class PostListByCategoryViewModel
    {
        public PostCategoryDto Category { get; set; }
        public PageResult<PostInListDto> Posts { get; set; }
    }
}
