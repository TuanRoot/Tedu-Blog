using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using TeduBlog.Core.Domain.Content;
using TeduBlog.Core.Models;
using TeduBlog.Core.Models.Content;
using TeduBlog.Core.Repositories;
using TeduBlog.Data.SeedWorks;

namespace TeduBlog.Data.Repositories
{
    public class PostCategoryRepository : RepositoryBase<PostCategory, Guid>, IPostCategoryRepository
    {
        private readonly IMapper _mapper;

        public PostCategoryRepository(TeduBlogContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<PageResult<PostCategoryDto>> GetAllPaging(string? keyword, int pageIndex = 1, int pageSize = 10)
        {
            var query = _context.PostCategories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword) || x.Slug.Contains(keyword));
            }
            var totalRow = await query.CountAsync();
            query = query.OrderByDescending(x => x.DateCreated)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            return new PageResult<PostCategoryDto>
            {
                Results = await _mapper.ProjectTo<PostCategoryDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

        }

        public async Task<PostCategoryDto> GetBySlug(string slug)
        {
            var category = await _context.PostCategories.FirstOrDefaultAsync(x => x.Slug == slug);
            if (category == null)
            {
                throw new Exception($"Cannot find {slug}");
            }
            return _mapper.Map<PostCategoryDto>(category);

        }

        public async Task<bool> HasPost(Guid categoryId)
        {
            return await _context.Posts.AnyAsync(x => x.CategoryId == categoryId);
        }
    }
}
