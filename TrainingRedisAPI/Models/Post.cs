using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TrainingRedisAPI.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Location { get; set; }
    }
}
