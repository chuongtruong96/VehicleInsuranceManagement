using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Project3;
using Project3.Models;
using Project3.ModelsView;

[Authorize]
[Route("[controller]")]
public class InsuranceProductsController : Controller
{
    private readonly VehicleInsuranceManagementContext _context;
    private readonly ILogger<InsuranceProductsController> _logger;
    private readonly Dictionary<int, string> _policyImageMap;

    public InsuranceProductsController(VehicleInsuranceManagementContext context, ILogger<InsuranceProductsController> logger, IWebHostEnvironment env)
    {
        _context = context;
        _logger = logger;

        // Load image mappings from JSON configuration file in wwwroot
        try
        {
            var jsonFilePath = Path.Combine(env.WebRootPath, "policyImages.json");
            if (System.IO.File.Exists(jsonFilePath))
            {
                var json = System.IO.File.ReadAllText(jsonFilePath);
                _policyImageMap = JsonConvert.DeserializeObject<Dictionary<int, string>>(json);
            }
            else
            {
                _logger.LogError("The JSON file 'policyImages.json' was not found at {Path}.", jsonFilePath);
                _policyImageMap = new Dictionary<int, string>(); // Fallback to empty dictionary
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while reading the 'policyImages.json' file.");
            _policyImageMap = new Dictionary<int, string>(); // Fallback to empty dictionary
        }
    }

    [Route("index")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Fetch all policy types and warranties
        var policies = await _context.VehiclePolicyTypes.ToListAsync();
        var warranties = await _context.VehicleWarranties.ToListAsync();

        // Create a list of InsuranceProductViewModel to represent the product cards
        var insuranceProducts = new List<InsuranceProductViewModel>();

        // Create combinations: pair each policy type with each warranty
        foreach (var policy in policies)
        {
            foreach (var warranty in warranties)
            {
                // Adjust the rate based on the warranty type or duration
                float adjustedRate = CalculateAdjustedRate((float)policy.VehicleRate, warranty.WarrantyDuration);

                // Get the image URL from the map
                if (!_policyImageMap.TryGetValue(policy.PolicyTypeId, out string imageUrl))
                {
                    imageUrl = "/images/default-product.jpg"; // Default image if no match found
                }

                // Add the product to the list
                insuranceProducts.Add(new InsuranceProductViewModel
                {
                    PolicyTypeId = policy.PolicyTypeId,
                    PolicyName = policy.PolicyName,
                    PolicyDetails = policy.PolicyDetails,
                    WarrantyId = warranty.WarrantyId,
                    WarrantyType = warranty.WarrantyType,
                    WarrantyDuration = warranty.WarrantyDuration,
                    WarrantyDetails = warranty.WarrantyDetails,
                    VehicleRate = adjustedRate, // Use the adjusted rate
                    ImageUrl = imageUrl // Use the image URL from the configuration
                });
            }
        }

        return View(insuranceProducts);
    }

    [Route("confirmproduct")]
    [HttpGet]
    public async Task<IActionResult> Buy(int policyTypeId, int warrantyId)
    {
        // Fetch the selected policy and warranty from the database
        var policy = await _context.VehiclePolicyTypes.FirstOrDefaultAsync(p => p.PolicyTypeId == policyTypeId);
        var warranty = await _context.VehicleWarranties.FirstOrDefaultAsync(w => w.WarrantyId == warrantyId);

        if (policy == null || warranty == null)
        {
            return NotFound("Policy or Warranty not found.");
        }

        // Get the image URL from the map
        _policyImageMap.TryGetValue(policy.PolicyTypeId, out string imageUrl);

        // Create a view model with the selected product details
        var product = new InsuranceProductViewModel
        {
            PolicyTypeId = policy.PolicyTypeId,
            PolicyName = policy.PolicyName,
            PolicyDetails = policy.PolicyDetails,
            WarrantyId = warranty.WarrantyId,
            WarrantyType = warranty.WarrantyType,
            WarrantyDuration = warranty.WarrantyDuration,
            WarrantyDetails = warranty.WarrantyDetails,
            VehicleRate = CalculateAdjustedRate((float)policy.VehicleRate, warranty.WarrantyDuration),
            ImageUrl = imageUrl // Pass the image URL
        };

        return View(product); // Return the Buy view with the selected product
    }

    [Route("confirmproduct")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Buy(int policyTypeId, int warrantyId, bool confirm = true)
    {
        // Fetch the selected policy and warranty from the database
        var policy = await _context.VehiclePolicyTypes.FirstOrDefaultAsync(p => p.PolicyTypeId == policyTypeId);
        var warranty = await _context.VehicleWarranties.FirstOrDefaultAsync(w => w.WarrantyId == warrantyId);

        if (policy == null || warranty == null)
        {
            return NotFound("Policy or Warranty not found.");
        }

        float adjustedRate = CalculateAdjustedRate((float)policy.VehicleRate, warranty.WarrantyDuration);

        // Get the image URL from the map
        _policyImageMap.TryGetValue(policy.PolicyTypeId, out string imageUrl);

        // Create a session object to store the selected product, including ImageUrl
        var productSession = new InsuranceProductViewModel
        {
            PolicyTypeId = policy.PolicyTypeId,
            PolicyName = policy.PolicyName,
            PolicyDetails = policy.PolicyDetails,
            WarrantyId = warranty.WarrantyId,
            WarrantyType = warranty.WarrantyType,
            WarrantyDuration = warranty.WarrantyDuration,
            WarrantyDetails = warranty.WarrantyDetails,
            VehicleRate = adjustedRate,
            ImageUrl = imageUrl // Include the ImageUrl
        };

        // Save the productSession into the session
        HttpContext.Session.SetObject("productSession", productSession);

        // Log the session data to console
        var sessionData = HttpContext.Session.GetObject<InsuranceProductViewModel>("productSession");
        _logger.LogInformation("Session Confirmation: {@SessionData}", sessionData);

        // Redirect to confirmation page
        return RedirectToAction("Confirmation");
    }

    [Route("Confirmation")]
    [HttpGet]
    public IActionResult Confirmation()
    {
        // Retrieve the productSession if it exists
        var productSession = HttpContext.Session.GetObject<InsuranceProductViewModel>("productSession");

        if (productSession == null)
        {
            return RedirectToAction("Index"); // Redirect to the product listing if no session is found
        }

        // Check if the user is authenticated
        if (!User.Identity.IsAuthenticated)
        {
            // If the user is not authenticated, redirect them to the login page
            return RedirectToAction("Login", "Account");
        }

        return View(productSession); // Pass the session data to the view for display
    }

    private float CalculateAdjustedRate(float baseRate, string warrantyDuration)
    {
        float adjustedRate = baseRate;

        if (warrantyDuration.Contains("5"))
        {
            adjustedRate += 50;  // Add $50 for 5-year warranties
        }
        else if (warrantyDuration.Contains("7"))
        {
            adjustedRate += 100; // Add $100 for 7-year warranties
        }

        return adjustedRate;
    }
}
