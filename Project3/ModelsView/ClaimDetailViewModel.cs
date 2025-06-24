using Project3.Models;
namespace Project3.ModelsView

{
    public class ClaimDetailViewModel
    {
        public int Id { get; set; }
       
        public string? PolicyNumber { get; set; }
            public string? CustomerName { get; set; }
            public string? CustomerPhoneNumber { get; set; }
            public string? CustomerAddProve { get; set; }
            public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? PolicyStartDate { get; set; }

        public DateOnly? PolicyEndDate { get; set; }
        public string? PlaceOfAccident { get; set; }
            public string? DateOfAccident { get; set; }
            public string? VehicleModel { get; set; }
            public decimal? VehicleRate { get; set; }
            public string? VehicleBodyNumber { get; set; }
            public string? VehicleEngineNumber { get; set; }
            // Các thuộc tính khác nếu cần
        }
    }

    

