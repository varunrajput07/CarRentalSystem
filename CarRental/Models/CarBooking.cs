using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class CarBooking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime BookingStartDate { get; set; }

        [Required]
        public DateTime BookingEndDate { get; set; }

        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        //public string EmailConfirmation { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string ContactNo { get; set; }

        [Required]
        [StringLength(int.MaxValue)]
        public string Address { get; set; }

        public int CarId { get; set; }
        [ValidateNever]
        public Car Car { get; set; }
        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public decimal? PaymentAmount { get; set; }

        [Required]
        [StringLength(int.MaxValue)]
        public string Note { get; set; }

        [Required]
        public int BookingStatus { get; set; }
        
    }
}
