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
        /// <summary>
        /// Gets a single user by a given Username
        /// </summary>
        /// <param name="userName">a given username, the entity key</param>
        /// <returns>User of BO</returns>
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
        /// <summary>
        /// Add new User to my the Data Source
        /// </summary>
        /// <param name="newUser"></param>
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
        /// <summary>
        /// Remove user from my the Data Source
        /// </summary>
        /// <param name="user"></param>
        public void RemoveUser(BO.User user)
        {
            var userDo = user.CopyPropertiesToNew(typeof(DO.User)) as DO.User;
            try
            {
                dl.RemoveUser(userDo);
            }
            catch (DO.BadUsernameException ex)
            {
                throw new BO.BadUsernameException(user.UserName, ex.Message);
            }
        }
        /// <summary>
        /// Update user in my Data Source
        /// </summary>
        /// <param name="user"></param>
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
        /// <summary>
        /// Get all users from my Data Source
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BO.User> GetAllUsers()
        {
            return from user in dl.GetAllUsers()
                   let userBo = user.CopyPropertiesToNew(typeof(BO.User)) as BO.User
                   select userBo;
        }
        /// <summary>
        /// Get all users by condition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BO.User> GetAllUsersBy(Predicate<BO.User> predicate)
        {
            return from user in dl.GetAllUsers()
                   let userBo = user.CopyPropertiesToNew(typeof(BO.User)) as BO.User
                   where predicate(userBo)
                   select userBo;
        }
        #endregion

        #region Bus
        /// <summary>
        /// Get all Buss from my Data Source
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BO.Bus> GetAllBuss()
        {
            return from bus in dl.GetAllBuss()
                   let busBo = bus.CopyPropertiesToNew(typeof(BO.Bus)) as BO.Bus
                   select busBo;
        }
        /// <summary>
        /// Get all buss by condition from my Data Source
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BO.Bus> GetAllBussBy(Predicate<BO.Bus> predicate)
        {
            return from bus in dl.GetAllBuss()
                   let busBo = bus.CopyPropertiesToNew(typeof(BO.Bus)) as BO.Bus
                   where predicate(busBo)
                   select busBo;
        }
        /// <summary>
        /// Get a singal bus from my Data Source
        /// </summary>
        /// <param name="licenseNum">license number ,the entity key</param>
        /// <returns>Bus BO</returns>
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
        /// <summary>
        /// Add a new bus to my Data Source
        /// </summary>
        /// <param name="bus"></param>
        public void AddBus(BO.Bus bus)
        {
            if(bus.FromDate.Year >= 2018 && bus.LicenseNum.ToString().Length != 8)
                throw new BO.BadBusException(bus.LicenseNum, "not valid lisense number for bus from year 2018");
            
            if (bus.FromDate.Year <= 2017 && bus.LicenseNum.ToString().Length != 7)
                throw new BO.BadBusException(bus.LicenseNum, "not valid lisense number for bus from year under 2017");
            
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
        /// <summary>
        /// Update Bus in my Data Source
        /// </summary>
        /// <param name="bus"></param>
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
        /// <summary>
        /// Remove bus from my Data Source
        /// </summary>
        /// <param name="bus"></param>
        public void RemoveBus(BO.Bus bus)
        {
            var busDo = bus.CopyPropertiesToNew(typeof(DO.Bus)) as DO.Bus;
            try
            {
                dl.RemoveBus(busDo);
            }
            catch (DO.BadBusException ex)
            {
                throw new BO.BadBusException(bus.LicenseNum, ex.Message);
            }
        }

        public void BusServices(BO.Bus bus, string service, double tripKm = 0)
        {
            switch (service)
            {
                case "Treatment" :
                    treatment(bus);
                    break;
                case "Refueling" :
                    refueling(bus);
                    break;
                case "Ride" :
                    ride(bus, tripKm);
                    break;
            }
            UpdateBus(bus);
        }

        void treatment(BO.Bus bus)
        {
            bus.Status = BO.BusStatus.TREATMENT;
            bus.KilometersAfterTreatment = 0;
            bus.LastTreatment = DateTime.Now;
        }

        void refueling(BO.Bus bus)
        {
            bus.Status = BO.BusStatus.REFUELING;
            bus.KilometersAfterFueling = 0;
            bus.FuelRemain = BO.Bus.FULL_CONTAINER;
        }
        void ride(BO.Bus bus, double km)
        {
            if (bus.KilometersAfterFueling + km >= BO.Bus.MAX_KILOMETER_AFTER_REFUELING)
                throw new BO.BadBusException(bus.LicenseNum, "This bus need refueling to make this trip");
            
            if (DateTime.Today.AddYears(-1) > bus.LastTreatment ||
                bus.KilometersAfterTreatment + km >= BO.Bus.KILOMETER_BEFORE_TREATMENT)
                throw new BO.BadBusException(bus.LicenseNum, "This bus need treatment to make this trip");
            
            bus.TotalTrip += km;
            bus.KilometersAfterFueling += km;
            bus.KilometersAfterTreatment += km;
            bus.FuelRemain -= (BO.Bus.FULL_CONTAINER / BO.Bus.MAX_KILOMETER_AFTER_REFUELING) * km;
        }
        #endregion

        #region LineStation
        /// <summary>
        /// Get all the line station of a spsific line in my Data Source
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns>all the line station of line</returns>
        public IEnumerable<BO.LineStation> GetAllLineStationsByLineID(int lineId)
        {
            return from ls1 in dl.GetAllLineStationBy(ls2 => ls2.LineId == lineId)
                    let lsBo = GetLineStation(ls1.Station, lineId)
                    select lsBo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationCode"></param>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public BO.LineStation GetLineStation(int stationCode, int lineId)
        {
            BO.LineStation stationBo;
            try
            {
                var stationDo = dl.GetLineStation(stationCode, lineId);
                stationBo = stationDo.CopyPropertiesToNew(typeof(BO.LineStation)) as BO.LineStation;
                if (stationBo.NextStation != -1) // last station in the line
                {
                    try
                    {
                        var next = GetAdjacentStations(stationCode, stationBo.NextStation);
                        stationBo.DistanceFromNext = next.Distance;
                        stationBo.TimeFromNext = next.Time;
                    }
                    catch (BO.BadAdjacentStationsException)
                    {
                        throw new BO.BadLineStationException(stationCode, lineId,
                                $"Missing distance and time information from station {stationBo.NextStation}");
                    }
                }
                return stationBo;
            }
            catch(DO.BadLineStationException ex)
            {
                throw new BO.BadLineStationException(stationCode, lineId, ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newLineStation"></param>
        public void AddLineStation(BO.LineStation newLineStation)
        {
            try
            {
                var lineStationDo = newLineStation.CopyPropertiesToNew(typeof(DO.LineStation)) as DO.LineStation;
                dl.AddLineStation(lineStationDo);
            }
            catch (DO.BadLineStationException ex)
            {
                throw new BO.BadLineStationException(newLineStation.Station, newLineStation.LineId,
                    ex.Message);
            }
        }

        public void RemoveLineStation(BO.LineStation station)
        {
            try
            {
                var lineStationDo = station.CopyPropertiesToNew(typeof(DO.LineStation)) as DO.LineStation;
                dl.RemoveLineStation(lineStationDo);
            }
            catch (DO.BadLineStationException ex)
            {
                throw new BO.BadLineStationException(station.Station, station.LineId, ex.Message);
            }
        }
        #endregion

        #region Line
        public IEnumerable<BO.Line> GetAllLines()
        {
            return from line in dl.GetAllLines()
                   let lineBo = GetLine(line.Id)
                   select lineBo;
        }
        
        public IEnumerable<BO.Line> GetAllLinesBy(Predicate<BO.Line> predicate)
        {
            return from line in GetAllLines()
                   where predicate(line)
                   select line;
        }

        public IEnumerable<BO.Line> GetAllLinePassByStation(int stationCode)
        {
            var allLineStationsOfSpecific = from st
                                            in dl.GetAllLineStationBy(ls => ls.Station == stationCode)
                                            select st;
            return from st in allLineStationsOfSpecific
                   from line in GetAllLinesBy(l => st.LineId == l.Id)
                   select line;
        }

        public BO.Line GetLine(int lineId)
        {
            var lineBo = new BO.Line();
            try
            {
                var lineDo = dl.GetLine(lineId);
                lineDo.CopyPropertiesTo(lineBo);
                lineBo.LineStations = GetAllLineStationsByLineID(lineId);
            }
            catch (DO.BadLineException ex)
            {
                throw new BO.BadLineException(lineId, ex.Message);
            }
            catch(BO.BadLineStationException ex)
            {
                throw ex;
            }
            return lineBo;
        }

        public int AddLine(BO.Line newLine)
        {
            if (!dl.GetAllStations().Any(s => s.Code == newLine.FirstStation))
                throw new BO.BadLineException(newLine.Id, $"Station '{newLine.FirstStation}' does not exist");
            
            if (!dl.GetAllStations().Any(s => s.Code == newLine.LastStation))
                throw new BO.BadLineException(newLine.Id, $"Station '{newLine.LastStation}' does not exist");

            if (!dl.GetAllAdjacentStations().Any(s => s.Station1 == newLine.FirstStation
             && s.Station2 == newLine.LastStation))
                throw new BO.BadLineException(newLine.Id,
                    $"Missing distance and time information between {newLine.FirstStation}" +
                    $" and {newLine.LastStation}");
            
            var lineDo = newLine.CopyPropertiesToNew(typeof(DO.Line)) as DO.Line;
            return dl.AddLine(lineDo);
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

        public void RemoveLine(BO.Line line)
        {
            var lineDo = line.CopyPropertiesToNew(typeof(DO.Line)) as DO.Line;
            try
            {
                dl.RemoveLine(lineDo);
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
                   let stationBo = GetStation(station.Code)
                   select stationBo;
        }

        public IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate)
        {
            return from station in GetAllStations()
                   where predicate(station)
                   select station;
        }

        public BO.Station GetStation(int code)
        {
            var stationBo = new BO.Station();
            try
            {
                var stationDo = dl.GetStation(code);
                stationDo.CopyPropertiesTo(stationBo);
                stationBo.LinesPassBy = GetAllLinePassByStation(code);
                stationBo.MyAdjacentStations = GetAllAdjacentStationsOf(code);
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
                dl.UpdateStation(stationDo);
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
                //if (!validStationCode(stationDo.Code))
                //    throw new BO.BadStationException(stationDo.Code, "Not valid station code");
                
                if(!validLatLon(stationDo.Latitude, stationDo.Longitude))
                    throw new BO.BadStationException(stationDo.Code,
                        "The location entered is outside the Earth");
                
                dl.AddStation(stationDo);
            }
            catch (DO.BadStationException ex)
            {
                throw new BO.BadStationException(station.Code, ex.Message);
            }
        }

        bool validLatLon(double lat, double lon)
        {
            return lat >= -90 && lat <= 90 && lon >= -180 && lon <= 180;
        }

        bool validStationCode(int code)
        {
            return code >= 100000 && code <= 999999;
        }

        public void RemoveStation(BO.Station station)
        {
            var stationDo = station.CopyPropertiesToNew(typeof(DO.Station)) as DO.Station;
            try
            {
                dl.RemoveStation(stationDo);
            }
            catch (DO.BadStationException ex)
            {
                throw new BO.BadStationException(station.Code, ex.Message);
            }
        }
        #endregion

        #region AdjacentStations
        public void AddAdjacentStations(BO.AdjacentStations adjacentStations)
        {
            var adjacentStationsDo = adjacentStations.CopyPropertiesToNew(
                typeof(DO.AdjacentStations)) as DO.AdjacentStations;
            try
            {
                dl.AddAdjacentStations(adjacentStationsDo);
            }
            catch (DO.BadAdjacentStationsException ex)
            {
                throw new BO.BadAdjacentStationsException(adjacentStations.Station1, adjacentStations.Station2,
                    ex.Message);
            }
            catch (DO.BadStationException ex)
            {
                throw new BO.BadAdjacentStationsException(adjacentStations.Station1, adjacentStations.Station2,
                    ex.Message);
            }
        }

        public IEnumerable<BO.AdjacentStations> GetAllAdjacentStationsOf(int stationCode)
        {
            return from adst in dl.GetAllAdjacentStations()
                   where adst.Station1 == stationCode
                   let adstBo = adst.CopyPropertiesToNew(typeof(BO.AdjacentStations)) as BO.AdjacentStations
                   select adstBo;
        }

        public BO.AdjacentStations GetAdjacentStations(int station1, int station2)
        {
            try
            {
                var adst = dl.GetAdjacentStations(station1, station2);
                var adstBo =  adst.CopyPropertiesToNew(typeof(BO.AdjacentStations)) as BO.AdjacentStations;
                return adstBo;
            }
            catch (DO.BadAdjacentStationsException ex)
            {

                throw new BO.BadAdjacentStationsException(station1, station2, ex.Message);
            }
        }

        public void UpdateAdjacentStations(BO.AdjacentStations adjacentStations)
        {
            var adjacentStationsDo = adjacentStations.CopyPropertiesToNew(
                typeof(DO.AdjacentStations)) as DO.AdjacentStations;
            try
            {
                dl.UpdateAdjacentStations(adjacentStationsDo);
            }
            catch (DO.BadAdjacentStationsException ex)
            {
                throw new BO.BadAdjacentStationsException(adjacentStations.Station1,
                    adjacentStations.Station2, ex.Message);
            }
        }
        #endregion

    }
}
