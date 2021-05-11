using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Restaurant
{
    public class RestaurantPick
    {
        #region You_should_not_modify_this_region


        private class Restaurant
        {
            public int RestaurantId { get; set; }
            public Dictionary<string, decimal> Menu { get; set; }
        }

        private readonly List<Restaurant> _restaurants = new List<Restaurant>();

        /// <summary>
        /// Reads the file specified at the path and populates the restaurants list
        /// </summary>
        /// <param name="filePath">Path to the comma separated restuarant menu data</param>
        public void ReadRestaurantData(string filePath)
        {
            try
            {
                var records = File.ReadLines(filePath);

                foreach (var record in records)
                {
                    var data = record.Split(',');
                    var restaurantId = int.Parse(data[0].Trim());
                    var restaurant = _restaurants.Find(r => r.RestaurantId == restaurantId);

                    if (restaurant == null)
                    {
                        restaurant = new Restaurant { Menu = new Dictionary<string, decimal>() };
                        _restaurants.Add(restaurant);
                    }

                    restaurant.RestaurantId = restaurantId;
                    restaurant.Menu.Add(data.Skip(2).Select(s => s.Trim()).Aggregate((a, b) => a.Trim() + "," + b.Trim()), decimal.Parse(data[1].Trim()));
                }

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        static void Main(string[] args)
        {
            var restaurantPicker = new RestaurantPick();
            
            restaurantPicker.ReadRestaurantData(
                Path.GetFullPath(
                    Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"../../../../restaurant_data.csv")
                    )
                );

            // Item is found in restaurant 2 at price 6.50
            var bestRestaurant = restaurantPicker.PickBestRestaurant("gac");

            Console.WriteLine(bestRestaurant.Item1 + ", " + bestRestaurant.Item2);

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        #endregion



        #region You_can_modify_this_region
        /// <summary>
        /// Takes in items you would like to eat and returns the best restaurant that serves them.
        /// </summary>
        /// <param name="items">Items you would like to eat (seperated by ',')</param>
        /// <returns>Restaurant Id and price tuple</returns>
        public Tuple<int, decimal> PickBestRestaurant(string items)
        {   /*
             *
             * Put your solution here
             *
             *
             */

            string[] rs = items.Split(',');
            List<int> restList = new List<int>();


            //  1. Search through _restaurants and make list of places which have all items requested

            /*
             * The following method generates an int list of all restaurant ids which have all
             * requested items on the menu
             */

            foreach (Restaurant restaurant in _restaurants)
            {
                List<string> concat = new List<string>();
                int tally = 0;

                foreach (KeyValuePair<string, decimal> entry in restaurant.Menu)
                {
                    string[] s = entry.Key.ToString().Split(',');
                    foreach (string str in s)
                    {
                        concat.Add(str);
                    }
                }
                foreach (var item in rs)
                {
                    if (concat.Contains<string>(item)) 
                    {
                        tally++;
                    }
                }
                if (tally == rs.Length)
                {
                    restList.Add(restaurant.RestaurantId);
                }
            }



            //  2. Search through list and find cheapest combo of all items per restaurant

            /*
             * The following method clones the _restaurants list, and removes all entries which are not 
             * listed in the int list restList.
             * 
             * This leaves a new list, temp, which only includes Restaurant objects which contain all requested
             * items on the menu
             */

            if(restList.Count() == 0)
            {
                return null;
            }
            List<Restaurant> temp = _restaurants.ToList();
            
            foreach (Restaurant rest in temp.ToList()) 
            {
                if (!restList.Contains(rest.RestaurantId))
                {
                    temp.RemoveAll(temp => temp.RestaurantId == rest.RestaurantId );
                }
                

            }

          
            List<string> set = new List<string>();

            decimal price = 999;
            int id = 0;

            foreach (Restaurant rest in temp.ToList())
            {
                foreach (var menu in rest.Menu)
                {
                    foreach(var s in rs)
                    {
                        if (menu.Key.Contains(s))
                        {
                            set.Add(menu.Value + "," + menu.Key); // creates a list of menu items
                        }
                    }
                }
                List<string> distinct = new List<string>(set.Distinct());
                foreach(var item in distinct)
                {
                    Console.WriteLine(distinct);
                }

                //Console.WriteLine("filtered: " + distinct.Count() + "\t || non-filtered: " + set.Count());
                
                //var powerSet = GetPowerSet(distinct); // creates a powerset of the list of menu items

                //Console.WriteLine("Powerset size: " + powerSet.Count());

                //foreach (var list in powerSet) // search the powerset for a line which contains all items
                //{ 

                //    string str = String.Join(',', list.ToArray());
                //    bool contains = true;
                //    foreach (var s in rs)
                //    {
                //        if (!str.Contains(s))
                //        {
                //            contains = false;
                //        }
                //    }
                //    if (contains) // parse the price, and if it's less than current min price, swap 'em
                //    {
                //        string[] str2 = str.Split(',');
                //        decimal tempPrice = 0;
                //        decimal temp2 = 0;
                //        foreach (string index in str2)
                //        {
                //            if (decimal.TryParse(index, out tempPrice))
                //            {
                //                temp2 += decimal.Parse(index);
                //            }
                //        }
                //        if (temp2 < price)
                //        {
                //            price = temp2;
                //            id = rest.RestaurantId;
                //        }
                //    }
                //}
            }

            //  3. Return cheapest

            return new Tuple<int, decimal>(id, price);
        }

        /*
         * The below method was taken from discussions on MSDN.com related to LINQ power set
         * generation, original author unknown.
         */
        private IEnumerable<IEnumerable<T>> GetPowerSet<T>(List<T> list)
        {
            return from m in Enumerable.Range(0, 1 << list.Count)
                   select
                     from i in Enumerable.Range(0, list.Count)
                     where (m & (1 << i)) != 0 
                     select list[i];
        }
        #endregion
    }
}
