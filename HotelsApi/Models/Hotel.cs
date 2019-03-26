using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsApi.Models
{
    public class Hotel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Required]
        [StringLength(128)]
        public string HotelName { get; set; }

        [StringLength(2048)]
        public string Description { get; set; }

        [Required]
        [StringLength(64)]
        public string Country { get; set; }

        [Required]
        [StringLength(64)]
        public string City { get; set; }

        [Required]
        [StringLength(128)]
        public string Address { get; set; }

        [StringLength(64)]
        public string PostalCode { get; set; }

        [StringLength(128)]
        public string ImageUrl { get; set; }
    }
}
