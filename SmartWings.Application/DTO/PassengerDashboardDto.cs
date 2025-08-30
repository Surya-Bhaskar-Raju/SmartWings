using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.Application.DTO
{
    public class PassengerDashboardDto
    {
        public Guid PassengerId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string PassportNumber { get; set; }
        public string SeatNumber { get; set; }
        public string SeatClass { get; set; }
    }
}
