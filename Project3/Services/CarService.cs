using Newtonsoft.Json;
using Project3.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Project3.Services
{
    public class CarService
    {
        private readonly string _filePath;

        public CarService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.WebRootPath, "cars.json");
        }

        public async Task<List<Car>> GetAllCarsAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Car>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonConvert.DeserializeObject<List<Car>>(json) ?? new List<Car>();
        }

        // This method should no longer be used, since we are not writing to the JSON file
        public async Task SaveCarsAsync(List<Car> cars)
        {
            var json = JsonConvert.SerializeObject(cars, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }

        public async Task<List<VehicleInformation>> GetAllVehicleInformationsAsync()
        {
            var cars = await GetAllCarsAsync();
            var vehicleList = cars.SelectMany(car => car.Models.Select(model => new VehicleInformation
            {
                VehicleName = car.Name,
                VehicleModel = model.Name,
                VehicleVersion = model.Codename,
                //VehicleRate = model.Trims.FirstOrDefault()?.Price ?? 0
            })).ToList();

            return vehicleList;
        }

        public async Task<VehicleInformation> GetVehicleInformationById(int id)
        {
            var vehicles = await GetAllVehicleInformationsAsync();
            return vehicles.FirstOrDefault(v => v.Id == id);
        }
    }
}
