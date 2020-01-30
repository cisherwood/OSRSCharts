using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OSRSCharts.Models
{
    public class Price
    {
        [Key]
        public int PriceID { get; set; }

        public int ItemID { get; set; } // item database ID


        [Required]
        public DateTime Time { get; set; }



        public int? DailyPrice { get; set; }
        public int? AveragePrice { get; set; }

        public Item Item { get; set; }

    }
}
