using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTour.Models
{
    [Table("ImageTour")]
    public class ImageTour
    {
        [Key]
        public int Id { get; set; }
        public string? URL { set; get; }

        public int? TourID { get; set; }

        [ForeignKey("TourID")]
        public Tours? Tours { set; get; }
    }
}
