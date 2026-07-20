using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace backend.Models
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Area { get; set; } = string.Empty;
        public string ApartmentType { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string MapEmbedUrl { get; set; } = string.Empty;

        // Fotoğraflar için list (FormData üzerinden gelir)
        public List<IFormFile>? NewPhotos { get; set; }
        
        // Silinecek mevcut fotoğrafların id'leri
        public List<int>? DeletedPhotoIds { get; set; }
    }
}
