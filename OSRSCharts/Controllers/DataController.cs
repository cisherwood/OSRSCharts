using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using CsvHelper;
using OSRSCharts.Data;
using OSRSCharts.Models;

namespace OSRSCharts.Controllers
{
    public class DataController : Controller
    {

        private readonly ApplicationDbContext _context;

        public DataController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult UpdateItem()
        {
            List<Item> items = _context.Item.ToList();

            using (var reader = new StreamReader(@"wwwroot\data\itemcsv.csv"))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csvReader.GetRecords<item>();

                foreach(item i in records)
                {
                    if(items.Any(x=>x.ItemID == i.itemID))
                    {
                        // do nothing - item exists
                    }
                    else
                    {
                        // add item to database
                        Item item = new Item()
                        {
                            ItemID = i.itemID,
                            ItemName = i.itemName,
                            ItemMembers = i.itemMembers,
                            ItemTradeable = i.itemTradeable,
                            ItemHighalch = i.itemHighalch,
                            ItemBuyLimit = i.itemBuyLimit
                        };


                        _context.Item.Add(item);
                        _context.SaveChanges();

                    }
                }
            }

             
            return View();

        }

        public class item
        {
            // itemID,itemName,itemMembers,itemTradeable,itemHighalch,itemBuyLimit
            public int itemID { get; set; }
            public string itemName { get; set; }
            public bool itemMembers { get; set; }
            public bool itemTradeable { get; set; }
            public int? itemHighalch { get; set; }
            public int? itemBuyLimit { get; set; }

        }
    }




}







