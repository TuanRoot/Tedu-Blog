using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduBlog.Core.Domain.Content;
using TeduBlog.Core.Domain.Identity;
using TeduBlog.Core.Models;
using TeduBlog.Core.Models.Content;
using TeduBlog.Core.Repositories;
using TeduBlog.Core.SeedWorks;
using TeduBlog.Core.SeedWorks.Constants;
using TeduBlog.Data.SeedWorks;
using static TeduBlog.Core.SeedWorks.Constants.Permissions;

namespace TeduBlog.Data.Repositories
{
    public class PostRepository : RepositoryBase<Post, Guid>, IPostRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public PostRepository(TeduBlogContext context, IMapper mapper, UserManager<AppUser> userManager) : base(context)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task AddTagToPost(Guid postId, Guid tagId)
        {
            await _context.PostTags.AddAsync(new PostTag
            {
                PostId = postId,
                TagId = tagId
            });
        }

        public async Task Approve(Guid id, Guid currentUserId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if (post == null) { 
                throw new ArgumentException("Post not found", nameof(id));
            }
            var user = await _context.Users.FindAsync(currentUserId);
            await _context.PostActivityLogs.AddAsync(new PostActivityLog
            {
                Id = Guid.NewGuid(),
                FromStatus = post.Status,
                ToStatus = PostStatus.Published,
                UserId = currentUserId,
                UserName = user.UserName,
                PostId = id,
                Note = $"{user?.UserName} duyệt bài"
            });
            post.Status = PostStatus.Published;
            _context.Posts.Update(post);
        }

        public Task<List<PostActivityLogDto>> GetActivityLog(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<PostInListDto>> GetAllPaging(string? keyword, Guid currentUserId, Guid? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var user = await _userManager.FindByIdAsync(currentUserId.ToString());
            if (user == null)
            {
                throw new ArgumentException("User not found", nameof(currentUserId));
            }
            var roles = await _userManager.GetRolesAsync(user);
            var canApprove = false;

            if (roles.Contains(Role.Admin))
            {
                canApprove = true;
            }
            else
            {
                canApprove = await _context.RoleClaims.AnyAsync(x => roles.Contains(x.RoleId.ToString()) && x.ClaimValue == Permissions.Posts.Approve);                  
            }
            var query = _context.Posts.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }
            if (!canApprove)
            {
                query = query.Where(x => x.AuthorUserId == currentUserId);
            }
            var totalRow = await query.CountAsync();
            query = query.OrderByDescending(x => x.DateCreated)
              .Skip((pageIndex - 1) * pageSize)
              .Take(pageSize);

            return new PageResult<PostInListDto>
            {
                Results = await _mapper.ProjectTo<PostInListDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }

        public async Task<List<SeriesInListDto>> GetAllSeries(Guid postId)
        {
            var query = from pis in _context.PostInSeries
                        join s in _context.Series
                        on pis.SeriesId equals s.Id
                        where pis.PostId == postId
                        select s;
            return await _mapper.ProjectTo<SeriesInListDto>(query).ToListAsync();
        }

        public async Task<List<string>> GetAllTags()
        {
            var query = _context.Tags.Select(x => x.Name);
            return await query.ToListAsync();
        }

        public async Task<PostDto> GetBySlug(string slug)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Slug == slug);
            if (post == null)
            {
                throw new ArgumentException("Post not found", nameof(slug));
            }
            return _mapper.Map<PostDto>(post);
        }

        public async Task<List<PostInListDto>> GetLastesPublishPosts(int top)
        {
            var query =  _context.Posts.Where(x => x.Status == PostStatus.Published)
                .OrderByDescending(x => x.DateCreated)
                .Take(top);
            return await _mapper.ProjectTo<PostInListDto>(query).ToListAsync();
        }

        public Task<List<Post>> GetListUnpaidPublishPosts(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetPopularPostsAsync(int count)
        {
            return _context.Posts.OrderByDescending(x => x.ViewCount).Take(count).ToListAsync();
        }

        public async Task<PageResult<PostInListDto>> GetPostByCategoryPaging(string categorySlug, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.Posts.AsQueryable();

            if (!string.IsNullOrEmpty(categorySlug))
            {
                query = query.Where(x => x.CategorySlug == categorySlug);
            }

            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PageResult<PostInListDto>
            {
                Results = await _mapper.ProjectTo<PostInListDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

        }

        public Task<PageResult<PostInListDto>> GetPostByTagPaging(string tagSlug, int pageIndex = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public Task<PageResult<PostInListDto>> GetPostByUserPaging(string keyword, Guid userId, int pageIndex = 1, int pageSize = 10)
        {
            throw new NotImplementedException();
        }

        public async Task<PageResult<PostInListDto>> GetPostsPagingAsync(string keyword, Guid? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.Posts.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Content.Contains(keyword));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            var totalRow = await query.CountAsync();

            query = query
                .OrderByDescending(x => x.DateCreated)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return new PageResult<PostInListDto>
            {
                Results = await _mapper.ProjectTo<PostInListDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };
        }

        public Task<string> GetReturnReason(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TagDto>> GetTagObjectsByPostId(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetTagsByPostId(Guid postId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPublishInLast(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsSlugAlreadyExisted(string slug, Guid? currentId = null)
        {
            throw new NotImplementedException();
        }

        public Task ReturnBack(Guid id, Guid currentUserId, string note)
        {
            throw new NotImplementedException();
        }

        public Task SendToApprove(Guid id, Guid currentUserId)
        {
            throw new NotImplementedException();
        }
    }
}
