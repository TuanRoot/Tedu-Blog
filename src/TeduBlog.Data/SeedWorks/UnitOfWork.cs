using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduBlog.Core.Domain.Identity;
using TeduBlog.Core.Repositories;
using TeduBlog.Core.SeedWorks;
using TeduBlog.Data.Repositories;
using static TeduBlog.Core.SeedWorks.Constants.Permissions;

namespace TeduBlog.Data.SeedWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TeduBlogContext _context;
        public UnitOfWork(TeduBlogContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            Posts = new PostRepository(context, mapper, userManager);
            PostCategories = new PostCategoryRepository(context, mapper);
        }

        public IPostRepository Posts { get; private set; }

        public IPostCategoryRepository PostCategories { get; private set; }

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
