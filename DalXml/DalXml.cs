using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dal
{
    public class DalXml : DalApi.IDaL
    {
        #region singelton
        public static DalApi.IDaL Instance { get; } = new DalXml();
        
        DalXml() { }
        #endregion

        #region Paths
        string linesPath = @"LinesXml.xml";
        string adjacentStationsPath = @"AdjacentStationsXml.xml";
        string lineStationsPath = @"LineStationsXml.xml";
        string stationPath = @"StationsXml.xml";
        string bussPath = @"BussXml.xml";
        string usersPath = @"UserXml.xml";
        #endregion

        #region Bus
        public DO.Bus GetBus(int licenseNum)
        {
            XElement bussRootElem = XmlTools.LoadListFromXMLElement(bussPath);

            DO.Bus bus = (from b in bussRootElem.Elements()
                          where int.Parse(b.Element("LicenseNum").Value) == licenseNum
                          select new DO.Bus()
                          {
                              LicenseNum = int.Parse(b.Element("LicenseNum").Value),
                              FromDate = DateTime.Parse(b.Element("FromDate").Value),
                              FuelRemain = double.Parse(b.Element("FuelRemain").Value),
                              IsDeleted = bool.Parse(b.Element("IsDeleted").Value),
                              KilometersAfterFueling = double.Parse(b.Element("KilometersAfterFueling").Value),
                              KilometersAfterTreatment = double.Parse(b.Element("KilometersAfterTreatment").Value),
                              LastTreatment = DateTime.Parse(b.Element("LastTreatment").Value),
                              Status = (DO.BusStatus)Enum.Parse(typeof(DO.BusStatus), b.Element("Status").Value),
                              TotalTrip = double.Parse(b.Element("TotalTrip").Value)
                          }
                        ).FirstOrDefault();

            if (bus != null && !bus.IsDeleted)
                return bus;
            else
                throw new DO.BadBusException(licenseNum, "bus not found");
        }

        public void AddBus(DO.Bus bus)
        {
            XElement bussRootElem;
            try
            {
                bussRootElem = XmlTools.LoadListFromXMLElement(bussPath);
            }
            catch (DO.XmlFileLoadCreateException ex)
            {
                throw ex;
            }
            var temp = (from b in bussRootElem.Elements()
                        where b.Element("LicenseNum").Value == bus.LicenseNum.ToString()
                        select b).FirstOrDefault();
            if (temp != null)
            {
                if (!bool.Parse(temp.Element("IsDeleted").Value))
                    throw new DO.BadBusException(bus.LicenseNum, "Duplicate License number of bus");
            }

            bussRootElem.Add(XmlTools.CreateElement(bus));
            XmlTools.SaveListToXMLElement(bussRootElem, bussPath);
        }

        public IEnumerable<DO.Bus> GetAllBuss()
        {
            var bussRootElem = XmlTools.LoadListFromXMLElement(bussPath);
            return from b in bussRootElem.Elements()
                   where b.Element("IsDeleted").Value == false.ToString()
                   select new DO.Bus()
                   {
                       LicenseNum = int.Parse(b.Element("LicenseNum").Value),
                       FromDate = DateTime.Parse(b.Element("FromDate").Value),
                       FuelRemain = double.Parse(b.Element("FuelRemain").Value),
                       IsDeleted = bool.Parse(b.Element("IsDeleted").Value),
                       KilometersAfterFueling = double.Parse(b.Element("KilometersAfterFueling").Value),
                       KilometersAfterTreatment = double.Parse(b.Element("KilometersAfterTreatment").Value),
                       LastTreatment = DateTime.Parse(b.Element("LastTreatment").Value),
                       Status = (DO.BusStatus)Enum.Parse(typeof(DO.BusStatus), b.Element("Status").Value),
                       TotalTrip = double.Parse(b.Element("TotalTrip").Value)
                   };
        }

        public IEnumerable<DO.Bus> GetAllBussBy(Predicate<DO.Bus> predicate)
        {
            return from b in GetAllBuss()
                   where predicate(b)
                   select b;
        }

        public void RemoveBus(DO.Bus bus)
        {
            XElement bussRootElem = XmlTools.LoadListFromXMLElement(bussPath);

            var busNode = (from b in bussRootElem.Elements()
                           where b.Element("LicenseNum").Value == bus.LicenseNum.ToString()
                           select b).FirstOrDefault();

            if (busNode == null)
                throw new DO.BadBusException(0, "bus not found");
            if (busNode.Element("IsDeleted").Value == true.ToString())
                throw new DO.BadBusException(bus.LicenseNum, "bus not found");

            busNode.Element("IsDeleted").SetValue(true);
            XmlTools.SaveListToXMLElement(bussRootElem, bussPath);
        }

        public void UpdateBus(DO.Bus bus)
        {
            XElement bussRootElem = XmlTools.LoadListFromXMLElement(bussPath);

            var busNode = (from b in bussRootElem.Elements()
                           where b.Element("LicenseNum").Value == bus.LicenseNum.ToString()
                           select b).FirstOrDefault();

            if (busNode == null)
                throw new DO.BadBusException(0, "bus not found");
            if (busNode.Element("IsDeleted").Value == true.ToString())
                throw new DO.BadBusException(bus.LicenseNum, "bus not found");

            busNode.UpdateElement(bus);
            XmlTools.SaveListToXMLElement(bussRootElem, bussPath);
        }
        #endregion

        #region Line
        public IEnumerable<DO.Line> GetAllLines()
        {
            var lineList = XmlTools.LoadListFromXMLSerializer<DO.Line>(linesPath);
            return from line in lineList
                   where !line.IsDeleted
                   select line;
        }

        public IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate)
        {
            var lineList = XmlTools.LoadListFromXMLSerializer<DO.Line>(linesPath);
            return from line in lineList
                   where predicate(line) && !line.IsDeleted
                   select line;
        }

        public DO.Line GetLine(int lineId)
        {
            var lineList = XmlTools.LoadListFromXMLSerializer<DO.Line>(linesPath);
            var line = lineList.Find(l => l.Id == lineId);

            if (line != null && !line.IsDeleted)
                return line;
            else
                throw new DO.BadLineException(lineId, "Line not found");
        }

        public int AddLine(DO.Line line)
        {
            line.Id = DalApi.Counters.LineCounter;
            
            var lineList = XmlTools.LoadListFromXMLSerializer<DO.Line>(linesPath);
            lineList.Add(line);
            XmlTools.SaveListToXMLSerializer(lineList, linesPath);
            
            return line.Id;
        }

        public void UpdateLine(DO.Line line)
        {
            var lineList = XmlTools.LoadListFromXMLSerializer<DO.Line>(linesPath);
            var lineToUp = lineList.Find(l => l.Id == line.Id);

            if (lineToUp != null && !lineToUp.IsDeleted)
            {
                lineList.Remove(lineToUp);
                lineList.Add(line);
                XmlTools.SaveListToXMLSerializer(lineList, linesPath);
            }             
            else
                throw new DO.BadLineException(line.Id, "Line not found");
        }

        public void RemoveLine(DO.Line line)
        {
            var lineList = XmlTools.LoadListFromXMLSerializer<DO.Line>(linesPath);
            var lineToRem = lineList.Find(l => l.Id == line.Id);

            if (lineToRem != null && !lineToRem.IsDeleted)
            {
                lineList.Remove(lineToRem);
                GetAllLineStationBy(ls => ls.LineId == lineToRem.Id).ToList().ForEach(RemoveLineStation);
                lineToRem.IsDeleted = true;
                lineList.Add(lineToRem);
                XmlTools.SaveListToXMLSerializer(lineList, linesPath);
            }
            else
                throw new DO.BadLineException(line.Id, "Line not found");
        }
        #endregion

        #region Station
        public IEnumerable<DO.Station> GetAllStations()
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            return from station in stationList
                   where !station.IsDeleted
                   select station;
        }

        public IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate)
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            return from station in stationList
                   where predicate(station) && !station.IsDeleted
                   select station;
        }

        public DO.Station GetStation(int code)
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            
            var station = stationList.Find(s => s.Code == code);
            if (station != null && !station.IsDeleted)
                return station;
            else
                throw new DO.BadStationException(code, "Station not found");
        }

        public void AddStation(DO.Station station)
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            var st = stationList.Find(s => s.Code == station.Code);
            if (st != null && !station.IsDeleted)
                throw new DO.BadStationException(station.Code, "Duplicate Station");
            
            stationList.Add(station);
            XmlTools.SaveListToXMLSerializer(stationList, stationPath);
        }

        public void UpdateStation(DO.Station station)
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            var stationToUp = stationList.Find(s => s.Code == station.Code);

            if (stationToUp != null && !stationToUp.IsDeleted)
            {
                stationList.Remove(stationToUp);
                stationList.Add(station);
                XmlTools.SaveListToXMLSerializer(stationList, stationPath);
            }
            else
                throw new DO.BadStationException(station.Code, "Station not found");
        }

        public void RemoveStation(DO.Station station)
        {
            var stationList = XmlTools.LoadListFromXMLSerializer<DO.Station>(stationPath);
            var stationToRem = stationList.Find(s => s.Code == station.Code);

            if (stationToRem != null && !stationToRem.IsDeleted)
            {
                stationList.Remove(stationToRem);
                
                GetAllLineStationBy(s => s.Station == stationToRem.Code).ToList().ForEach(RemoveLineStation);
                GetAllAdjacentStationsBy(s => s.Station1 == stationToRem.Code
                || s.Station2 == stationToRem.Code).ToList().ForEach(RemoveAdjacentStations);
                
                stationToRem.IsDeleted = true;
                stationList.Add(stationToRem);
                XmlTools.SaveListToXMLSerializer(stationList, stationPath);
            }
            else
                throw new DO.BadStationException(station.Code, "Station not found");
        }
        #endregion

        #region LineStation
        public IEnumerable<DO.LineStation> GetAllLineStation()
        {
            var lineStationList = XmlTools.LoadListFromXMLSerializer<DO.LineStation>(lineStationsPath);
            return from lineStation in lineStationList
                   where !lineStation.IsDeleted
                   select lineStation;
        }

        public IEnumerable<DO.LineStation> GetAllLineStationBy(Predicate<DO.LineStation> predicate)
        {
            var lineStationList = XmlTools.LoadListFromXMLSerializer<DO.LineStation>(lineStationsPath);
            return from lineStation in lineStationList
                   where predicate(lineStation) && !lineStation.IsDeleted
                   orderby lineStation.LineId, lineStation.LineStationIndex
                   select lineStation;
        }

        public DO.LineStation GetLineStation(int stationCode, int lineId)
        {
            var lineStationList = XmlTools.LoadListFromXMLSerializer<DO.LineStation>(lineStationsPath);
            var lineStation =
                lineStationList.Find(s => s.Station == stationCode && s.LineId == lineId);

            if (lineStation != null && !lineStation.IsDeleted)
                return lineStation;
            else
                throw new DO.BadLineStationException(stationCode, lineId, "Station Line not found");
        }

        public void AddLineStation(DO.LineStation lineStation)
        {
            var lineStationList = XmlTools.LoadListFromXMLSerializer<DO.LineStation>(lineStationsPath);
            var ls = lineStationList.FirstOrDefault(s =>
             s.Station == lineStation.Station && s.LineId == lineStation.LineId);
            if(ls != null && !ls.IsDeleted)
            {
                throw new DO.BadLineStationException(lineStation.Station, lineStation.LineId,
                                                    "Duplicate line station");
            }

            var line = GetLine(lineStation.LineId);
            foreach (var s in lineStationList)
            {
                if(s.LineId == line.Id)
                {
                    if (lineStation.PrevStation == s.Station)
                    {
                        lineStation.LineStationIndex = s.LineStationIndex + 1;
                        continue;
                    }
                    if (lineStation.LineStationIndex != -1)
                    {
                        s.LineStationIndex += 1;
                        UpdateLineStation(s);
                    }
                }
            }
            var station = GetStation(lineStation.Station);
            lineStation.Name = station.Name;
            lineStationList.Add(lineStation);
            XmlTools.SaveListToXMLSerializer(lineStationList, lineStationsPath);
        }

        public void UpdateLineStation(DO.LineStation lineStation)
        {
            var lineStationList = XmlTools.LoadListFromXMLSerializer<DO.LineStation>(lineStationsPath);
            var ls = lineStationList.Find(s => s.Station == lineStation.Station
                                            && s.LineId == lineStation.LineId);

            if (ls != null && !ls.IsDeleted)
            {
                lineStationList.Remove(ls);
                lineStationList.Add(lineStation);
                XmlTools.SaveListToXMLSerializer(lineStationList, lineStationsPath);
            }
            else
                throw new DO.BadLineStationException(lineStation.Station,
                                                lineStation.LineId, "Station line not found");
        }

        public void RemoveLineStation(DO.LineStation lineStation)
        {
            var lineStationList = XmlTools.LoadListFromXMLSerializer<DO.LineStation>(lineStationsPath);
            var ls = lineStationList.Find(s => s.Station == lineStation.Station
                                            && s.LineId == lineStation.LineId);

            if (ls != null && !ls.IsDeleted)
            {
                foreach (var lineS in GetAllLineStationBy(s => s.LineId == ls.LineId
                    && s.LineStationIndex > ls.LineStationIndex))
                {
                    lineS.LineStationIndex -= 1;
                    UpdateLineStation(lineS);
                }
                lineStationList.Remove(ls);
                ls.IsDeleted = true;
                lineStationList.Add(ls);
                XmlTools.SaveListToXMLSerializer(lineStationList, lineStationsPath);
            }
            else
                throw new DO.BadLineStationException(lineStation.Station,
                                                lineStation.LineId, "Station line not found");
        }
        #endregion

        #region AdjacentStations
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStations()
        {
            var adjacentStationsList = XmlTools.LoadListFromXMLSerializer<DO.AdjacentStations>(adjacentStationsPath);
            return from adst in adjacentStationsList
                   where !adst.IsDeleted
                   select adst;
        }

        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate)
        {
            var adjacentStationsList = XmlTools.LoadListFromXMLSerializer<DO.AdjacentStations>(adjacentStationsPath);
            return from adst in adjacentStationsList
                   where !adst.IsDeleted && predicate(adst)
                   select adst;
        }

        public DO.AdjacentStations GetAdjacentStations(int station1, int station2)
        {
            var adjacentStationsList = XmlTools.LoadListFromXMLSerializer<DO.AdjacentStations>(adjacentStationsPath);
            var adst = adjacentStationsList.Find(s =>
                                        s.Station1 == station1 && s.Station2 == station2);
            if (adst != null && !adst.IsDeleted)
            {
                return adst;
            }
            throw new DO.BadAdjacentStationsException(station1, station2,
                    "Adjacent stations not found");
        }

        public void AddAdjacentStations(DO.AdjacentStations adjacentStations)
        {
            var adjacentStationsList = XmlTools.LoadListFromXMLSerializer<DO.AdjacentStations>(adjacentStationsPath);
            var adst = adjacentStationsList.Find(s =>
            s.Station1 == adjacentStations.Station1 && s.Station2 == adjacentStations.Station2);
            if (adst != null)
            {
                if (!adst.IsDeleted)
                    throw new DO.BadAdjacentStationsException(adjacentStations.Station1, adjacentStations.Station2,
                       "Duplicate adjacent stations");
            }
            adjacentStationsList.Add(adjacentStations);
            XmlTools.SaveListToXMLSerializer(adjacentStationsList, adjacentStationsPath);
        }

        public void RemoveAdjacentStations(DO.AdjacentStations adjacentStations)
        {
            var adjacentStationsList = XmlTools.LoadListFromXMLSerializer<DO.AdjacentStations>(adjacentStationsPath);
            var adst =
                adjacentStationsList.Find(s => s.Station1 == adjacentStations.Station1
                && s.Station2 == adjacentStations.Station2);

            if (adst != null && !adst.IsDeleted)
            {
                adjacentStationsList.Remove(adst);
                adst.IsDeleted = true;
                adjacentStationsList.Add(adst);
                XmlTools.SaveListToXMLSerializer(adjacentStationsList, adjacentStationsPath);
            }
                
            else
                throw new DO.BadAdjacentStationsException(adjacentStations.Station1, adjacentStations.Station2
                    , "Adjacent Stations not found");
        }

        public void UpdateAdjacentStations(DO.AdjacentStations adjacentStations)
        {
            var adjacentStationsList = XmlTools.LoadListFromXMLSerializer<DO.AdjacentStations>(adjacentStationsPath);
            var adst =
               adjacentStationsList.Find(s => s.Station1 == adjacentStations.Station1
                && s.Station2 == adjacentStations.Station2);

            if (adst != null && !adst.IsDeleted)
            {
                adjacentStationsList.Remove(adst);
                adjacentStationsList.Add(adjacentStations);
                XmlTools.SaveListToXMLSerializer(adjacentStationsList, adjacentStationsPath);
            }
            else
                throw new DO.BadAdjacentStationsException(adjacentStations.Station1, adjacentStations.Station2
                    , "Adjacent Stations not found");
        }
        #endregion

        #region User
        public IEnumerable<DO.User> GetAllUsers()
        {
            var userList = XmlTools.LoadListFromXMLSerializer<DO.User>(usersPath);
            return from user in userList
                   where !user.IsDeleted
                   select user;
        }

        public IEnumerable<DO.User> GetAllUsersBy(Predicate<DO.User> predicate)
        {
            var userList = XmlTools.LoadListFromXMLSerializer<DO.User>(usersPath);
            return from user in userList
                   where predicate(user) && !user.IsDeleted
                   select user;
        }

        public DO.User GetUser(string userName)
        {
            var userList = XmlTools.LoadListFromXMLSerializer<DO.User>(usersPath);
            var user = userList.Find(u => u.UserName == userName);

            if (user != null && !user.IsDeleted)
                return user;
            else
                throw new DO.BadUsernameException(userName, $"User not found {userName}");
        }

        public void AddUser(DO.User newUser)
        {
            var userList = XmlTools.LoadListFromXMLSerializer<DO.User>(usersPath);
            var user = userList.FirstOrDefault(u => u.UserName == newUser.UserName);
            if(user != null && !newUser.IsDeleted)
                throw new DO.BadUsernameException(newUser.UserName, "Duplicate user username");

            userList.Add(newUser);
            XmlTools.SaveListToXMLSerializer(userList, usersPath);
        }

        public void UpdateUser(DO.User user)
        {
            var userList = XmlTools.LoadListFromXMLSerializer<DO.User>(usersPath);
            var findUser = userList.Find(u => u.UserName == user.UserName);

            if (findUser != null && !findUser.IsDeleted)
            {
                userList.Remove(findUser);
                userList.Add(user);
                XmlTools.SaveListToXMLSerializer(userList, usersPath);
            }
            else
                throw new DO.BadUsernameException(user.UserName, $"User not found {user.UserName}");
        }

        public void RemoveUser(DO.User user)
        {
            var userList = XmlTools.LoadListFromXMLSerializer<DO.User>(usersPath);
            var findUser = userList.Find(u => u.UserName == user.UserName);

            if (findUser != null && !findUser.IsDeleted)
            {
                userList.Remove(findUser);
                findUser.IsDeleted = true;
                userList.Add(findUser);
                XmlTools.SaveListToXMLSerializer(userList, usersPath);
            }
            else
                throw new DO.BadUsernameException(user.UserName, $"User not found {user.UserName}");
        }
        #endregion
    }
}
