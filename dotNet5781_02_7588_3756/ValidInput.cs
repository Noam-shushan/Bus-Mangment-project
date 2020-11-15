using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace dotNet5781_02_7588_3756
{
    static class ValidInput
    {
        static Random random = new Random();
        public static List<int> randomListStationKeys = new List<int>();

        public static int GetBusLineNumberUesr()
        {
            Console.WriteLine("enter bus line number");
            string buskey = Console.ReadLine();
            if (!validNumber(buskey) || buskey.Length > 3)
            {
                Console.WriteLine("not valid bus line");
                return -1;
            }
            return int.Parse(buskey);
        }

        public static string GetUniqueStationKey(string userOrRand = "rand")
        {
            if (userOrRand == "user")
            {
                Console.WriteLine("Enter station key");
                string sKey = Console.ReadLine();
                if (!validStationKey(sKey))
                        return "Not valid";
                return sKey;
            }

            int key = random.Next(100000, 999999);
            while (randomListStationKeys.Contains(key))
            {
                key = random.Next(100000, 999999);
                randomListStationKeys.Add(key);
            }
            return key.ToString();
        }

        public static bool GetLocation(out double latitude, out double longitude, string userOrRand = "rand")
        {
            if (userOrRand == "user")
            {
                return getLocationFromUser(out latitude, out longitude);
            }

            double xTemp = random.NextDouble();
            double yTemp = random.NextDouble();
            xTemp += (double)random.Next(29, 33);
            yTemp += (double)random.Next(34, 35);
            latitude = xTemp;
            longitude = yTemp;
            return true;
        }

        private static bool getLocationFromUser(out double latitude, out double longitude)
        {
            latitude = longitude = 0;
            Console.WriteLine("Enter latitude: ");
            string strTempX = Console.ReadLine();
            if (!validNumber(strTempX, "double"))
            {
                Console.WriteLine("not a valid number");
                return false;
            }

            Console.WriteLine("Enter longitude: ");
            string strTempY = Console.ReadLine();
            if (!validNumber(strTempX, "double"))
            {
                Console.WriteLine("not a valid number");
                return false;
            }

            if (!InRange(double.Parse(strTempX), double.Parse(strTempY), -90, 90, -180, 180))
            {
                Console.WriteLine("The location entered is outside the Earth");
                return false;
            }

            latitude = double.Parse(strTempX);
            longitude = double.Parse(strTempY);
            return true;
        }

        public static bool InRange(double param1, double param2, double x1, double y1, double x2, double y2)
        {
            return param1 >= x1 && param1 <= y1 && param2 >= x2 && param2 <= y2;
        }


        private static bool validNumber(string num, string intOrDouble = "int")
        {
            string allowableLetters = intOrDouble == "int" ? "0123456789" : "0123456789.";
            foreach (char c in num)
            {
                if (!allowableLetters.Contains(c.ToString()))
                    return false;  
            }
            return true;
        }

        private static bool validStationKey(string num)
        {
            if(num.Length != 6 && !validNumber(num))
            {
                Console.WriteLine("not a valid number");
                return false;
            }
            return true;
        }

    }
}
