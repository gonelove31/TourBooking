using System.ComponentModel.DataAnnotations.Schema;

namespace BookingTour.Models
{
    public class UserActionHistory
    {
        public int Id { get; set; }

        // Khóa ngoại liên kết với bảng người dùng Identity
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
        [NotMapped]
        public string UserName => User?.UserName;
    }
}
