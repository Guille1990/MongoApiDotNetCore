using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoApi.Models;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoApi.Controllers
{
  [Route("api/[controller]")]
  public class UsersController : ControllerBase
  {
    private readonly IUserRepository _userRepository;

    public UsersController (IUserRepository userRepository)
    {
      _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult> Get ()
    {
      return new ObjectResult(await _userRepository.GetAll());
    }

    [HttpGet("{id}", Name = "Get")]
    public async Task<ActionResult> Get (string id)
    {
      var user = await _userRepository.GetUser(id);
      Console.WriteLine(user);
      if (user == null)
      {
        return new NotFoundResult();
      }
      return new ObjectResult(user);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] User user)
    {
      await _userRepository.Create(user);
      return new OkObjectResult(user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put (string id, [FromBody] User user)
    {
      var userFromDb = await _userRepository.GetUser(id);
      Console.WriteLine(userFromDb.ToJson().ToString());
      if (userFromDb == null)
      {
        return new NotFoundResult();
      }

      user.Id = userFromDb.Id;
      await _userRepository.Update(user);

      return new OkObjectResult(user);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete (string id)
    {
      var userFromDb = await _userRepository.GetUser(id);
      Console.WriteLine(userFromDb.ToJson().ToString());
      if (userFromDb == null)
      {
        return new NotFoundResult();
      }

      await _userRepository.Delete(id);
      return new OkResult();
    }
  }
}