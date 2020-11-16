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
                int key = ValidInput.GetUniqueStationKey();
                if (key == -1)
                    return null;
                double latitude, longitude;
                if (!ValidInput.GetLocation(out latitude, out longitude))
                    return null;
                stationArr[i] = new BusLineStation(key, latitude, longitude);
                allStation.Add(stationArr[i]);
            }
            return stationArr;
        }


        private static void printMe(BusLineCollection busColl)
        {
            Console.WriteLine("To print all buss enter: 1\n" +
                "To print all stations of bus line enter: 2\n");
            string c = Console.ReadLine();
            if (c == "1")
            {
                foreach (var b in busColl)
                    Console.WriteLine(b);
            }
            if (c == "2")
            {
                int busKey = ValidInput.GetBusLineNumberUesr();
                if (busKey == -1)
                    return;

                try
                {
                    busColl[busKey].PrintStationInfo();
                }
                catch (Exception)
                {
                    Console.WriteLine("bus not found");
                }
            }
        }

        private static void printTheFastLineBetweenStations(BusLineCollection busCol)
        {
            int stationKey1 = ValidInput.GetUniqueStationKey("user");
            if (stationKey1 == -1)
                return;
            int stationKey2 = ValidInput.GetUniqueStationKey("user");
            if (stationKey2 == -1)
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

            var sortOutPut = outpot.SortBusLineList();
            foreach (var b in sortOutPut)
                Console.WriteLine(b);
        }

        private static void linsPassInStation(BusLineCollection busCol)
        {
            int stationKey = ValidInput.GetUniqueStationKey("user");
            if (stationKey == -1)
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
            int busKey = ValidInput.GetBusLineNumberUesr();
            if (busKey == -1)
                return;
            
            if (c == "1")
            {
                busCol.RemoveBusLine(busCol[busKey]);
            }

            if (c == "2")
            {
                int stationKey = ValidInput.GetUniqueStationKey("user");
                if (stationKey == -1)
                    return;

                busCol[busKey].RemoveStation(busCol[busKey].GetStationByKey(stationKey));
                allStation.Remove(busCol[busKey].GetStationByKey(stationKey));
            }

        }

        private static void addBusLineUser(BusLineCollection busCol)
        {
            Console.WriteLine("To add bus line enter: 1\n" +
                "To add station to a bus line enter: 2\n" +
                "To add exist station to a bus line enter: 3");
            string c = Console.ReadLine();
 
            int busKey = ValidInput.GetBusLineNumberUesr();
            if (busKey == -1)
                return;

            int stationKey1 = ValidInput.GetUniqueStationKey("user");
            if (stationKey1 == -1)
                return;

            if (checkIfStationExist(stationKey1) != null)
            {
                Console.WriteLine("Station key is already exist");
                return;
            }
            Console.WriteLine("Enter location of the station");
            double latitude, longitude;
            if (!ValidInput.GetLocation(out latitude, out longitude, "user"))
                return;

            BusLineStation firstStation = new BusLineStation(stationKey1, latitude, longitude);
            
            if (c == "1")
            {
                addNewBusToColl(busCol, busKey, firstStation);
                return;
            }
            if (c == "2")
            {
                addStationToBusLine(busCol, busKey, firstStation);
                return;
            }
            if (c == "3")
                addExistStation(busCol, busKey);
        }

        private static void addNewBusToColl(BusLineCollection busCol, int busKey, BusLineStation firstStation)
        {

            Console.WriteLine("enter last station");
            int stationKey = ValidInput.GetUniqueStationKey("user");
            if (stationKey == -1)
                return;
            
            if (checkIfStationExist(stationKey) != null)
            {
                Console.WriteLine("Station key is already exist");
                return;
            }
            
            double latitude, longitude;
            Console.WriteLine("Enter location of the last station");
            if (!ValidInput.GetLocation(out latitude, out longitude, "user"))
                return;
            
            BusLineStation lastStation = new BusLineStation(stationKey, latitude, longitude);

            if (!busCol.AddBusLine(new BusLine(busKey, firstStation, lastStation)))
            {
                Console.WriteLine("Error: This bus exists in the system");
                return;
            }
            else
            {
                Console.WriteLine($"Bus number {busKey} Add successfully");
                allStation.Add(firstStation);
                allStation.Add(lastStation);
            }
        }

        private static void addStationToBusLine(BusLineCollection busCol, int busKey, BusLineStation firstStation)
        {
            busCol[busKey].AddStation(firstStation);
            Console.WriteLine($"Station number {firstStation.BusStationKey} Add successfully to bus number  {busKey}");
        }

        private static void addExistStation(BusLineCollection busCol, int busKey)
        {

            int stationKey = ValidInput.GetUniqueStationKey("user");
            if (stationKey == -1)
                return;
            BusLineStation st;
            if ((st = checkIfStationExist(stationKey)) != null)
            {
                busCol[busKey].AddStation(st);
                Console.WriteLine($"Station number {stationKey} Add successfully to bus number  {busKey}");
                return;
            }
        }

        private static BusLineStation checkIfStationExist(int key)
        {
            return allStation.Find(x => x.BusStationKey == key);
        }
    }

}
