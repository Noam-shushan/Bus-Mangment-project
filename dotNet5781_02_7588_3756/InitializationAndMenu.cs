using System;
using System.Collections.Generic;
using System.Linq;


namespace dotNet5781_02_7588_3756
{
    static class InitializationAndMenu
    {
        static List<BusLineStation> allStation = new List<BusLineStation>();
        static Random random = new Random();

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
                        printTheFastLineBetweenStations(busCol);
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

        public static void InitializBusCollection(BusLineCollection busCol, int numOfBusLines, int numOfStation)
        {
            BusLineStation[] stationArr = getInitializStations(numOfStation);
            if (stationArr == null)
                return;

            BusLine[] busLineArr = new BusLine[numOfBusLines];
            int j = 0;
            for (int i = 0; i < numOfBusLines; i++, j += 2)
            {
                BusLineStation first = new BusLineStation(stationArr[j % numOfStation]);
                BusLineStation last = new BusLineStation(stationArr[j + 1 % numOfStation]);
                first.DistanceFromPrevStation = 0;
                first.TimeFromPrevStation = new TimeSpan();
                last.DistanceFromPrevStation = last.DistanceBetweenStations(first);
                busLineArr[i] = new BusLine(random.Next(1, 999), first, last);
                busLineArr[i].TotalTimeOfTheLine = new
                    TimeSpan(random.Next(0, 24), random.Next(0, 60), 0);
                last.TimeFromPrevStation = busLineArr[i].TotalTimeOfTheLine;
            }

            for (; j < numOfStation; j++)
            {
                busLineArr[random.Next(0, numOfBusLines)].AddStation(stationArr[j]);
            }

            busCol.BusLineList = busLineArr.ToList();
        }

        private static BusLineStation[] getInitializStations(int numOfStation)
        {
            if (numOfStation == 0)
                return null;
            BusLineStation[] stationArr = new BusLineStation[numOfStation];
            for (int i = 0; i < numOfStation; i++)
            {
                string key = ValidInput.GetUniqueStationKey();
                if (key == "m")
                    return null;
                double latitude, longitude;
                if (!ValidInput.GetLocation(out latitude, out longitude))
                    return null;
                stationArr[i] = new BusLineStation(key, latitude, longitude);
                allStation.Add(stationArr[i]);
            }
            return stationArr;
        }


        private static void printMe(BusLineCollection busCol)
        {
            Console.WriteLine("To print all buss enter: 1\n" +
                "To print all stations enter: 2\n");
            string c = Console.ReadLine();
            if (c == "1")
            {
                foreach (var b in busCol)
                    Console.WriteLine(b);
            }
            if (c == "2")
            {
                foreach (var s in allStation)
                    Console.WriteLine(s);
            }
        }

        private static void printTheFastLineBetweenStations(BusLineCollection busCol)
        {
            string stationKey1 = ValidInput.GetUniqueStationKey("user");
            if (stationKey1 == "Not valid")
                return;
            string stationKey2 = ValidInput.GetUniqueStationKey("user");
            if (stationKey2 == "Not valid")
                return;
            BusLineCollection outpot = new BusLineCollection();
            foreach (var busLine in busCol)
            {
                var s1 = busLine.GetStationByKey(stationKey1);
                var s2 = busLine.GetStationByKey(stationKey2);
                if (s1 != null && s2 != null)
                {
                    var temp = busLine.SubRouteBetweenStation(s1, s2);
                    if (temp != null)
                        outpot.AddBusLine(temp);
                }
            }

            var sortOutPot = outpot.SortBusLineList();
            foreach (var b in sortOutPot)
                Console.WriteLine(b);
        }

        private static void linsPassInStation(BusLineCollection busCol)
        {
            string stationKey = ValidInput.GetUniqueStationKey("user");
            if (stationKey == "Not valid")
                return;
            var busLines = busCol.GetListPassInStation(stationKey);
            Console.WriteLine($"Bus lines that pass through the station {stationKey} is:");
            foreach (var bus in busLines)
                Console.WriteLine(bus.BusLineNum);
        }

        private static void deletBusLineUser(BusLineCollection busCol)
        {
            Console.WriteLine("To remove bus line enter: 1\n" +
                    "to remove station from a bus line enter: 2");
            string c = Console.ReadLine();
            int buskey = ValidInput.GetBusLineNumberUesr();
            if (buskey == -1)
            {
                Console.WriteLine("not valid bus line");
                return;
            }
            if (c == "1")
            {
                var bus = busCol[buskey];
                if (busCol == null)
                {
                    Console.WriteLine("Bus not found!");
                    return;
                }
                busCol.RemoveBusLine(bus);
            }

            if (c == "2")
            {
                string stationKey = ValidInput.GetUniqueStationKey("user");
                if (stationKey == "Not valid")
                    return;

                var bus = busCol[buskey];
                bus.RemoveStation(bus.GetStationByKey(stationKey));
                allStation.Remove(bus.GetStationByKey(stationKey));
            }

        }

        private static void addBusLineUser(BusLineCollection busCol)
        {
            Console.WriteLine("To add bus line enter: 1\n" +
                "To add station to a bus line enter: 2\n" +
                "To add exist station to a bus line enter: 3");
            string c = Console.ReadLine();
            int buskey = ValidInput.GetBusLineNumberUesr();
            if (buskey == -1)
            {
                Console.WriteLine("not valid bus line");
                return;
            }
            var bus = busCol[buskey];
            if (bus == null)
            {
                Console.WriteLine("Bus not found!");
                return;
            }
            if (c == "3")
            {
                string stationKey3 = ValidInput.GetUniqueStationKey("user");
                if (stationKey3 == "Not valid")
                    return;
                if (checkIfStationExist(stationKey3))
                {
                    var st = allStation.Find(x => x.BusStationKey == stationKey3);
                    bus.AddStation(st);
                    Console.WriteLine($"Station number {stationKey3} Add successfully to bus number  {buskey}");
                    return;
                }

            }
            string stationKey1 = ValidInput.GetUniqueStationKey("user");
            if (stationKey1 == "Not valid")
                return;
            if(checkIfStationExist(stationKey1))
            {
                Console.WriteLine("Station key is already exist");
                return;
            }

            Console.WriteLine("Enter location of the station");
            double latitude1, longitude1, latitude2, longitude2;
            if (!ValidInput.GetLocation(out latitude1, out longitude1, "user"))
                return;

            var first = new BusLineStation(stationKey1, latitude1, longitude1);

            if (c == "1")
            {
                string stationKey2 = ValidInput.GetUniqueStationKey("user");
                if (stationKey2 == "Not valid")
                    return;
                if (checkIfStationExist(stationKey1))
                {
                    Console.WriteLine("Station key is already exist");
                    return;
                }
                Console.WriteLine("Enter location of the last station");
                if (!ValidInput.GetLocation(out latitude2, out longitude2, "user"))
                    return;
                var last = new BusLineStation(stationKey2, latitude2, longitude2);

                if (!busCol.AddBusLine(new BusLine(buskey, first, last)))
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
            if (c == "2")
            {
                bus.AddStation(first);
                Console.WriteLine($"Station number {stationKey1} Add successfully to bus number  {buskey}");
            }
        }

        private static bool checkIfStationExist(string key)
        {
            foreach(var s in allStation)
            {
                if(s.BusStationKey == key)
                    return true;
            }
            return false;
        }
    }

}
