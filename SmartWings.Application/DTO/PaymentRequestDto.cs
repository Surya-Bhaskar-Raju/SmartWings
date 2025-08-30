using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.Application.DTO
{
    public class PaymentRequestDto
    {
        public Guid BookingId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
    }
}
