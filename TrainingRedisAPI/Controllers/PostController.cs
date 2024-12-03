using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingRedisAPI.Models;
using TrainingRedisAPI.RedisImplementations;
using TrainingRedisAPI.Repositories;

namespace TrainingRedisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IRedisEntityRepository _redisRepository;
        private readonly PostService _postService;

        public PostController(IRedisEntityRepository redisRepository,PostService postService)
        {
            _redisRepository = redisRepository;
            _postService = postService;
        }

        [Route("CreatePost")]
        [HttpPost]
        public async Task<IActionResult> CreatePost(Post post)
        {
            await _postService.CreateAsync(post);

            _redisRepository.HashSet(post.Id, "LikeCount", post.LikeCount);
            _redisRepository.HashSet(post.Id, "CommentCount", post.CommentCount);

            return Ok();
        }

        [Route("GetPost")]
        [HttpGet]
        public async Task<ActionResult<Post>> GetPost(string postId)
        {
            int likeCount = _redisRepository.HashGet<int>(postId, "LikeCount");
            var commentCount = _redisRepository.HashGet<int>(postId, "CommentCount");

            var post = await _postService.GetAsync(postId);
            post.LikeCount = _redisRepository.HashGet<int>(postId, "LikeCount");
            post.CommentCount = _redisRepository.HashGet<int>(postId, "CommentCount");

            _redisRepository.Refresh(postId);

            return Ok(post);
        }

        [Route("LikePost")]
        [HttpGet]
        public IActionResult LikePost(string id)
        {
            var likeCount = _redisRepository.HashGet<int>(id, "LikeCount");

            _redisRepository.Increment(id.ToString(),"LikeCount",1);
            
            return Ok();
        }

        [Route("AddCommantPost")]
        [HttpGet]
        public IActionResult MakeCommentPost(string id)
        {
            var commentCount = _redisRepository.HashGet<int>(id, "CommentCount");

            _redisRepository.Increment(id, "CommentCount", 1);

            return Ok();
        }
    }
}
