using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public required string Title { get; set; }

        // Navigation Properties - Foreign Keys
        public int LearnPlatformId { get; set; }
        public Platform LearnPlatform { get; set; } = null!;

        public int TopicId { get; set; }
        public Topic Topic { get; set; } = null!;

        public bool isCompleted { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }
        public string? Notes { get; set; }

        //User Relationship 
        public required string username { get; set; }

        [ForeignKey(nameof(username))]
        public AppUser User { get; set; } = null!;
    }
}