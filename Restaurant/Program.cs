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



            foreach (int rest in restList)
            {
                Console.WriteLine(rest);

            }

            //  2. Search through list and find cheapest combo of all items per restaurant

            if(restList.Count() == 0)
            {
                return null;
            }
            List<KeyValuePair<string, decimal>> menuList = new List<KeyValuePair<string, decimal>>();
            List<KeyValuePair<int, List<KeyValuePair<string, decimal>>>> associations = new List<KeyValuePair<int, List<KeyValuePair<string, decimal>>>>();
            foreach (int index in restList)
            {
                foreach (KeyValuePair<string, decimal> entry in _restaurants[index - 1].Menu)
                {
                    foreach (var item in rs)
                    {
                        if (entry.Key.Contains(item))
                        {
                            if (!menuList.Contains(entry))
                            {
                                menuList.Add(entry);
                                
                            }
                        }
                    }
                }
                associations.Add(new KeyValuePair<int, List<KeyValuePair<string, decimal>>>(index, menuList));

                Console.WriteLine(associations.Count());


                menuList.Clear();
            }


            //  3. Return cheapest








            return new Tuple<int, decimal>(0, 0);
        }

        #endregion
    }
}
