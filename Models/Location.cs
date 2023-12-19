using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTour.Models
{
    [Table("Location")]
    public class Location : Common
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên địa chỉ")]
        [Required(ErrorMessage = "Bắt buộc phải nhập tên địa điểm")]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { set; get; }

        [Display(Name = "Chuỗi định danh (url)", Prompt = "Nhập hoặc để trống tự phát sinh theo Title")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        public string? Slug { set; get; }
        public ICollection<Tours>? Tours { set; get; }

        [Display(Name = "Hình ảnh")]
        public string? Image { set; get; }
    }
}
