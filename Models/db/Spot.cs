using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarTimer.Models.db
{
    public class Spot
    {
        public int SpotId { get; set; }

        public Channel Channel { get; set; }

        public Monster Monster { get; set; }

        public DateTime? lastSpot { get; set; }
        public DateTime? lastNoSpot { get; set; }
    }
}
