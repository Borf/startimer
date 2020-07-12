using StarTimer.Models.db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarTimer.Models
{
    public class FullDatabase
    {
        public List<Channel> Channels { get; set; }
        public List<Monster> Monsters { get; set; }
    }
}
