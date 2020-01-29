using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OSRSCharts.Models
{
    public class Item
    {
        [Key]
        public int ID { get; set; } // database ID

        public int ItemID { get; set; } // OSRS item number

        [MaxLength(500)]
        public string ItemName { get; set; }
        public bool ItemMembers { get; set; }
        public bool ItemTradeable { get; set; }
        public int? ItemHighalch { get; set; }
        public int? ItemBuyLimit { get; set; }
    }
}
