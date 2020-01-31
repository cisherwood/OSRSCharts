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
using HtmlAgilityPack;

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

        public IActionResult UpdateTradeVolume()
        {
            // Get all tradeable items
            // TODO - Get items with price history data
            List<Item> items = _context.Item.Where(x => x.ItemTradeable == true).Where(x => x.ItemBuyLimit != null).ToList();
            List<TradeVolume> existingTrades = _context.TradeVolume.ToList();

            string url; // OSRS GE  url
            var web = new HtmlWeb(); // website data object

            foreach(Item i in items)
            {
                List<TradeVolume> tradeVolumes = new List<TradeVolume>();
                string itemID = i.ItemID.ToString(); // Get itemID (OSRS ID)
                string itemName = i.ItemName.Replace(" ", "+"); // format item name to url compatible item name

                int c = 1; // counter variable - how many times to try to reach OSRS GE website?

                while (c <= 3) // countiue to attempt to query GE website until 3 attempts have been reached
                {
                    try
                    {

                        url = "http://services.runescape.com/m=itemdb_oldschool/" + itemName + "/viewitem?obj=" + itemID; // create GE URL

                        var doc = web.Load(url); // Load web data

                        var scriptNode = doc.DocumentNode.SelectSingleNode("/html/body/div/div/main/div[2]/script"); // Select script node with trade data values
                        string tradeVolumeText = scriptNode.InnerText; // Get text from within script node

                        using (StringReader reader = new StringReader(tradeVolumeText)) // string reader to loop through text lines
                        {
                            string line = string.Empty; // Create empty string that contains text of each line
                            do
                            {
                                line = reader.ReadLine();
                                if (line != null)
                                {
                                    if (line.Contains("trade180") && !line.Contains("[['Date','Daily','Average']]"))
                                    {
                                        line = line.Replace("\t", string.Empty);

                                        // Get date from string
                                        int volumeDateStartPos = line.LastIndexOf("Date('") + 6; // "6" removes "Date('"
                                        int volumeDateLength = line.IndexOf("'),") - volumeDateStartPos;
                                        string volumeDate = line.Substring(volumeDateStartPos, volumeDateLength);

                                        // Get trade volume amount from string
                                        int volumeAmountStartPos = line.LastIndexOf("'), ") + 4; // "4" removes "'), "
                                        int volumeAmountLength = line.IndexOf("]);") - volumeAmountStartPos;
                                        string volumeAmount = line.Substring(volumeAmountStartPos, volumeAmountLength);

                                        // Parse data into trade volume object
                                        TradeVolume tradeVolume = new TradeVolume();
                                        tradeVolume.ItemID = i.ID;
                                        tradeVolume.Time = Convert.ToDateTime(volumeDate);
                                        tradeVolume.NumberOfTrades = Convert.ToInt32(volumeAmount);

                                        tradeVolumes.Add(tradeVolume);
                                    }
                                }

                            }
                            while (line != null);
                        }

                        c = 4; // Set counter above threshold - succsessful GE query!

                    }
                    catch
                    {
                        Thread.Sleep(2000); // unable to reach GE website - wait
                        c++; // increment counter variable
                    }

                }



                // Add each trade record to database
                foreach(TradeVolume trade in tradeVolumes)
                {

                    // TODO - check it item trade data exists
                    if (existingTrades.Where(x => x.ItemID == i.ID).Where(x => x.Time == trade.Time).Any())
                    {
                        // record already exists - do nothing
                    }
                    else
                    {
                        _context.TradeVolume.Add(trade);

                    }


                }

                // Update database
                _context.SaveChanges();
            }


            //http://services.runescape.com/m=itemdb_oldschool/Ahrim%27s+hood/viewitem?obj=4708
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







