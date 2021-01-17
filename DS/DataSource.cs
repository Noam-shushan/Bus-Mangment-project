using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS
{
    public static class DataSource
    {
        public static List<DO.User> UsersList; // did
        public static List<DO.Bus> BussList; // did
        public static List<DO.Line> LinesList; // did
        public static List<DO.BusOnTrip> BusesOnTripList; 
        public static List<DO.AdjacentStations> AdjacentStationsList; // did
        public static List<DO.Station> StationsList; // did
        public static List<DO.LineStation> LineStationsList; // did
        public static List<DO.Trip> TripsList;
        public static List<DO.LineTrip> LineTripsList;

        static DataSource()
        {
            initAllLists();
        }

        static void initAllLists()
        {
            InitializStations.Initializ50Stations();
            //initializStations();
            initializLines();
            initializBuss();
            initializUsers();
            initializAdjacentStationsList();
        }

        static void initializUsers()
        {
            UsersList = new List<DO.User>()
            {
                new DO.User
                {
                    UserName = "noam",
                    Password = "1234",
                    Admin = true,
                    IsDeleted = false
                },
                new DO.User
                {
                    UserName = "David_Hamelch",
                    Password = "David1234",
                    Admin = false,
                    IsDeleted = false
                },
                new DO.User
                {
                    UserName = "Yossi_Sossi",
                    Password = "Yossi1234",
                    Admin = true,
                    IsDeleted = false
                },
                new DO.User
                {
                    UserName = "Honi_Hamehagel",
                    Password = "Honi1234",
                    Admin = false,
                    IsDeleted = false
                },
                new DO.User
                {
                    UserName = "Nach_Nachma_Nachman",
                    Password = "Breslev1234",
                    Admin = true,
                    IsDeleted = false
                },
                new DO.User
                {
                    UserName = "Rashbi-in-Mirom",
                    Password = "Rashbi1234",
                    Admin = false,
                    IsDeleted = false
                },
            };
        }

        //static void initializStations(int numOfStation = 100)
        //{
        //    StationsList = new List<DO.Station>();
        //    for (int i = 0; i < numOfStation; i++)
        //    {
        //        double latitude, longitude;
        //        RandomValues.getLocation(out latitude, out longitude);
        //        StationsList.Add(new DO.Station()
        //        {
        //            Code = RandomValues.getUniqueStationKey(),
        //            Latitude = latitude,
        //            Longitude = longitude,
        //            Area = RandomValues.getArea(longitude, latitude),
        //            IsDeleted = false
        //        });
        //    }
        //}

        static void initializLines(int numOfLins = 10)
        {
            LinesList = new List<DO.Line>();
            LineStationsList = new List<DO.LineStation>();
            for(int i = 0; i < numOfLins; i++)
            {
                var newLine = new DO.Line()
                {
                    Id = DalApi.Counters.LineCounter,
                    Code = RandomValues.getLineCode(),
                    IsDeleted = false
                };
                var firstAndLastStationCode = initializLineStations(newLine.Id);
                newLine.FirstStation = firstAndLastStationCode[0];
                newLine.LastStation = firstAndLastStationCode[1];
                newLine.Area = StationsList.Find(s => s.Code == newLine.FirstStation).Area;
                LinesList.Add(newLine);
            }
            foreach (var ls in LineStationsList)
            {
                ls.Name = StationsList.Find(s => s.Code == ls.Station).Name;
            }
        }

        static int[] initializLineStations(int lineId, int numOfLS = 10) 
        {
            List<DO.LineStation> stationTemp = new List<DO.LineStation>();
            var first = new DO.LineStation()
            {
                LineId = lineId,
                Station = RandomValues.getStation(stationTemp),
                LineStationIndex = 0,
                PrevStation = 0,
                IsDeleted = false
            };
            var area = StationsList.Find(s => s.Code == first.Station).Area;
            stationTemp.Add(first);
            for(int i = 1; i < numOfLS; i++)
            {
                stationTemp.Add(new DO.LineStation()
                {
                    LineId = lineId,
                    Station = RandomValues.getStation(stationTemp, area),
                    LineStationIndex = i,
                    PrevStation = stationTemp.ElementAt(i - 1).Station,
                    IsDeleted = false
                }) ;
            }
            foreach(var ls in stationTemp)
            {
                if (ls.LineStationIndex != stationTemp.Count-1)
                    ls.NextStation = stationTemp.ElementAt(ls.LineStationIndex + 1).Station;
                else
                    ls.NextStation = -1; // last station mark in -1
            }
            LineStationsList.AddRange(stationTemp);
            
            return new int[] {stationTemp.ElementAt(0).Station, 
                stationTemp.ElementAt(stationTemp.Count - 1).Station };
        }

        static void initializBuss(int numOfBuss = 20)
        {
            BussList = new List<DO.Bus>();
            for(int i = 0; i < numOfBuss; i++)
            {
                var dateFrom = RandomValues.randomDate();
                BussList.Add(new DO.Bus()
                {
                    FromDate = dateFrom,
                    LicenseNum = RandomValues.randomLicenseNumber(dateFrom),
                    Status = DO.BusStatus.READY,
                    TotalTrip = RandomValues.getKilometers(),
                    LastTreatment = RandomValues.randomDate(2018),
                    KilometersAfterFueling = RandomValues.getKilometers(0, DO.Bus.MAX_KILOMETER_AFTER_REFUELING),
                    KilometersAfterTreatment = RandomValues.getKilometers(0, DO.Bus.KILOMETER_BEFORE_TREATMENT),
                    FuelRemain = RandomValues.getFuelLiters(),
                    IsDeleted = false
                });
            }
        }

        static void initializAdjacentStationsList()
        {
            AdjacentStationsList = new List<DO.AdjacentStations>();
            foreach(var line in LinesList)
            {
                var temp = (from ls in LineStationsList
                           where ls.LineId == line.Id
                           select ls).ToList();
                foreach(var ls in temp)
                {
                    if (ls.NextStation == -1)
                        break; // last station in the line
                    
                    var station1 = StationsList.Find(s => s.Code == ls.Station);
                    var station2 = StationsList.Find(s => s.Code == ls.NextStation);
                    var dist = AuxiliaryFunctions.GetDisteance(station1.Latitude, station1.Longitude,
                        station2.Latitude, station2.Longitude);
                    AdjacentStationsList.Add(new DO.AdjacentStations()
                    {
                        Station1 = ls.Station,
                        Station2 = ls.NextStation,
                        LineCode = line.Code,
                        Distance = dist,
                        Time = AuxiliaryFunctions.GetTimeBetweenStations(dist),
                        IsDeleted = false
                    }) ;
                }
            }
        }
    }
}
