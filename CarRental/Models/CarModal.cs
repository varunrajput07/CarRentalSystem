using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class CarModal
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        public string? Discription { get; set; }

        internal static object GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
