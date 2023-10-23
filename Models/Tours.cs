using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BookingTour.Models
{

    [Table("Tours")]
    public class Tours
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên tour")]
        public string? Name { get; set; }

        [Display(Name = "Mô tả")]
        public string? Description { get; set; }

        [Display(Name = "Đánh giá")]
        public float? Rate { get; set; }

        [Display(Name = "Giá")]
        public decimal? Price { get; set; }

        [Display(Name = "Số lượng còn lại")]
        public int? AvailableSeats { get; set; }

        [Display(Name = "Chuỗi định danh (url)", Prompt = "Nhập hoặc để trống tự phát sinh theo Title")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        public string? Slug { set; get; }

        [Display(Name = "Thời gian bắt đầu")]
        public DateTime? StartDate { get; set; }

        [Display(Name = "Thời gian kết thúc")]
        public DateTime? EndDate { get; set; }

        public int? LocationID { get; set; }

        [ForeignKey("LocationID")]
        public Location? Location { set; get; }

        [Display(Name = "Hình ảnh")]
        public string? Image { set; get; }

    }
}
