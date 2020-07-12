using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StarTimer.Models.db
{
    public class Monster
    {
        public int MonsterId { get; set; }

        [Column(TypeName = "VARCHAR(64)")]
        public string Name { get; set; }

        public int RespawnTime { get; set; }

    }
}
