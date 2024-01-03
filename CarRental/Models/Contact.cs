using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Contact
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
      
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        public string ContactNo { get; set; }
        [Required]

        [StringLength(int.MaxValue)]
        public string Message { get; set; }
        
        internal static object GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
