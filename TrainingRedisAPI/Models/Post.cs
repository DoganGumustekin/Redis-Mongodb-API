namespace TrainingRedisAPI.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Location { get; set; }
    }
}
