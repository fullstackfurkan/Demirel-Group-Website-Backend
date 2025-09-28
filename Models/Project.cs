namespace backend.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category {  get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Area {  get; set; } = string.Empty;
        public string ApartmentType {  get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Details {  get; set; } = string.Empty;
        public string Location {  get; set; } = string.Empty;
        public string MapEmbedUrl { get; set; } = string.Empty;

        public List<ProjectPhoto> Photos { get; set; } = [];
    }
}