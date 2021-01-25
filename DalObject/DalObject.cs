using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class DalObject : DalApi.IDaL
    {
        #region singelton
        public static DalApi.IDaL Instance { get; } = new DalObject();
        
        DalObject() { } 
        #endregion

        #region Bus
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        public void AddBus(DO.Bus bus)
        {
            var findBus = DS.DataSource.BussList.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum);
            
            if(findBus != null)
            {
                if (!bus.IsDeleted)
                    throw new DO.BadBusException(bus.LicenseNum, "Duplicate License number of bus");
            }

            DS.DataSource.BussList.Add(bus.Clone());
        }

        public IEnumerable<DO.Bus> GetAllBuss()
        {
            return from bus in DS.DataSource.BussList
                   where !bus.IsDeleted
                   select bus;
        }

        public IEnumerable<DO.Bus> GetAllBussBy(Predicate<DO.Bus> predicate)
        {
            return from bus in DS.DataSource.BussList
                   where predicate(bus) && !bus.IsDeleted
                   select bus;
        }

        public DO.Bus GetBus(int licenseNum)
        {
            var bus = DS.DataSource.BussList.Find(b => b.LicenseNum == licenseNum);

            if (bus != null && !bus.IsDeleted)
                return bus.Clone();
            else
                throw new DO.BadBusException(licenseNum, "bus not found");
        }

        public void RemoveBus(DO.Bus bus)
        {
            var busToRem = DS.DataSource.BussList.Find(b => b.LicenseNum == bus.LicenseNum);

            if (busToRem != null && !bus.IsDeleted)
            {
                DS.DataSource.BussList.Remove(busToRem);
                busToRem.IsDeleted = true;
                DS.DataSource.BussList.Add(busToRem);
            }
            else
                throw new DO.BadBusException(bus.LicenseNum, "bus not found");
        }

        public void UpdateBus(DO.Bus bus)
        {
            var busToUp = DS.DataSource.BussList.Find(b => b.LicenseNum == bus.LicenseNum);

            if (busToUp != null && !bus.IsDeleted)
            {
                DS.DataSource.BussList.Remove(busToUp);
                DS.DataSource.BussList.Add(bus);
            }
            else
                throw new DO.BadBusException(bus.LicenseNum, "bus not found");
        }
        #endregion

        #region Line
        public IEnumerable<DO.Line> GetAllLines()
        {
            return from line in DS.DataSource.LinesList
                   where !line.IsDeleted 
                   select line;
        }

        public IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate)
        {
            return from line in DS.DataSource.LinesList
                   where predicate(line) && !line.IsDeleted
                   select line;
        }

        public DO.Line GetLine(int lineId)
        {
            var line = DS.DataSource.LinesList.Find(l => l.Id == lineId);

            if (line != null && !line.IsDeleted)
                return line.Clone();
            else
                throw new DO.BadLineException(lineId, "Line not found");
        }

        public int AddLine(DO.Line line)
        {
            line.Id = DalApi.Counters.LineCounter;
            DS.DataSource.LinesList.Add(line.Clone());
            return line.Id;
        }

        public void UpdateLine(DO.Line line)
        {
            var lineToUp = DS.DataSource.LinesList.Find(l => l.Id == line.Id);

            if (lineToUp != null && !lineToUp.IsDeleted)
            {
                DS.DataSource.LinesList.Remove(lineToUp);
                DS.DataSource.LinesList.Add(line);
            }
            else
                throw new DO.BadLineException(line.Id, "Line not found");
        }

        public void RemoveLine(DO.Line line)
        {
            var lineToRem = DS.DataSource.LinesList.Find(l => l.Id == line.Id);

            if (lineToRem != null && !lineToRem.IsDeleted) 
            {
                GetAllLineStationBy(ls => ls.LineId == lineToRem.Id).ToList().ForEach(RemoveLineStationOnRemoveline);
                DS.DataSource.LinesList.Remove(lineToRem);
                lineToRem.IsDeleted = true;
                DS.DataSource.LinesList.Add(lineToRem);
            }              
            else
                throw new DO.BadLineException(line.Id, "Line not found");
        }
        #endregion

        #region Station
        public IEnumerable<DO.Station> GetAllStations()
        {
            return from station in DS.DataSource.StationsList
                   where !station.IsDeleted
                   select station;
        }

        public IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate)
        {
            return from station in DS.DataSource.StationsList
                   where predicate(station) && !station.IsDeleted
                   select station;
        }

        public DO.Station GetStation(int code)
        {
            var station = DS.DataSource.StationsList.Find(s => s.Code == code);
            if (station != null && !station.IsDeleted)
                return station.Clone();
            else
                throw new DO.BadStationException(code, "Station not found");
        }

        public void AddStation(DO.Station station)
        {
            var st = DS.DataSource.StationsList.FirstOrDefault(s => s.Code == station.Code);
             if(st != null && !st.IsDeleted)
                throw new DO.BadStationException(station.Code, "Duplicate Station");
            else
                DS.DataSource.StationsList.Add(station.Clone());
        }

        public void UpdateStation(DO.Station station)
        {
            var stationToUp = DS.DataSource.StationsList.Find(s => s.Code == station.Code);

            if (stationToUp != null && !stationToUp.IsDeleted)
            {
                DS.DataSource.StationsList.Remove(stationToUp);
                DS.DataSource.StationsList.Add(station);
            }
            else
                throw new DO.BadStationException(station.Code, "Station not found");
        }

        public void RemoveStation(DO.Station station)
        {
            var stationToRem = DS.DataSource.StationsList.Find(s => s.Code == station.Code);

            if (stationToRem != null && !stationToRem.IsDeleted)
            {
                GetAllLineStationBy(s => s.Station == stationToRem.Code).ToList().ForEach(RemoveLineStation);
                GetAllAdjacentStationsBy(s => s.Station1 == stationToRem.Code 
                || s.Station2 == stationToRem.Code).ToList().ForEach(RemoveAdjacentStations);
                
                DS.DataSource.StationsList.Remove(stationToRem);
                stationToRem.IsDeleted = true;
                DS.DataSource.StationsList.Add(stationToRem);
            }            
            else
                throw new DO.BadStationException(station.Code, "Station not found");
        }
        #endregion

        #region LineStation
        public IEnumerable<DO.LineStation> GetAllLineStation()
        {
            return from lineStation in DS.DataSource.LineStationsList
                   where !lineStation.IsDeleted
                   select lineStation;
        }

        public IEnumerable<DO.LineStation> GetAllLineStationBy(Predicate<DO.LineStation> predicate)
        {
            return from lineStation in DS.DataSource.LineStationsList
                   where predicate(lineStation) && !lineStation.IsDeleted
                   orderby lineStation.LineId, lineStation.LineStationIndex
                   select lineStation;
        }

        public DO.LineStation GetLineStation(int stationCode, int lineId)
        {
            var lineStation =
                DS.DataSource.LineStationsList.Find(s => s.Station == stationCode && s.LineId == lineId);

            if (lineStation != null && !lineStation.IsDeleted)
                return lineStation.Clone();
            else
                throw new DO.BadLineStationException(stationCode, lineId, "Station Line not found");
        }

        public void AddLineStation(DO.LineStation lineStation)
        {
            var ls = DS.DataSource.LineStationsList.FirstOrDefault(s =>
             s.Station == lineStation.Station && s.LineId == lineStation.LineId);
            if(ls != null)
            {
                if(!lineStation.IsDeleted)
                    throw new DO.BadLineStationException(lineStation.Station, lineStation.LineId,
                                                    "Duplicate line station");
            }

            var station = DS.DataSource.StationsList.Find(s => s.Code == lineStation.Station);
            if(station == null)
                throw new DO.BadLineStationException(lineStation.Station, lineStation.LineId,
                    $"Station '{lineStation.Station}' does not exist");
            if(station.IsDeleted)
                throw new DO.BadLineStationException(lineStation.Station, lineStation.LineId,
                    $"Station '{lineStation.Station}' does not exist");

            var line = DS.DataSource.LinesList.Find(l => l.Id == lineStation.LineId);
            if(line == null)
                throw new DO.BadLineStationException(lineStation.Station, lineStation.LineId,
                    $"Line '{lineStation.LineId}' does not exist");
            if(line.IsDeleted)
                throw new DO.BadLineStationException(lineStation.Station, lineStation.LineId,
                    $"Line '{lineStation.LineId}' does not exist");

            foreach (var s in GetAllLineStationBy(s => s.LineId == line.Id))
            {
                if(lineStation.PrevStation == s.Station)
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
            lineStation.Name = station.Name; 
            DS.DataSource.LineStationsList.Add(lineStation.Clone());
        }

        public void UpdateLineStation(DO.LineStation lineStation)
        {
            var ls = 
                DS.DataSource.LineStationsList.Find(s => s.Station == lineStation.Station
                && s.LineId == lineStation.LineId);

            if (ls != null && !ls.IsDeleted)
            {
                DS.DataSource.LineStationsList.Remove(ls);
                DS.DataSource.LineStationsList.Add(lineStation);
            }
            else
                throw new DO.BadLineStationException(lineStation.Station,
                                                lineStation.LineId, "Station line not found");
        }

        public void RemoveLineStationOnRemoveline(DO.LineStation lineStation)
        {
            var ls =
                DS.DataSource.LineStationsList.Find(s => s.Station == lineStation.Station
                && s.LineId == lineStation.LineId);
            if (ls != null && !ls.IsDeleted)
            {
                DS.DataSource.LineStationsList.Remove(ls);
                ls.IsDeleted = true;
                DS.DataSource.LineStationsList.Add(ls);
            }
        }

        public void RemoveLineStation(DO.LineStation lineStation)
        {
            var ls =
                DS.DataSource.LineStationsList.Find(s => s.Station == lineStation.Station
                    && s.LineId == lineStation.LineId);

            if (ls != null)
            {
                if (!ls.IsDeleted)
                {
                    updatePrevAndNextStationOnRemove(ls);
                    DS.DataSource.LineStationsList.Remove(ls);
                    ls.IsDeleted = true;
                    DS.DataSource.LineStationsList.Add(ls);
                }     
            }
            else  
                throw new DO.BadLineStationException(lineStation.Station,
                                                lineStation.LineId, "Station line not found");
        }

        void updatePrevAndNextStationOnRemove(DO.LineStation lineStation)
        {
            if (lineStation.NextStation == -1 && lineStation.PrevStation == 0)
                return;
            foreach (var ls in GetAllLineStationBy(s => s.LineId == lineStation.LineId
                    && s.LineStationIndex > lineStation.LineStationIndex))
            {
                ls.LineStationIndex -= 1;
                UpdateLineStation(ls);
            }
            if (lineStation.PrevStation != 0 && lineStation.NextStation != -1)
            {
                var nextAdja = GetAdjacentStations(lineStation.Station, lineStation.NextStation);
                var prevAdja = GetAdjacentStations(lineStation.PrevStation, lineStation.Station);
                AddAdjacentStations(new DO.AdjacentStations
                {
                    Distance = nextAdja.Distance + nextAdja.Distance,
                    IsDeleted = false,
                    Station1 = prevAdja.Station1,
                    Station2 = nextAdja.Station2,
                    TimeInHours = prevAdja.TimeInHours + nextAdja.TimeInHours,
                    TimeInMinutes = prevAdja.TimeInMinutes + nextAdja.TimeInMinutes,
                    LineCode = prevAdja.LineCode
                });

                var prev = GetLineStation(lineStation.PrevStation, lineStation.LineId);
                var next = GetLineStation(lineStation.NextStation, lineStation.LineId);
                prev.NextStation = lineStation.NextStation;
                next.PrevStation = lineStation.PrevStation;
                UpdateLineStation(prev);
                UpdateLineStation(next);
                return;
            }
            if (lineStation.PrevStation == 0)
            {
                var next = GetLineStation(lineStation.NextStation, lineStation.LineId);
                next.PrevStation = 0;
                UpdateLineStation(next);
                return;
            }
            if (lineStation.NextStation == -1)
            {
                var prev = GetLineStation(lineStation.PrevStation, lineStation.LineId);
                prev.NextStation = -1;
                UpdateLineStation(prev);
                return;
            }
        }
        #endregion

        #region User
        public IEnumerable<DO.User> GetAllUsers()
        {
            return from user in DS.DataSource.UsersList
                   where !user.IsDeleted
                   select user.Clone();
        }

        public IEnumerable<DO.User> GetAllUsersBy(Predicate<DO.User> predicate)
        {
            return from user in DS.DataSource.UsersList
                   where predicate(user) && !user.IsDeleted
                   select user;
        }

        public DO.User GetUser(string userName)
        {
            var user = DS.DataSource.UsersList.Find(u => u.UserName == userName);

            if (user != null && !user.IsDeleted)
                return user.Clone();
            else
                throw new DO.BadUsernameException(userName, $"User not found {userName}");
        }

        public void AddUser(DO.User newUser)
        {
            var user = DS.DataSource.UsersList.FirstOrDefault(u => u.UserName == newUser.UserName);
            if(user != null && !newUser.IsDeleted)
                throw new DO.BadUsernameException(newUser.UserName, "Duplicate user username");
            
            DS.DataSource.UsersList.Add(newUser.Clone());
        }

        public void UpdateUser(DO.User user)
        {
            var findUser = DS.DataSource.UsersList.Find(u => u.UserName == user.UserName);

            if (findUser != null && !findUser.IsDeleted)
            {
                DS.DataSource.UsersList.Remove(findUser);
                DS.DataSource.UsersList.Add(user);
            }
            else
                throw new DO.BadUsernameException(user.UserName, $"User not found {user.UserName}");
        }

        public void RemoveUser(DO.User user)
        {
            var findUser = DS.DataSource.UsersList.Find(u => u.UserName == user.UserName);

            if (findUser != null && !findUser.IsDeleted)
            {
                DS.DataSource.UsersList.Remove(findUser);
                findUser.IsDeleted = true;
                DS.DataSource.UsersList.Add(findUser);
            }
            else
                throw new DO.BadUsernameException(user.UserName, $"User not found {user.UserName}");
        }
        #endregion

        #region AdjacentStations
        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStations()
        {
            return from adst in DS.DataSource.AdjacentStationsList
                   where !adst.IsDeleted
                   select adst.Clone();
        }

        public IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate)
        {
            return from adst in DS.DataSource.AdjacentStationsList
                   where !adst.IsDeleted && predicate(adst)
                   select adst.Clone();
        }

        public DO.AdjacentStations GetAdjacentStations(int station1, int station2)
        {
            var adst = DS.DataSource.AdjacentStationsList.Find(s =>
                                        s.Station1 == station1 && s.Station2 == station2);
            if (adst != null && !adst.IsDeleted)
            {
                return adst.Clone();
            }
                
            throw new DO.BadAdjacentStationsException(station1, station2,
                    "Adjacent stations not found");
        }

        public void AddAdjacentStations(DO.AdjacentStations adjacentStations)
        {
            var adst = DS.DataSource.AdjacentStationsList.Find(s =>
            s.Station1 == adjacentStations.Station1 && s.Station2 == adjacentStations.Station2);
            if(adst != null) 
            {
                if (!adst.IsDeleted)
                    throw new DO.BadAdjacentStationsException(adjacentStations.Station1, adjacentStations.Station2,
                       "Duplicate adjacent stations");
            }
            DS.DataSource.AdjacentStationsList.Add(adjacentStations);
        }

        public void RemoveAdjacentStations(DO.AdjacentStations adjacentStations)
        {
            var adst =
                DS.DataSource.AdjacentStationsList.Find(s => s.Station1 == adjacentStations.Station1
                && s.Station2 == adjacentStations.Station2);

            if (adst != null && !adst.IsDeleted)
            {
                DS.DataSource.AdjacentStationsList.Remove(adst);
                adst.IsDeleted = true;
                DS.DataSource.AdjacentStationsList.Add(adst);
            }
                
            else
                throw new DO.BadAdjacentStationsException(adjacentStations.Station1, adjacentStations.Station2
                    , "Adjacent Stations not found");
        }

        public void UpdateAdjacentStations(DO.AdjacentStations adjacentStations)
        {
            var adst =
                DS.DataSource.AdjacentStationsList.Find(s => s.Station1 == adjacentStations.Station1
                && s.Station2 == adjacentStations.Station2);

            if (adst != null && !adst.IsDeleted)
            {
                DS.DataSource.AdjacentStationsList.Remove(adst);
                DS.DataSource.AdjacentStationsList.Add(adjacentStations.Clone());
            }
            else
                throw new DO.BadAdjacentStationsException(adjacentStations.Station1, adjacentStations.Station2
                    , "Adjacent Stations not found");
        }
        #endregion

        #region LineTrip
        public int AddLineTrip(DO.LineTrip lineTrip)
        {
            lineTrip.Id = DalApi.Counters.LineTripCounter;
            DS.DataSource.LineTripsList.Add(lineTrip);
            return lineTrip.Id;
        }

        public DO.LineTrip GetLineTrip(int id)
        {
            var lineTrip = DS.DataSource.LineTripsList.Find(l => l.Id == id);

            if (lineTrip != null)
                return lineTrip.Clone();
            else
                throw new DO.BadLineTripException(id, "line trip not found");
        }

        public IEnumerable<DO.LineTrip> GetAllLineTrips()
        {
            return from line in DS.DataSource.LineTripsList
                   select line.Clone();
        }

        public IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate)
        {
            return from line in DS.DataSource.LineTripsList
                   where predicate(line)
                   select line.Clone();
        }
        #endregion
    }
}
