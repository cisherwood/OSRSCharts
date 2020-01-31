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
using System.Net.Http;
using System.Threading;

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

        // Update daily and average prices per day
        public async Task<IActionResult> UpdatePrice()
        {
            // Get all tradeable items
            List<Item> items = _context.Item.Where(x=>x.ItemTradeable == true).Where(x=>x.ItemBuyLimit != null).ToList();
            List<Price> existingPrices = _context.Price.ToList();

            string url; // OSRS GE API url
            string json; // json data from rquest

            foreach(Item i in items) // Loop through all items
            {
                Thread.Sleep(1000);

                // list of prices for item
                List<Price> prices = new List<Price>();
                int itemid = items.Where(x => x.ItemID == i.ItemID).FirstOrDefault().ID; // get database item ID from OSRS item id

                int c = 1; // counter variable - how many times to try to reach OSRS GE API?

                while (c <= 3) // countiue to attempt to query API until 3 attempts have been reached
                {

                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {


                            url = "http://services.runescape.com/m=itemdb_oldschool/api/graph/" + i.ItemID + ".json"; // Create url
                            json = await client.GetStringAsync(url); // Get JSON data

                            PriceConvert price = JsonConvert.DeserializeObject<PriceConvert>(json); // Deserialize json data to C# object

                            foreach (var averagePrice in price.Average) // Loop through each average price 
                            {
                                Price p = new Price(); // Create new price object

                                p.ItemID = itemid; // Set price object item ID to database value of item id
                                p.AveragePrice = Convert.ToInt32(averagePrice.Value); // convert price (long) to int
                                p.Time = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(averagePrice.Key)).UtcDateTime; // convert epoch (unix) time to UTC

                                prices.Add(p); // Add price to list of prices
                            }


                            foreach (var dailyPrice in price.Daily) // Loop through all daily key / values
                            {
                                DateTime dailyTime = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(dailyPrice.Key)).UtcDateTime; // conver epoch (unix) time to UTC

                                prices.Where(x => x.ItemID == itemid).Where(x => x.Time == dailyTime).FirstOrDefault().DailyPrice = Convert.ToInt32(dailyPrice.Value); // Select average price entry, and add daily price value to it
                            }


                        }

                        c = 4; // set counter above # of tries threshold (sucssesfull API query, move on

                    }
                    catch (Exception e)
                    {
                        Thread.Sleep(2000); // unable to reach GE API - wait
                        c++; // increment counter variable
                    }

                } 



                foreach (Price p in prices) // Loop through all days for this item
                {
                    // TODO - check it item price data exists
                    if(existingPrices.Where(x=>x.ItemID == i.ID).Where(x=>x.Time == p.Time).Any())
                    {
                        // record already exists - do nothing
                    }
                    else
                    {
                        _context.Price.Add(p); // Add price object to database

                    }

                }

                _context.SaveChanges();

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


        public partial class PriceConvert
        {
            [JsonProperty("daily")]
            public Dictionary<string, long> Daily { get; set; }

            [JsonProperty("average")]
            public Dictionary<string, long> Average { get; set; }
        }


    }




}







