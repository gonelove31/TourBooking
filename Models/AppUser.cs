using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookingTour.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<UserActionHistory> UserActionHistories { get; set; }
    }
}