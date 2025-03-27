using System;
using Microsoft.AspNetCore.Mvc;
using macro_tracker_web_service.Services;
using macro_tracker_web_service.Models;

namespace macro_tracker_web_service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodsController : ControllerBase
    {
        private readonly FoodService _foodService;

        public FoodsController(FoodService foodService)
        {
            _foodService = foodService;
        }

        // GET: api/Foods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FoodPer100g>>> GetAllFoods()
        {
            var foods = await _foodService.GetAllFoodsAsync();
            return Ok(foods);
        }

        // GET: api/Foods/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FoodPer100g>> GetFoodById(int id)
        {
            var food = await _foodService.GetFoodByIdAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return Ok(food);
        }

        // GET: api/Foods/name/Pork
        [HttpGet("name/{name}")]
        public async Task<ActionResult<FoodPer100g>> GetFoodByName(string name)
        {
            var food = await _foodService.GetFoodByNameAsync(name);
            if (food == null)
            {
                return NotFound();
            }
            return Ok(food);
        }

        // GET: api/Foods/substring/Pork
        [HttpGet("substring/{name}")]
        public async Task<ActionResult<IEnumerable<FoodPer100g>>> GetFoodBySubstring(string name)
        {
            var foods = await _foodService.GetFoodBySubstringAsync(name);
            if (foods == null || !foods.Any())
            {
                return NotFound();
            }
            return Ok(foods);
        }

        // POST: api/Foods
        [HttpPost]
        public async Task<ActionResult<FoodPer100g>> CreateFood(FoodPer100g food)
        {
            if (food == null)
            {
                return BadRequest("Food item cannot be null");
            }

            var createdFood = await _foodService.CreateFoodAsync(food);
            return CreatedAtAction(nameof(GetFoodById), new { id = createdFood?.FoodId }, createdFood);
        }

        // PUT: api/Foods
        [HttpPut]
        public async Task<IActionResult> UpdateFood(FoodPer100g food)
        {
            if (food == null)
            {
                return BadRequest("Food item cannot be null");
            }

            await _foodService.UpdateFoodAsync(food);
            return NoContent();
        }
    }
}