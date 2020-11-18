using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dotNet5781_02_7588_3756
{
    public static class ValidInput
    {
        static Random random = new Random();
        public static List<int> randomListStationKeys = new List<int>();

        public static int GetBusLineNumberUesr()
        {
            Console.WriteLine("enter bus line number");
            string buskey = Console.ReadLine();
            int res;
            if (!int.TryParse(buskey, out res) && buskey.Length > 3 && res >= 0)
            {
                Console.WriteLine("not valid bus line number");
                return -1;
            }
            return res;
        }

        public static int GetUniqueStationKey(string userOrRand = "rand")
        {
            if (userOrRand == "user")
            {
                Console.WriteLine("Enter station key");
                string sKey = Console.ReadLine();
                int res;
                if (!int.TryParse(sKey, out res) && sKey.Length != 6 && res >= 0)
                {
                    Console.WriteLine("not valid station key");
                    return -1;
                }
                return res; ;
            }

            int key = random.Next(100000, 999999);
            while (randomListStationKeys.Contains(key))
            {
                key = random.Next(100000, 999999);
                randomListStationKeys.Add(key);
            }
            return key;
        }

        public static bool GetLocation(out double latitude, out double longitude, string userOrRand = "rand")
        {
            if (userOrRand == "user")
            {
                return getLocationFromUser(out latitude, out longitude);
            }

            double xTemp = random.NextDouble();
            double yTemp = random.NextDouble();
            xTemp += (double)random.Next(33, 36);
            yTemp += (double)random.Next(29, 34);
            latitude = xTemp;
            longitude = yTemp;
            return true;
        }

        private static bool getLocationFromUser(out double latitude, out double longitude)
        {
            latitude = longitude = 0;
            Console.WriteLine("Enter latitude: ");
            string strTempX = Console.ReadLine();
            double x;
            if (!double.TryParse(strTempX, out x))
            {
                Console.WriteLine("not a valid number");
                return false;
            }

            Console.WriteLine("Enter longitude: ");
            string strTempY = Console.ReadLine();
            double y;
            if (!double.TryParse(strTempY, out y))
            {
                Console.WriteLine("not a valid number");
                return false;
            }
            
            if (!InRange(x, y, -90, 90, -180, 180))
            {
                Console.WriteLine("The location entered is outside the Earth");
                return false;
            }

            latitude = x;
            longitude = y;
            return true;
        }

        public static bool InRange(double param1, double param2, double x1, double y1, double x2, double y2)
        {
            return param1 >= x1 && param1 <= y1 && param2 >= x2 && param2 <= y2;
        }
    }
}
