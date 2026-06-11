using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CourseCreateDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public int PlatformId { get; set; }
        [Required]
        public int TopicId { get; set; }
        
        public bool IsCompleted { get; set; }
        
        [Range(1, 5)]
        public int? Rating { get; set; }
        public string? Notes { get; set; }
        }
}