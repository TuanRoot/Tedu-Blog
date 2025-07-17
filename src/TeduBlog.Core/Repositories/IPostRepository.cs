using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduBlog.Core.Domain.Content;
using TeduBlog.Core.Models;
using TeduBlog.Core.Models.Content;
using TeduBlog.Core.SeedWorks;

namespace TeduBlog.Core.Repositories
{
    public interface IPostRepository : IRepository<Post, Guid>
    {
        Task<PageResult<PostInListDto>> GetAllPaging(string? keyword, Guid currentUserId, Guid? categoryId, int pageIndex = 1, int pageSize = 10);
        Task<List<Post>> GetPopularPostsAsync(int count);

        Task<PageResult<PostInListDto>> GetPostsPagingAsync(string keyword, Guid? categoryId, int pageIndex = 1, int pageSize = 10);
        Task<bool> IsSlugAlreadyExisted(string slug, Guid? currentId = null);

        Task<List<SeriesInListDto>> GetAllSeries(Guid postId);

        Task Approve(Guid id, Guid currentUserId);

        Task SendToApprove(Guid id, Guid currentUserId);
        Task ReturnBack(Guid id, Guid currentUserId, string note);

        Task<string> GetReturnReason(Guid id);

        Task<PageResult<PostInListDto>> GetPostByCategoryPaging(string categorySlug, int pageIndex = 1, int pageSize = 10);

        Task<bool> HasPublishInLast(Guid id);
        Task<List<PostActivityLogDto>> GetActivityLog(Guid userId);

        Task<List<Post>> GetListUnpaidPublishPosts(Guid userId);

        Task<List<PostInListDto>> GetLastesPublishPosts(int top);

        Task<PostDto> GetBySlug(string slug);

        Task<List<string>> GetAllTags();

        Task AddTagToPost(Guid postId, Guid tagId);
        Task<List<string>> GetTagsByPostId(Guid postId);

        Task<List<TagDto>> GetTagObjectsByPostId(Guid postId);

        Task<PageResult<PostInListDto>> GetPostByTagPaging(string tagSlug, int pageIndex = 1, int pageSize = 10);
        Task<PageResult<PostInListDto>> GetPostByUserPaging(string keyword, Guid userId, int pageIndex = 1, int pageSize = 10);
    }
}
