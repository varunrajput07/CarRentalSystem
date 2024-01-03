using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string Number { get; set; }

        [Required]
        public int NumOfSeats { get; set; }

        [Required]
        public int Mileage { get; set; }
        [ValidateNever]
        public string CarImage { get; set; }

        [Required]
        public decimal? KMUsed { get; set; }

        [Required]
        public int NumOfAirbags { get; set; }

        [Required]
        public int FuelType { get; set; }

        [Required]
        public decimal? Price { get; set; }

        [Required]
        [StringLength(int.MaxValue)]
        public string Description { get; set; }

        public int CarModalId { get; set; }
        [ValidateNever]
        public CarModal CarModal { get; set; }
      
    }
}
