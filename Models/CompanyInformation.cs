namespace backend.Models
{
    public class CompanyInformation
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyAdress { get; set; } = string.Empty;
        public string ContactNumber1 { get; set; } = string.Empty;
        public string ContactNumber2 { get; set; } = string.Empty;
        public string AboutUsText { get; set; } = string.Empty;
        public string FooterText { get; set; } = string.Empty;
        public string CompanyEmail { get; set; } = string.Empty;
    }
}
