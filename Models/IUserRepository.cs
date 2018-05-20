using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;

namespace MongoApi.Models
{
  public class UserRepository: IUserRepository
  {
    private readonly IUserContext _context;

    public UserRepository(IUserContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
      return await _context
        .Users
        .Find(_ => true)
        .ToListAsync();
    }

    public Task<User> GetUser(string id)
    {
      FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Id, new ObjectId(id));
      return _context
        .Users
        .Find(filter)
        .FirstOrDefaultAsync();
    }

    public async Task Create (User user)
    {
      await _context.Users.InsertOneAsync(user);
    }


    public async Task<bool> Update (User user)
    {
      ReplaceOneResult updateUser = await _context
        .Users
        .ReplaceOneAsync(filter: u => u.Id == user.Id, replacement: user);

      return updateUser.IsAcknowledged && updateUser.ModifiedCount > 0;
    }

    public async Task<bool> Delete (string id)
    {
      FilterDefinition<User> filter = Builders<User>.Filter.Eq(u => u.Id, new ObjectId(id));
      DeleteResult deleteResult = await _context
        .Users
        .DeleteOneAsync(filter);

      return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
  }

  public interface IUserRepository
  {
    Task<IEnumerable<User>> GetAll();
    Task<User> GetUser(string id);
    Task Create(User user);
    Task<bool> Update (User user);
    Task<bool> Delete (string id);

  }
}