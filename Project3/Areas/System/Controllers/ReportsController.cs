//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Project3.Models;
//using System.Linq;
//using System.Threading.Tasks;
//using Project3.ModelsView;
//using Microsoft.EntityFrameworkCore;


//namespace Project3.Areas.System.Controllers
//{
//    [Area("system")]
//    [Route("system/reports")]
//    public class ReportsController : Controller
//    {
//        private readonly VehicleInsuranceManagementContext _context;

//        public  ReportsController(VehicleInsuranceManagementContext context)
//        {
//            _context = context;
//        }

//        // Action to generate and display the Monthly Sales Report
//        [HttpGet]
//        [Route("MonthlySalesReport")]
//        public async Task<IActionResult> MonthlySalesReport()
//        {
//            var monthlySales = await _context.CompanyBillingPolicies
//                .GroupBy(p => new { p.Date.Value.Year, p.Date.Value.Month })
//                .Select(g => new MonthlySalesReportViewModel
//                {
//                    Year = g.Key.Year,
//                    Month = g.Key.Month,
//                    TotalSales = g.Sum(p => p.Amount)
//                })
//                .ToListAsync();

//            return View(monthlySales);
//        }


//        // Action to generate and display the Vehicle-Wise Analysis Report
//        [HttpGet("VehicleAnalysis")]
//        public async Task<IActionResult> VehicleWiseAnalysisReport()
//        {
//            var vehicleAnalysis = await _context.VehicleInformations
//                .GroupBy(v => v.VehicleModel)
//                .Select(g => new VehicleWiseAnalysisReportViewModel
//                {
//                    VehicleModel = g.Key,
//                    TotalPolicies = g.Count(),
//                    TotalRevenue = (double)g.Sum(v => v.VehicleRate)
//                })
//                .ToListAsync();

//            return View(vehicleAnalysis); // Return a view displaying the report
//        }

//        // Action to generate and display the Claims Report
//        [HttpGet("Claims")]
//        public async Task<IActionResult> ClaimsReport()
//        {
//            var claimsReport = await _context.ClaimDetails
//                .Select(c => new ClaimsReportViewModel
//                {
//                    ClaimNumber = c.ClaimNumber,
//                    PolicyNumber = c.PolicyNumber,
//                    PolicyStartDate = c.PolicyStartDate,
//                    PolicyEndDate = c.PolicyEndDate,
//                    CustomerName = c.CustomerName,
//                    PlaceOfAccident = c.PlaceOfAccident,
//                    DateOfAccident = c.DateOfAccident,
//                    InsuredAmount = c.InsuredAmount,
//                    ClaimableAmount = c.ClaimableAmount
//                })
//                .ToListAsync();

//            return View(claimsReport); // Return a view displaying the report
//        }


//        // Action to generate and display the Policies Due for Renewal Report
//        [HttpGet("DueRenewals")]
//        public async Task<IActionResult> PoliciesDueRenewalsReport()
//        {
//            var today = DateTime.Today;

//            // Step 1: Retrieve all records with PolicyDuration and PolicyDate not null
//            var insuranceProcesses = await _context.InsuranceProcesses
//                .Where(p => p.PolicyDuration.HasValue && !string.IsNullOrEmpty(p.PolicyDate))
//                .ToListAsync();

//            // Step 2: Filter the records in memory after parsing PolicyDate
//            var dueRenewals = insuranceProcesses
//                .Where(p => DateTime.TryParse(p.PolicyDate, out DateTime policyDate) &&
//                            policyDate.AddMonths((int)p.PolicyDuration.Value) > today &&
//                            policyDate.AddMonths((int)p.PolicyDuration.Value) <= today.AddMonths(1))
//                .Select(p => new PoliciesDueRenewalsReportViewModel
//                {
//                    PolicyNumber = p.PolicyNumber,
//                    CustomerName = p.CustomerName,
//                    VehicleModel = p.VehicleModel,
//                    PolicyEndDate = DateTime.Parse(p.PolicyDate).AddMonths((int)p.PolicyDuration.Value)
//                })
//                .ToList();

//            return View(dueRenewals); // Return a view displaying the report
//        }

//        // Action to generate and display the Policies Lapsed Report
//        [HttpGet("LapsedPolicies")]
//        public async Task<IActionResult> PoliciesLapsedReport()
//        {
//            var today = DateTime.Today;

//            // Retrieve all records with PolicyDuration not null and where PolicyDate is not null or empty
//            var insuranceProcesses = await _context.InsuranceProcesses
//                .Where(p => p.PolicyDuration.HasValue && !string.IsNullOrEmpty(p.PolicyDate))
//                .ToListAsync();

//            // Filter the records in memory after parsing PolicyDate
//            var lapsedPolicies = insuranceProcesses
//                .Where(p => DateTime.TryParse(p.PolicyDate, out DateTime policyDate) &&
//                            policyDate.AddMonths((int)p.PolicyDuration.Value) < today)
//                .Select(p => new PoliciesLapsedReportViewModel
//                {
//                    PolicyNumber = p.PolicyNumber,
//                    CustomerName = p.CustomerName,
//                    VehicleModel = p.VehicleModel,
//                    PolicyEndDate = DateTime.Parse(p.PolicyDate).AddMonths((int)p.PolicyDuration.Value)
//                })
//                .ToList();

//            return View(lapsedPolicies); // Return a view displaying the report
//        }

//    }
//}
