using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeduBlog.Core.Domain.Content;
using TeduBlog.Core.Models;
using TeduBlog.Core.Models.Content;
using TeduBlog.Core.SeedWorks;

namespace TeduBlog.Api.Controllers.AdminAPI
{
    /// <summary>
    /// [Route("api/[controller]")]
    /// </summary>
    [Route("api/admin/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PostController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreateUpdatePostRequest request)
        {
            var posts = _mapper.Map<CreateUpdatePostRequest, Post>(request);
            _unitOfWork.Posts.Add(posts);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0
                ? Ok(new { message = "Post created successfully." })
                : BadRequest(new { message = "Failed to create post." });
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] CreateUpdatePostRequest request)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound(new { message = "Post not found." });
            }
            _mapper.Map(request, post);
            //_unitOfWork.Posts.Update(post);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0
                ? Ok(new { message = "Post updated successfully." })
                : BadRequest(new { message = "Failed to update post." });
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound(new { message = "Post not found." });
            }
            _unitOfWork.Posts.Remove(post);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0
                ? Ok(new { message = "Post deleted successfully." })
                : BadRequest(new { message = "Failed to delete post." });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PostDto>> GetPostById(Guid id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound(new { message = "Post not found." });
            }
            var response = _mapper.Map<Post, CreateUpdatePostRequest>(post);
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<PageResult<PostInListDto>>>GetPostsPaging(string? keyword, Guid? categoryId,
            int pageIndex, int pageSize = 10)
        {
            var result = await _unitOfWork.Posts.GetPostsPagingAsync(keyword, categoryId, pageIndex, pageSize);
            return Ok(result);
        }
    } 
}
