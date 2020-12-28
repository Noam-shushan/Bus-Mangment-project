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
        public void AddBus(DO.Bus bus)
        {
            if (DS.DataSource.BussList.FirstOrDefault(b => b.LicenseNum == bus.LicenseNum) != null)
                throw new DO.BadBusException(bus.LicenseNum, "Duplicate License number of bus");
            else
                DS.DataSource.BussList.Add(bus);
        }

        public IEnumerable<DO.Bus> GetAllBuss()
        {
            return from bus in DS.DataSource.BussList
                  select bus;
        }

        public IEnumerable<DO.Bus> GetAllBussBy(Predicate<DO.Bus> predicate)
        {
            return from bus in DS.DataSource.BussList
                   where predicate(bus)
                   select bus;
        }

        public DO.Bus GetBus(int licenseNum)
        {
            var bus = DS.DataSource.BussList.Find(b => b.LicenseNum == licenseNum);

            if (bus != null)
                return bus.Clone();
            else
                throw new DO.BadBusException(licenseNum, "bus not found");
        }

        public void RemmoveBus(DO.Bus bus)
        {
            var busToRem = DS.DataSource.BussList.Find(b => b.LicenseNum == bus.LicenseNum);

            if (busToRem != null)
                DS.DataSource.BussList.Remove(busToRem);
            else
                throw new DO.BadBusException(bus.LicenseNum, "bus not found");
        }

        public void UpdateBus(DO.Bus bus)
        {
            var busToUp = DS.DataSource.BussList.Find(b => b.LicenseNum == bus.LicenseNum);

            if (busToUp != null)
                busToUp = bus.Clone();
            else
                throw new DO.BadBusException(bus.LicenseNum, "bus not found");
        }
        #endregion

        #region Line
        public IEnumerable<DO.Line> GetAllLines()
        {
            return from l in DS.DataSource.LinesList
                   select l;
        }

        public IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate)
        {
            return from l in DS.DataSource.LinesList
                   where predicate(l)
                   select l;
        }

        public DO.Line GetLine(int lineId)
        {
            var line = DS.DataSource.LinesList.Find(l => l.Id == lineId);

            if (line != null)
                return line.Clone();
            else
                throw new DO.BadLineException(lineId, "Line not found");
        }

        public void AddLine(DO.Line line)
        {
            if (DS.DataSource.LinesList.FirstOrDefault(l => l.Code == line.Code) != null)
                throw new DO.BadLineException(line.Code, "Duplicate line id");
            else
                DS.DataSource.LinesList.Add(line.Clone());
        }

        public void UpdateLine(DO.Line line)
        {
            var lineToUp = DS.DataSource.LinesList.Find(l => l.Id == line.Id);

            if (lineToUp != null)
                lineToUp = line.Clone();
            else
                throw new DO.BadLineException(line.Id, "Line not found");
        }

        public void RemmoveLine(DO.Line line)
        {
            var lineToRem = DS.DataSource.LinesList.Find(l => l.Id == line.Id);

            if (lineToRem != null)
                DS.DataSource.LinesList.Remove(lineToRem);
            else
                throw new DO.BadLineException(line.Id, "Line not found");
        }
        #endregion

        #region Station
        public IEnumerable<DO.Station> GetAllStations()
        {
            return from s in DS.DataSource.StationsList
                   select s;
        }

        public IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate)
        {
            return from s in DS.DataSource.StationsList
                   where predicate(s)
                   select s;
        }

        public DO.Station GetStation(int code)
        {
            var station = DS.DataSource.StationsList.Find(s => s.Code == code);
            if (station != null)
                return station.Clone();
            else
                throw new DO.BadStationException(code, "Station not found");
        }

        public void AddStation(DO.Station station)
        {
            if (DS.DataSource.StationsList.FirstOrDefault(s => s.Code == station.Code) != null)
                throw new DO.BadStationException(station.Code, "Duplicate Station");
            else
                DS.DataSource.StationsList.Add(station.Clone());
        }

        public void UpdateStation(DO.Station station)
        {
            var stationToUp = DS.DataSource.StationsList.Find(s => s.Code == station.Code);

            if (stationToUp != null)
                stationToUp = station.Clone();
            else
                throw new DO.BadStationException(station.Code, "Station not found");
        }

        public void RemmoveStation(DO.Station station)
        {
            var stationToRem = DS.DataSource.StationsList.Find(s => s.Code == station.Code);

            if (stationToRem != null)
                DS.DataSource.StationsList.Remove(stationToRem);
            else
                throw new DO.BadStationException(station.Code, "Station not found");
        }
        #endregion

        #region LineStation
        public IEnumerable<DO.LineStation> GetAllLineStation()
        {
            return from ls in DS.DataSource.LineStationsList
                   select ls;
        }

        public IEnumerable<DO.LineStation> GetAllLineStationBy(Predicate<DO.LineStation> predicate)
        {
            return from lineStation in DS.DataSource.LineStationsList
                   where predicate(lineStation)
                   select lineStation;
        }

        public DO.LineStation GetLineStation(int stationCode, int lineId)
        {
            var lineStation =
                DS.DataSource.LineStationsList.Find(s => s.Station == stationCode && s.LineId == lineId);

            if (lineStation != null)
                return lineStation.Clone();
            else
                throw new DO.BadLineStationException(stationCode, lineId, "Station Line not found");
        }

        public void AddLineStation(DO.LineStation lineStation)
        {
            if (DS.DataSource.LineStationsList.FirstOrDefault(s =>
             s.Station == lineStation.Station && s.LineId == lineStation.LineId) != null)
                throw new DO.BadLineStationException(lineStation.Station, lineStation.LineId,
                    "Duplicate station line");

            DS.DataSource.LineStationsList.Add(lineStation);
        }

        public void UpdateLineStation(DO.LineStation lineStation)
        {
            var ls = 
                DS.DataSource.LineStationsList.Find(s => s.Station == lineStation.Station
                && s.LineId == lineStation.LineId);

            if (ls == null)
                throw new DO.BadLineStationException(lineStation.Station,
                    lineStation.LineId, "Station line not found");
            else
                ls = lineStation.Clone();
        }

        public void RemmoveLineStation(DO.LineStation lineStation)
        {
            var ls =
                DS.DataSource.LineStationsList.Find(s => s.Station == lineStation.Station
                    && s.LineId == lineStation.LineId);

            if (ls == null)
                throw new DO.BadLineStationException(lineStation.Station,
                    lineStation.LineId, "Station line not found");
            else
                DS.DataSource.LineStationsList.Remove(ls);
        }
        #endregion

        #region User
        public IEnumerable<DO.User> GetAllUsers()
        {
            return from user in DS.DataSource.UsersList
                   select user.Clone();
        }

        public IEnumerable<DO.User> GetAllUsersBy(Predicate<DO.User> predicate)
        {
            return from user in DS.DataSource.UsersList
                   where predicate(user)
                   select user;
        }

        public DO.User GetUser(string userName)
        {
            var user = DS.DataSource.UsersList.Find(u => u.UserName == userName);

            if (user != null)
                return user.Clone();
            else
                throw new DO.BadUsernameException(userName, $"User not found {userName}");
        }

        public void AddUser(DO.User newUser)
        {
            if (DS.DataSource.UsersList.FirstOrDefault(u => u.UserName == newUser.UserName) != null)
                throw new DO.BadUsernameException(newUser.UserName, "Duplicate user username");
            DS.DataSource.UsersList.Add(newUser.Clone());
        }

        public void UpdateUser(DO.User user)
        {
            var findUser = DS.DataSource.UsersList.Find(u => u.UserName == user.UserName);

            if (findUser != null)
                findUser = user.Clone();
            else
                throw new DO.BadUsernameException(user.UserName, $"User not found {user.UserName}");
        }

        public void RemmoveUser(DO.User user)
        {
            var findUser = DS.DataSource.UsersList.Find(u => u.UserName == user.UserName);

            if (findUser != null)
                DS.DataSource.UsersList.Remove(findUser);
            else
                throw new DO.BadUsernameException(user.UserName, $"User not found {user.UserName}");
        } 
        #endregion

    }
}
