using System.Collections.Generic;
using Foodsharing_app.Models;
using Foodsharing_app.Services;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public User CreateUser([FromBody] User user, [FromQuery] string password) =>
            _userService.CreateUser(user, password);

        [HttpGet]
        [Route("{id}")]
        public User GetUserById([FromRoute] string id) =>
            _userService.Get(id);

        [HttpGet]
        public User GetUserByName([FromQuery] string username) =>
            _userService.GetByUsername(username);

        [HttpGet]
        public List<User> GetAllUsers() =>
            _userService.Get();

        [HttpDelete]
        [Route("{id}")]
        public void DeleteUser([FromRoute] string id) =>
            _userService.Remove(id);

    }
}