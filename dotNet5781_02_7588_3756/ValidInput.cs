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
        static List<BusLineStation> allStation = new List<BusLineStation>();

        public static void Menu(BusLineCollection busCol)
        {
            Console.WriteLine("Welcome!\n" +
                "To add a bus line, or station to a bus line, enter: 1\n" +
                "Delete a bus line or delete a station from the bus line, enter: 2\n" +
                "Look for lines that pass through a station, enter: 3\n" +
                "Printing the options for travel between 2 stations, without changing buses, enter 4\n" +
                "To print All bus lines in the system, enter: 5\n" +
                "To exit enter: 6\n");
            
            while (true)
            {
                Console.Write(" -> ");
                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        addBusLineUser(busCol);
                        break;
                    case "2":
                        deletBusLineUser(busCol);
                        break;
                    case "3":
                        linsPassInStation(busCol);
                        break;
                    case "4":
                        case4(busCol);
                        break;
                    case "5":
                        printMe(busCol);
                        break;
                    case "6":
                        Console.WriteLine("bye!");
                        return;
                    default:
                        break;
                }
            }
        }

        private static void printMe(BusLineCollection busCol)
        {
            Console.WriteLine("To print all buss enter: 1\n" +
                "To print all stations enter: 2\n");
            string c = Console.ReadLine();
            if(c == "1")
            {
                foreach (var b in busCol)
                    Console.WriteLine(b);
            }
            if(c == "2")
            {
                foreach(var s in allStation)
                    Console.WriteLine(s);
            }
        }

        private static void case4(BusLineCollection busCol)
        {
            string stationKey1 = getUniqueStationKey("user");
            if (stationKey1 == "m")
                return;
            string stationKey2 = getUniqueStationKey("user");
            if (stationKey2 == "m")
                return;
            BusLineCollection outpot = new BusLineCollection();
            foreach (var busLine in busCol)
            {
                var s1 = busLine.GetStationByKey(stationKey1);
                var s2 = busLine.GetStationByKey(stationKey2);
                if(s1 != null && s2 != null)
                {
                    var temp = busLine.SubRouteBeteenStation(s1, s2);
                    if (temp != null)
                        outpot.AddBusLine(temp);
                }
            }
            
            var sortOutPot = outpot.SortBusLineList();
            foreach(var b in sortOutPot)
                Console.WriteLine(b);
        }

        private static void linsPassInStation(BusLineCollection busCol)
        {
            string stationKey = getUniqueStationKey("user");
            if (stationKey == "m")
                return;
            var bl = busCol.GetListPassInStation(stationKey);
            
            foreach(var b in bl)
                Console.WriteLine(b);
        }

        private static void deletBusLineUser(BusLineCollection busCol)
        {
            Console.WriteLine("To remove bus line enter: 1\n" +
                    "to remove station from a bus line enter: 2");
            string c = Console.ReadLine();
            int buskey = getBusLineNumberUesr();
            if (buskey == -1)
            {
                Console.WriteLine("not valid bus line");
                return;
            }
            if (c == "1")
            {
                var bus = busCol[buskey];
                if(busCol == null)
                {
                    Console.WriteLine("Bus not found!");
                    return;
                }
                busCol.RemoveBusLine(bus);
            }
            
            if(c == "2")
            {
                string stationKey = getUniqueStationKey("user");
                if (stationKey == "m")
                    return;
                
                var bus = busCol[buskey];
                bus.RemoveStation(bus.GetStationByKey(stationKey));
                allStation.Remove(bus.GetStationByKey(stationKey));
            }

        }

        private static void addBusLineUser(BusLineCollection busCol)
        {
            Console.WriteLine("To add bus line enter: 1\n" +
                "to add station to a bus line enter: 2");
            string c = Console.ReadLine();
            int buskey = getBusLineNumberUesr();
            if (buskey == -1)
            {
                Console.WriteLine("not valid bus line");
                return;
            }
            string stationKey1 = getUniqueStationKey("user");
            if (stationKey1 == "m")
                return;

            Console.WriteLine("Enter location of the station");
            double latitude1, longitude1, latitude2, longitude2;
            if (!getLocation(out latitude1, out longitude1, "user"))
                return;
            
            var first = new BusLineStation(stationKey1, latitude1, longitude1);

            if (c == "1")
            {
                string stationKey2 = getUniqueStationKey("user");
                if (stationKey2 == "m")
                    return;
                Console.WriteLine("Enter location of the last station");
                if (!getLocation(out latitude2, out longitude2, "user"))
                    return;
                var last = new BusLineStation(stationKey2, latitude2, longitude2);

                if(!busCol.AddBusLine(new BusLine(buskey, first, last)))
                {
                    Console.WriteLine("Error: This bus exists in the system");
                    return;
                }
                else
                {
                    Console.WriteLine($"Bus number {buskey} Add successfully");
                    allStation.Add(first);
                    allStation.Add(last);
                }

            }
            if(c == "2")
            {
                var bus = busCol[buskey];
                if(bus == null)
                {
                    Console.WriteLine("Bus not found!");
                    return;
                }
                bus.AddStation(first);
                Console.WriteLine($"Station number {stationKey1} Add successfully to bus number  {buskey}");
            }
        }

        public static void InitializBusCollection(BusLineCollection busCol, int numOfBusLines, int numOfStation, string userOrRand = "rand")
        {
            BusLineStation[] stationArr = getInitializStations(numOfStation, userOrRand);
            if (stationArr == null)
                return;
            
            BusLine[] busLineArr = new BusLine[numOfBusLines];
            int j = 0;
            for(int i = 0; i < numOfBusLines; i++, j += 2)
            {
                BusLineStation first = new BusLineStation(stationArr[j % numOfStation]);
                BusLineStation last = new BusLineStation(stationArr[j+1 % numOfStation]);
                first.DistanceFromPrevStation = 0;
                first.TimeFromPrevStation = new TimeSpan();
                last.DistanceFromPrevStation = last.DistanceBetweenStations(first);
                busLineArr[i] = new BusLine(random.Next(1, 999) , first, last);
                busLineArr[i].TotalTimeOfTheLine = new
                    TimeSpan(random.Next(0, 24), random.Next(0, 60), 0);
                last.TimeFromPrevStation = busLineArr[i].TotalTimeOfTheLine;
            }

            for(; j < numOfStation; j++)
            {
                busLineArr[random.Next(0, numOfBusLines)].AddStation(stationArr[j]);
            }

            busCol.BusLineList = busLineArr.ToList();
        }

        private static BusLineStation[] getInitializStations(int numOfStation, string userOrRand = "rand")
        {
            if (numOfStation == 0)
                return null;
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
                allStation.Add(stationArr[i]);
            }
            return stationArr;
        }

        private static int getBusLineNumberUesr()
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

        private static bool getLocation(out double latitude, out double longitude, string userOrRand = "rand")
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
            if (!validLocation(strTempX))
                return false;

            Console.WriteLine("Enter longitude: ");
            string strTempY = Console.ReadLine();
            if (!validLocation(strTempX))
                return false;

            if (!InRange(double.Parse(strTempX), double.Parse(strTempY), -90, 90, -180, 180))
            {
                Console.WriteLine("The location entered is outside the Earth");
                return false;
            }

            latitude = double.Parse(strTempX);
            longitude = double.Parse(strTempY);
            return true;
        }

        private static bool validLocation(string coord)
        {
            bool flag = true;
            do
            {
                flag = validNumber(coord, "double");
                if (!flag)
                {
                    Console.WriteLine("not a valid number, enter new");
                    Console.WriteLine(@"to return the menu prass 'm'");
                    coord = Console.ReadLine();
                }
                if (coord == "m")
                    return false;
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
                string sKey = Console.ReadLine();
                while(!validStationKey(sKey, userOrRand))
                {
                    Console.WriteLine(@"to return the menu prass 'm'");
                    sKey = Console.ReadLine();
                    if (sKey == "m")
                        return "m";
                }
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

        private static bool validStationKey(string num, string userOrRand = "rand")
        {
            if(num.Length != 6 && !validNumber(num))
            {
                Console.WriteLine("not a valid number, enter new");
                return false;
            }
            if(randomListStationKeys.Contains(int.Parse(num)) && userOrRand == "rand")
            {
                Console.WriteLine("Station key is already exist, enter new");
                return false;
            }
            return true;
        }

    }
}
