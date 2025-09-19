namespace backend.Models
{
    public class ProjectPhoto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
    }
}
