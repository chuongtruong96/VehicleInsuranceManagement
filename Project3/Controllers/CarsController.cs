using Microsoft.AspNetCore.Mvc;
using Project3.Services;
using System.Threading.Tasks;

namespace Project3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController(CarService carService) : ControllerBase
    {
        private readonly CarService _carService = carService;

        [HttpGet]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }
    }
}
