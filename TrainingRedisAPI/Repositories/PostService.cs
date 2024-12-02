using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TrainingRedisAPI.Models;

namespace TrainingRedisAPI.Repositories
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _postCollection;

        public PostService(
            IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(
                settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _postCollection = mongoDatabase.GetCollection<Post>(
                settings.Value.CollectionName);
        }

        public async Task<List<Post>> GetAsync() =>
            await _postCollection.Find(_ => true).ToListAsync();

        public async Task<Post?> GetAsync(string id) =>
            await _postCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Post newBook) =>
            await _postCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Post updatedBook) =>
            await _postCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _postCollection.DeleteOneAsync(x => x.Id == id);
    }
}
