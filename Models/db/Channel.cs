using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StarTimer.Models.db
{
    public class Channel
    {
        public int ChannelId { get; set; }

        [Column(TypeName = "VARCHAR(4)")]
        public string Name { get; set; }

    }
}
