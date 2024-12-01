using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingRedisAPI.Models;
using TrainingRedisAPI.RedisImplementations;

namespace TrainingRedisAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        private readonly IRedisEntityRepository _redisRepository;

        public RedisController(IRedisEntityRepository redisRepository)
        {
            _redisRepository = redisRepository;
        }

        [Route("SetPost")]
        [HttpPost]
        public IActionResult AddDataToRedis(Post param)
        {
            Post post = new Post 
            { 
                Id = 1,
                Name = "hello",
                LikeCount = 45,
                CommentCount = 14,
                CreatedDate = DateTime.Now,
                Location = "İstanbul - Bahçelievler" 
            };

            PostLikeCommentCountDTO postLikeCommentCountDTO = new PostLikeCommentCountDTO 
            { 
                LikeCount = post.LikeCount,
                CommentCount = post.CommentCount
            };
            
            _redisRepository.HashSet(post.Id.ToString(), "LikeCount", postLikeCommentCountDTO.LikeCount);
            _redisRepository.HashSet(post.Id.ToString(), "CommentCount", postLikeCommentCountDTO.CommentCount);

            return Ok();
        }

        [Route("GetPost")]
        [HttpGet]
        public IActionResult GetPost(int id = 1)
        {
            var likeCount = _redisRepository.HashGet<int>(id.ToString(), "LikeCount");
            var commentCount = _redisRepository.HashGet<int>(id.ToString(), "CommentCount");

            Post post = new Post
            {
                Id = 1,
                Name = "hello",
                LikeCount = likeCount,
                CommentCount = commentCount,
                CreatedDate = DateTime.Now,
                Location = "İstanbul - Bahçelievler"
            };

            return Ok(post);
        }

        [Route("LikePost")]
        [HttpGet]
        public IActionResult LikePost(int id = 1)
        {
            var likeCount = _redisRepository.HashGet<int>(id.ToString(), "LikeCount");
            var commentCount = _redisRepository.HashGet<int>(id.ToString(), "CommentCount");

            _redisRepository.Increment(id.ToString(),"LikeCount",1);
            
            return Ok();
        }

        [Route("AddCommantPost")]
        [HttpGet]
        public IActionResult MakeCommentPost(int id = 1)
        {
            var likeCount = _redisRepository.HashGet<int>(id.ToString(), "LikeCount");
            var commentCount = _redisRepository.HashGet<int>(id.ToString(), "CommentCount");

            _redisRepository.Increment(id.ToString(), "CommentCount", 1);

            return Ok();
        }
    }
}
