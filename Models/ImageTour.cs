using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTour.Models
{
    [Table("ImageTour")]
    public class ImageTour
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Ảnh")]
        public string? URL { set; get; }

        [Display(Name  = "Tên tour")]
        public int? TourID { get; set; }

        [ForeignKey("TourID")]
        [Display(Name = "Tên tour")]

        public Tours? Tours { set; get; }
    }
}
