using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OSRSCharts.Models
{
    public class TradeVolume
    {
        [Key]
        public int TradeVolumeID { get; set; }

        public int ItemID { get; set; } // item database ID


        [Required]
        public DateTime Time { get; set; }

        public int NumberOfTrades { get; set; }

        public Item Item { get; set; }
    }
}
