namespace Project3.ModelsView
{
    public class ClaimsReportViewModel
    {
        public decimal? ClaimNumber { get; set; }
        public string? PolicyNumber { get; set; }
        public DateTime? PolicyStartDate { get; set; }
        public DateTime? PolicyEndDate { get; set; }

        public string? CustomerName { get; set; }

        public string PlaceOfAccident { get; set; }
        public string DateOfAccident { get; set; }
        public decimal? InsuredAmount { get; set; }
        public decimal? ClaimableAmount { get; set; }
    }
}
