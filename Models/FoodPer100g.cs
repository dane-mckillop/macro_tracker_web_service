using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace macro_tracker_web_service.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class FoodPer100g
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FoodId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public double Protein { get; set; }
        public double Calories { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
        public double Price { get; set; }
    }
}
