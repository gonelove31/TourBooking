using System.ComponentModel.DataAnnotations;

namespace BookingTour.Models
{
    public class Hotel
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "Tên khách sạn")]
        [Required(ErrorMessage = "Bắt buộc phải nhập tên địa điểm")]
        public string Name { get; set; }

        [Display(Name = "Mô tả khách sạn")]
        public string Description { get; set; }

        [Display(Name = "Ảnh khách sạn")]
        public string Image { get; set; }
        
        }
    }
}
