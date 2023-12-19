using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTour.Models
{
    [Table("Booking")]
    public class Booking : Common
    {
        [Key]
        public int Id { get; set; }
        public int? CustomerID { get; set; }
        [Required(ErrorMessage = "Bắt buộc phải nhập id chuyến đi")]
        public int TourID { get; set; }

        [Display(Name = "Ngày đặt")]
        public DateTime? BookingDate { get; set; }

        [Display(Name = "Số lượng người lớn")]
        [Required(ErrorMessage = "Bắt buộc phải nhập số người lớn")]
        public int NumberOfAdult { get; set; }

        [Display(Name = "Số lượng trẻ em ( dưới 12 tuổi )")]
        [Required(ErrorMessage = "Bắt buộc phải nhập số trẻ em")]
        public int NumberOfChildren  { get; set; }

        [Display(Name = "Tổng tiền")]
        public decimal? TotalAmount { get; set; }

        public Tours? Tour { set; get; }

        [Display(Name = "Trạng thái")]
        public int? Status { set; get; }

    }
}
