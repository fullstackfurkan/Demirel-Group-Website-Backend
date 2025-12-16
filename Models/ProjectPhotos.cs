namespace backend.Models
{
    public class ProjectPhotos
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public string OriginalFileName { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }
}
