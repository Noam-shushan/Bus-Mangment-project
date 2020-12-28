using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class BLImp : BlApi.IBL
    {
        DalApi.IDaL dl = DalApi.DaLFactory.GetDL();

        #region singelton
        public static BlApi.IBL Instance { get; } = new BLImp();

        BLImp() { }
        #endregion

        #region User
        public BO.User GetUser(string userName)
        {
            BO.User userBo = new BO.User();
            DO.User userDo;
            try
            {
                userDo = dl.GetUser(userName);
                userDo.CopyPropertiesTo(userBo);
            }
            catch (DO.BadUsernameException ex)
            {
                throw new BO.BadUsernameException(userName, ex.Message);
            }
            return userBo;
        }

        public void AddUser(BO.User newUser)
        {
            var userDo = newUser.CopyPropertiesToNew(typeof(DO.User)) as DO.User;
            try
            {
                dl.AddUser(userDo);
            }
            catch(DO.BadUsernameException ex)
            {
                throw new BO.BadUsernameException(newUser.UserName, ex.Message);
            }
        }

        public void RemmoveUser(BO.User user)
        {
            var userDo = user.CopyPropertiesToNew(typeof(DO.User)) as DO.User;
            try
            {
                dl.RemmoveUser(userDo);
            }
            catch (DO.BadUsernameException ex)
            {
                throw new BO.BadUsernameException(user.UserName, ex.Message);
            }
        }

        public void UpdateUser(BO.User user)
        {
            var userDo = user.CopyPropertiesToNew(typeof(DO.User)) as DO.User;
            try
            {
                dl.UpdateUser(userDo);
            }
            catch (DO.BadUsernameException ex)
            {
                throw new BO.BadUsernameException(user.UserName, ex.Message);
            }
        }

        public IEnumerable<BO.User> GetAllUsers()
        {
            return from user in dl.GetAllUsers()
                   let userBo = user.CopyPropertiesToNew(typeof(BO.User)) as BO.User
                   select userBo;
        }

        public IEnumerable<BO.User> GetAllUsersBy(Predicate<BO.User> predicate)
        {
            return from user in dl.GetAllUsers()
                   let userBo = user.CopyPropertiesToNew(typeof(BO.User)) as BO.User
                   where predicate(userBo)
                   select userBo;
        }
        #endregion

        #region Bus
        public IEnumerable<BO.Bus> GetAllBuss()
        {
            return from bus in dl.GetAllBuss()
                   let busBo = bus.CopyPropertiesToNew(typeof(BO.Bus)) as BO.Bus
                   select busBo;
        }

        public IEnumerable<BO.Bus> GetAllBussBy(Predicate<BO.Bus> predicate)
        {
            return from bus in dl.GetAllBuss()
                   let busBo = bus.CopyPropertiesToNew(typeof(BO.Bus)) as BO.Bus
                   where predicate(busBo)
                   select busBo;
        }

        public BO.Bus GetBus(int licenseNum)
        {
            var busBo = new BO.Bus();
            try
            {
                var busDo = dl.GetBus(licenseNum);
                busDo.CopyPropertiesTo(busBo);
            }
            catch (DO.BadBusException ex)
            {
                throw new BO.BadBusException(licenseNum, ex.Message);
            }
            return busBo;
        }

        public void AddBus(BO.Bus bus)
        {
            var busDo = bus.CopyPropertiesToNew(typeof(DO.Bus)) as DO.Bus;
            try
            {
                dl.AddBus(busDo);
            }
            catch(DO.BadBusException ex)
            {
                throw new BO.BadBusException(bus.LicenseNum, ex.Message);
            }
        }

        public void UpdateBus(BO.Bus bus)
        {
            var busDo = bus.CopyPropertiesToNew(typeof(DO.Bus)) as DO.Bus;
            try
            {
                dl.UpdateBus(busDo);
            }
            catch (DO.BadBusException ex)
            {
                throw new BO.BadBusException(bus.LicenseNum, ex.Message);
            }
        }

        public void RemmoveBus(BO.Bus bus)
        {
            var busDo = bus.CopyPropertiesToNew(typeof(DO.Bus)) as DO.Bus;
            try
            {
                dl.RemmoveBus(busDo);
            }
            catch (DO.BadBusException ex)
            {
                throw new BO.BadBusException(bus.LicenseNum, ex.Message);
            }
        }
        #endregion

        #region Line
        public IEnumerable<BO.Line> GetAllLines()
        {
            return from line in dl.GetAllLines()
                   let lineBo = line.CopyPropertiesToNew(typeof(BO.Line)) as BO.Line
                   select lineBo;
        }
        
        public IEnumerable<BO.Line> GetAllLinesBy(Predicate<BO.Line> predicate)
        {
            return from line in dl.GetAllLines()
                   let lineBo = line.CopyPropertiesToNew(typeof(BO.Line)) as BO.Line
                   where predicate(lineBo)
                   select lineBo;
        }

        public BO.Line GetLine(int lineId)
        {
            var lineBo = new BO.Line();
            try
            {
                var busDo = dl.GetBus(lineId);
                busDo.CopyPropertiesTo(lineBo);
            }
            catch (DO.BadLineException ex)
            {
                throw new BO.BadLineException(lineId, ex.Message);
            }
            return lineBo;
        }

        public void AddLine(BO.Line newLine)
        {
            var lineDo = newLine.CopyPropertiesToNew(typeof(DO.Line)) as DO.Line;
            try
            {
                dl.AddLine(lineDo);
            }
            catch (DO.BadLineException ex)
            {
                throw new BO.BadLineException(newLine.Id, ex.Message);
            }
        }

        public void UpdateLine(BO.Line line)
        {
            var lineDo = line.CopyPropertiesToNew(typeof(DO.Line)) as DO.Line;
            try
            {
                dl.UpdateLine(lineDo);
            }
            catch (DO.BadLineException ex)
            {
                throw new BO.BadLineException(line.Id, ex.Message);
            }
        }

        public void RemmoveLine(BO.Line line)
        {
            var lineDo = line.CopyPropertiesToNew(typeof(DO.Line)) as DO.Line;
            try
            {
                dl.RemmoveLine(lineDo);
            }
            catch (DO.BadLineException ex)
            {
                throw new BO.BadLineException(line.Id, ex.Message);
            }
        }
        #endregion

        #region Station
        public IEnumerable<BO.Station> GetAllStations()
        {
            return from station in dl.GetAllStations()
                   let stationBo = station.CopyPropertiesToNew(typeof(BO.Station)) as BO.Station
                   select stationBo;
        }

        public IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate)
        {
            return from station in dl.GetAllStations()
                   let stationBo = station.CopyPropertiesToNew(typeof(BO.Station)) as BO.Station
                   where predicate(stationBo)
                   select stationBo;
        }

        public BO.Station GetStation(int code)
        {
            var stationBo = new BO.Station();
            try
            {
                var stationDo = dl.GetStation(code);
                stationDo.CopyPropertiesTo(stationBo);
            }
            catch (DO.BadStationException ex)
            {
                throw new BO.BadStationException(code, ex.Message);
            }
            return stationBo;
        }

        public void UpdateStation(BO.Station station)
        {
            var stationDo = station.CopyPropertiesToNew(typeof(DO.Station)) as DO.Station;
            try
            {
                dl.AddStation(stationDo);
            }
            catch (DO.BadStationException ex)
            {
                throw new BO.BadStationException(station.Code, ex.Message);
            }
        }

        public void AddStation(BO.Station station)
        {
            var stationDo = station.CopyPropertiesToNew(typeof(DO.Station)) as DO.Station;
            try
            {
                dl.AddStation(stationDo);
            }
            catch (DO.BadStationException ex)
            {
                throw new BO.BadStationException(station.Code, ex.Message);
            }
        }

        public void RemmoveStation(BO.Station station)
        {
            var stationDo = station.CopyPropertiesToNew(typeof(DO.Station)) as DO.Station;
            try
            {
                dl.RemmoveStation(stationDo);
            }
            catch (DO.BadStationException ex)
            {
                throw new BO.BadStationException(station.Code, ex.Message);
            }
        }
        #endregion
    }
}
