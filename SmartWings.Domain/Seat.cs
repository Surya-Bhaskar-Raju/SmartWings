using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartWings.Domain
{
    public class Seat
    {
        [Key]
        public Guid SeatId { get; set; }

        [ForeignKey("Flight")]
        public Guid FlightId { get; set; }

        [Required, StringLength(10)]
        public string SeatNumber { get; set; }

        [Required, StringLength(10)]
        public string Class { get; set; }

        public bool IsBooked { get; set; }

        public Flight Flight { get; set; }



    }

}
