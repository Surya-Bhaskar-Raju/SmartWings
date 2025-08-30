using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWings.Application.DTO
{
    public class AirCraftCreateDto
    {
        public string Model { get; set; }
        public int TotalSeats { get; set; }
        public int EconomySeats { get; set; }
        public int BusinessSeats { get; set; }
    }

    public class AirCraftReadDto
    {
        public Guid AirCraftId { get; set; }
        public string Model { get; set; }
        public int TotalSeats { get; set; }
        public int EconomySeats { get; set; }
        public int BusinessSeats { get; set; }
    }

    public class AirCraftUpdateDto
    {
        public Guid AirCraftId { get; set; }
        public string Model { get; set; }
        public int TotalSeats { get; set; }
        public int EconomySeats { get; set; }
        public int BusinessSeats { get; set; }
    }


}
