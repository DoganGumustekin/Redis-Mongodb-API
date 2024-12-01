using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TrainingRedisAPI.Models;

namespace TrainingRedisAPI.Repositories
{
    public class PostService
    {
        private readonly IMongoCollection<Post> _booksCollection;

        public PostService(
            IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(
                settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<Post>(
                settings.Value.CollectionName);
        }

        public async Task<List<Post>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<Post?> GetAsync(string id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Post newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Post updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
