using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartWings.Domain
{
    public class AirCraft
    {
        [Key]
        public Guid AirCraftId { get; set; }

        [Required]
        [StringLength(50)]
        public string Model { get; set; }

        public int TotalSeats { get; set; }

        public int EconomySeats { get; set; }

        public int BusinessSeats { get; set; }

        public ICollection<Flight> Flights { get; set; }

    }
}
