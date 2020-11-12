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
        static List<int> randomListStationKeys = new List<int>();

        public static void InitializBusLines(BusLineCollection busCol, int numOfBusLines, int numOfStation, string userOrRand = "rand")
        {
            BusLineStation[] stationArr = getInitializStations(numOfStation, userOrRand);
            if (stationArr == null)
                return;
            BusLine[] busLineArr = new BusLine[numOfBusLines];
            for(int i = 0; i < numOfBusLines; i++)
            {
                BusLineStation first = new BusLineStation(stationArr[i % numOfStation]);
                BusLineStation last = new BusLineStation(stationArr[i+1 % numOfStation]);
                busLineArr[i] = new BusLine(random.Next(1, 1000) , first, last);
                busLineArr[i].TotalTimeOfTheLine = new
                    TimeSpan(random.Next(0, 24), random.Next(0, 60), 0);
            }
        }

        private static BusLineStation[] getInitializStations(int numOfStation, string userOrRand = "rand")
        {
            BusLineStation[] stationArr = new BusLineStation[numOfStation];
            for (int i = 0; i < numOfStation; i++)
            {
                string key = getUniqueStationKey(userOrRand);
                if (key == "m")
                    return null;
                double latitude, longitude;
                if (!getLocation(out latitude, out longitude, userOrRand))
                    return null;
                stationArr[i]  = new BusLineStation(key, latitude, longitude);
            }
            return stationArr;
        }


        private static bool getLocation(out double latitude, out double longitude, string userOrRand = "rand")
        {
            latitude = longitude = 0;
            if (userOrRand == "user")
            {
                Console.WriteLine("Enter location of the station");              
                
                Console.WriteLine("Enter latitude: ");
                string strTempX = Console.ReadLine();
                if (!validLocation(strTempX))
                    return false;
                
                Console.WriteLine("Enter longitude: ");
                string strTempY = Console.ReadLine();
                if (!validLocation(strTempX))
                    return false;

                if(InRange(double.Parse(strTempX), double.Parse(strTempY), -90, 90, -180, 180))
                {
                    Console.WriteLine("The location entered is outside the Earth");
                    return false;
                }

                latitude = double.Parse(strTempX);
                longitude = double.Parse(strTempY);
                return true;
            }

            double xTemp = random.NextDouble();
            double yTemp = random.NextDouble();
            xTemp += (double)random.Next(31, 33);
            yTemp += (double)random.Next(34, 35);
            latitude = xTemp;
            longitude = yTemp;
            return true;
        }

        private static bool validLocation(string coord)
        {
            bool flag = true;
            do
            {
                coord = Console.ReadLine();
                if (coord == "m")
                    return false;
                flag = validNumber(coord, "double");
                if (!flag)
                {
                    Console.WriteLine("not a valid number, enter new");
                    Console.WriteLine(@"to return the menu prass 'm'");
                }
            } while (!flag);
            return true;
        }

        public static bool InRange(double param1, double param2, double x1, double y1, double x2, double y2)
        {
            return param1 >= x1 && param1 <= y1 && param2 >= x2 && param2 <= y2;
        }

        private static string getUniqueStationKey(string userOrRand = "rand")
        {
            if(userOrRand == "user")
            {
                Console.WriteLine("Enter station key");
                bool flag = true;
                string sKey = "";
                do
                {
                    sKey = Console.ReadLine();
                    flag = validStationKey(sKey);
                    Console.WriteLine(@"to return the menu prass 'm'");
                    if (sKey == "m")
                        return "m";
                } while (!flag);
                return sKey;
            }
            
            int key = random.Next(100000, 999999);
            while(randomListStationKeys.Contains(key)) 
            {
                key = random.Next(100000, 999999);
                randomListStationKeys.Add(key);
            }
            return key.ToString();
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
                Console.WriteLine("not a valid number, enter new");
                return false;
            }
            if(randomListStationKeys.Contains(int.Parse(num)))
            {
                Console.WriteLine("Station key is already exist, enter new");
                return false;
            }
            return true;
        }

    }
}
