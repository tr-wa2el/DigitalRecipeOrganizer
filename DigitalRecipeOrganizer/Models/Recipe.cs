using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalRecipeOrganizer.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        public string Ingredients { get; set; } = string.Empty;

        public string Instructions { get; set; } = string.Empty;

        public int PrepTimeMinutes { get; set; }

        public int CookTimeMinutes { get; set; }

        public int Servings { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? LastModifiedDate { get; set; }

        // For meal planning
        public DateTime? ScheduledDate { get; set; }

        public string? Notes { get; set; }

        // User relationship
        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
