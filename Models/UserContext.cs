using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace MongoApi.Models
{
  public class UserContext: IUserContext
  {
    private readonly IMongoDatabase _db;
    public UserContext (IOptions<Setting> setting)
    {
      var client = new MongoClient(setting.Value.ConnectionStrig);
      _db = client.GetDatabase(setting.Value.Database);
    }

    public IMongoCollection<User> Users => _db.GetCollection<User>("user");
  }

  public interface IUserContext
  {
    IMongoCollection<User> Users { get; }
  }

  public class Setting
  {
    public string ConnectionStrig { get; set; }
    public string Database { get; set; }
  }
}