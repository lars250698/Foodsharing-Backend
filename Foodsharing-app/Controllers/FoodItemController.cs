using System.Collections.Generic;
using System.Linq;
using Foodsharing_app.Models;
using Foodsharing_app.Services;
using Microsoft.AspNetCore.Mvc;

namespace Foodsharing_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoodItemController : Controller
    {
        private readonly FoodItemService _foodItemService;

        public FoodItemController([FromBody] FoodItemService foodItemService)
        {
            _foodItemService = foodItemService;
        }

        [HttpPost]
        public FoodItem Post(FoodItem item)
        {
            item.SetPosition(item.Coordinates);
            _foodItemService.Create(item);
            return item;
        }

        [HttpGet]
        public List<FoodItem> GetAll() =>
            _foodItemService.Get();

        [HttpGet("{id}")]
        public FoodItem GetById([FromRoute] string id) =>
            _foodItemService.Get(id);

        [HttpGet("byRadius")]
        public List<FoodItem> GetByRadius([FromQuery] double lat, [FromQuery] double lng, [FromQuery] int radius)
        {
            return _foodItemService.GetByRadius(lat, lng, radius)
                .Where(item => item.Show())
                .ToList();
        }

        [HttpGet("byUser")]
        public List<FoodItem> GetByUser([FromQuery] string userId) =>
            _foodItemService.GetByUser(userId);
    }
}