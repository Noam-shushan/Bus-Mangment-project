using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BL
{
    /// <summary>
    /// Implementation of IBL - The logical layer of the project
    /// Links the entities and cross-references data coming from the database
    /// in order to present them to the user
    /// </summary>
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
        /// Get the hash password of a given real password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string GetHashPassword(string password)
        {
            SHA512 shaM = new SHA512Managed();
            return Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        /// <summary>
        /// Check if a given password is correct
        /// </summary>
        /// <param name="user">the user to check</param>
        /// <param name="password">real password</param>
        /// <returns>treu is mach else throw exeption</returns>
        public bool IsCorrectPassword(BO.User user ,string password)
        {
            if(user.HashedPassword == GetHashPassword(password))
            {
                return true;
            }
            throw new BO.BadUsernameException(user.UserName, "Not valid password");
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
        /// <summary>
        /// make a treatment, refueling, ride to the bus
        /// </summary>
        /// <param name="bus">the bus</param>
        /// <param name="service">kind of service</param>
        /// <param name="tripKm">if the sevice is ride that will be the kilometers</param>
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

        List<DO.Bus> getCopyBuss()
        {
            return new List<DO.Bus>(dl.GetAllBussBy(b => b.Status != DO.BusStatus.READY));
        }

        public void SetBusStatusOnClosing()
        {
            foreach(var bus in getCopyBuss())
            {
                bus.Status = DO.BusStatus.READY;
                dl.UpdateBus(bus);
            }
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
        /// Get a single line station by is key (station code, line id)
        /// Build the station with the distace and time from the next station on the line
        /// </summary>
        /// <param name="stationCode"></param>
        /// <param name="lineId"></param>
        /// <returns></returns>
        public BO.LineStation GetLineStation(int stationCode, int lineId)
        {
            BO.LineStation lineStationBo;
            try
            {
                var lineStationDo = dl.GetLineStation(stationCode, lineId);
                lineStationBo = lineStationDo.CopyPropertiesToNew(typeof(BO.LineStation)) as BO.LineStation;

                var station = dl.GetStation(stationCode);
                lineStationBo.Name = station.Name; // save the name of this station

                if (lineStationBo.NextStation != -1) // last station in the line
                {
                    try
                    {   // get infromation on distanc and time from next  
                        var next = GetAdjacentStations(stationCode, lineStationBo.NextStation); 
                        lineStationBo.DistanceFromNext = next.Distance;
                        lineStationBo.TimeFromNext = next.Time;
                    }
                    catch (BO.BadAdjacentStationsException)
                    {
                        throw new BO.BadLineStationException(stationCode, lineId,
                                $"Missing distance and time information from station {lineStationBo.NextStation}");
                    }
                }
                return lineStationBo;
            }
            catch(DO.BadLineStationException ex)
            {
                throw new BO.BadLineStationException(stationCode, lineId, ex.Message);
            }
        }

        /// <summary>
        /// Add new line station
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

        /// <summary>
        /// Remove line station
        /// </summary>
        /// <param name="station"></param>
        public void RemoveLineStation(BO.LineStation station)
        {
            try
            {   /* A line must remain with at least two stations
                   and therefore it is not possible to delete a station
                   from a line that has only 2 left */
                if (GetLine(station.LineId).LineStations.Count() <= 2)
                    throw new BO.BadLineStationException(station.Station, station.LineId,
                        "A line must remain with at least two stations");
                
                var lineStationDo = station.CopyPropertiesToNew(typeof(DO.LineStation)) as DO.LineStation;
                dl.RemoveLineStation(lineStationDo);
            }
            catch (DO.BadLineStationException ex)
            {
                throw new BO.BadLineStationException(station.Station, station.LineId, ex.Message);
            }
        }

        /// <summary>
        /// Upadte a line station
        /// </summary>
        /// <param name="lineStation"></param>
        public void UpdateLineStation(BO.LineStation lineStation)
        {
            try
            {
                var lineStationDo = lineStation.CopyPropertiesToNew(typeof(DO.LineStation)) as DO.LineStation;
                dl.UpdateLineStation(lineStationDo);
            }
            catch(DO.BadLineStationException ex)
            {
                throw new BO.BadLineStationException(lineStation.Station, lineStation.LineId, ex.Message);
            }
        }

        /// <summary>
        /// When adding a line station we would like to check
        /// if there is data on distance and time from adjacent stations.
        /// If there is, we will update the next and previous station of this new station.
        /// If not, we will update the user. 
        /// In addition we will keep flags of what information is missing,
        /// in order to request it from the user
        /// </summary>
        /// <param name="lineStation">the new line station</param>
        /// <param name="prevMiss">flag if prev is missing</param>
        /// <param name="nextMiss">flag if next is missing</param>
        public void AddDistEndTimeToNewLineStation(BO.LineStation lineStation, out bool prevMiss, out bool nextMiss)
        {
            // Flags to know if there infromation of distance and time in the database
            prevMiss = nextMiss = false;

            BO.AdjacentStations nextStationAdst, prevStationAdst;
            try
            {
                nextStationAdst = GetAdjacentStations(lineStation.Station, lineStation.NextStation);
                lineStation.DistanceFromNext = nextStationAdst.Distance;
                lineStation.TimeFromNext = nextStationAdst.Time;

                // updade the next station with the new next station
                var prevStation = GetLineStation(lineStation.PrevStation, lineStation.LineId); 
                prevStation.NextStation = lineStation.Station;
                UpdateLineStation(prevStation);
            }
            catch (BO.BadAdjacentStationsException)
            {
                nextMiss = true;
            }
            try
            {
                prevStationAdst = GetAdjacentStations(lineStation.PrevStation, lineStation.Station);
                lineStation.DistanceFromPrev = prevStationAdst.Distance;
                lineStation.TimeFromPrev = prevStationAdst.Time;

                //updade the prev station with the new prev station
                var nextStation = GetLineStation(lineStation.PrevStation, lineStation.LineId);

                nextStation.PrevStation = lineStation.Station;
                UpdateLineStation(nextStation);
            }
            catch (BO.BadAdjacentStationsException)
            {
                prevMiss = true;
            }
            catch (BO.BadLineStationException ex)
            {
                throw ex;
            }
            if(prevMiss && nextMiss)
            {
                throw new BO.BadLineStationException(lineStation.Station, lineStation.LineId,
                            $"Missing distance and time\n" +
                            $"information from the stations:\n" +
                            $"{lineStation.PrevStation} and {lineStation.NextStation}");
            }
            else if (prevMiss)
            {
                throw new BO.BadLineStationException(lineStation.Station, lineStation.LineId,
                        $"Missing distance and time information from station {lineStation.PrevStation}");
            }
            else if (nextMiss)
            {
                throw new BO.BadLineStationException(lineStation.Station, lineStation.LineId,
                        $"Missing distance and time information from station {lineStation.NextStation}");
            }

        }           
        #endregion

        #region Line
        /// <summary>
        /// Get all line from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BO.Line> GetAllLines()
        {
            return from line in dl.GetAllLines()
                   let lineBo = GetLine(line.Id)
                   select lineBo;
        }

        /// <summary>
        /// Get all line from the database by some coondition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BO.Line> GetAllLinesBy(Predicate<BO.Line> predicate)
        {
            return from line in GetAllLines()
                   where predicate(line)
                   select line;
        }

        /// <summary>
        /// Get all lines thet pass in some station 
        /// </summary>
        /// <param name="stationCode">the station code of the station</param>
        /// <returns></returns>
        public IEnumerable<BO.Line> GetAllLinePassByStation(int stationCode)
        {
            var allLineStationsOfSpecific = from st
                                            in dl.GetAllLineStationBy(ls => ls.Station == stationCode)
                                            select st;
            return from st in allLineStationsOfSpecific
                   from line in GetAllLinesBy(l => st.LineId == l.Id)
                   select line;
        }

        /// <summary>
        /// Get a single line from the database
        /// build the line with a list of hes line stations
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add new line to the database
        /// </summary>
        /// <param name="newLine"></param>
        /// <returns>the id of this line that came from the run number in the database</returns>
        public int AddLine(BO.Line newLine)
        {
            #region Check if there is needed data on the database
            if (!dl.GetAllStations().Any(s => s.Code == newLine.FirstStation))
                throw new BO.BadLineException(newLine.Id, $"Station '{newLine.FirstStation}' does not exist");

            if (!dl.GetAllStations().Any(s => s.Code == newLine.LastStation))
                throw new BO.BadLineException(newLine.Id, $"Station '{newLine.LastStation}' does not exist");

            if (!dl.GetAllAdjacentStations().Any(s => s.Station1 == newLine.FirstStation
             && s.Station2 == newLine.LastStation))
                throw new BO.BadLineException(newLine.Id,
                    $"Missing distance and time information between {newLine.FirstStation}" +
                    $" and {newLine.LastStation}"); 
            #endregion

            var lineDo = newLine.CopyPropertiesToNew(typeof(DO.Line)) as DO.Line;
            return dl.AddLine(lineDo);
        }

        /// <summary>
        /// Upadate line
        /// </summary>
        /// <param name="line"></param>
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

        /// <summary>
        /// Remove line
        /// </summary>
        /// <param name="line"></param>
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
        /// <summary>
        /// Get all station from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BO.Station> GetAllStations()
        {
            return from station in dl.GetAllStations()
                   let stationBo = GetStation(station.Code)
                   select stationBo;
        }

        /// <summary>
        /// Get all the station from the database by some predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate)
        {
            return from station in GetAllStations()
                   where predicate(station)
                   select station;
        }

        /// <summary>
        /// Get a single station from the database
        /// build the station with the line that pass in her and the adjacent stations to her
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update station
        /// </summary>
        /// <param name="station"></param>
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

        /// <summary>
        /// Add new station to the database
        /// </summary>
        /// <param name="station"></param>
        public void AddStation(BO.Station station)
        {
            var stationDo = station.CopyPropertiesToNew(typeof(DO.Station)) as DO.Station;
            try
            {
                if (!validLatLon(stationDo.Latitude, stationDo.Longitude))
                    throw new BO.BadStationException(stationDo.Code,
                        "The location entered is outside the earth");
                
                dl.AddStation(stationDo);
            }
            catch (DO.BadStationException ex)
            {
                throw new BO.BadStationException(station.Code, ex.Message);
            }
        }

        // check valid location
        bool validLatLon(double lat, double lon)
        {
            return lat >= -90 && lat <= 90 && lon >= -180 && lon <= 180;
        }

        /// <summary>
        /// Remove station from the database
        /// </summary>
        /// <param name="station"></param>
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
        /// <summary>
        /// Add new adjacent stations to the database
        /// </summary>
        /// <param name="adjacentStations"></param>
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

        /// <summary>
        /// Get all adjacent stations of a spesific station
        /// </summary>
        /// <param name="stationCode"></param>
        /// <returns></returns>
        public IEnumerable<BO.AdjacentStations> GetAllAdjacentStationsOf(int stationCode)
        {
            return from adst in dl.GetAllAdjacentStations()
                   where adst.Station1 == stationCode
                   let adstBo = adst.CopyPropertiesToNew(typeof(BO.AdjacentStations)) as BO.AdjacentStations
                   select adstBo;
        }

        /// <summary>
        /// Get all adjacent stations from the database
        /// </summary>
        /// <param name="station1"></param>
        /// <param name="station2"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update adjacent stations
        /// </summary>
        /// <param name="adjacentStations"></param>
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

        #region LineTrip
        /// <summary>
        /// Add new line trip to the database
        /// </summary>
        /// <param name="lineTrip"></param>
        /// <returns></returns>
        public int AddLineTrip(BO.LineTrip lineTrip)
        {
            var lineTripDo = lineTrip.CopyPropertiesToNew(typeof(DO.LineTrip)) as DO.LineTrip;
            return dl.AddLineTrip(lineTripDo);
        }
        
        /// <summary>
        /// Get a single line trip from the database
        /// build the line trip with the line code and the last station of the line
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BO.LineTrip GetLineTrip(int id)
        {
            BO.LineTrip lineTripBo = new BO.LineTrip(); 
            try
            {
                var lineTripDo = dl.GetLineTrip(id);
                lineTripDo.CopyPropertiesTo(lineTripBo);
                var line = GetLine(lineTripBo.LineId);
                lineTripBo.LineCode = line.Code;
                lineTripBo.LastStation = line.LastStationName;
                return lineTripBo;
            }
            catch(DO.BadLineTripException ex)
            {
                throw new BO.BadLineTripException(id, ex.Message);
            }
        }

        /// <summary>
        /// Get all line trip of spesific station
        /// build each one with the arrival time to this station
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public IEnumerable<BO.LineTrip> GetAllLineTripsByStation(BO.Station station)
        {
            return from line in station.LinesPassBy
                   from lineTrip in GetAllLineTrips()
                   where lineTrip.LineId == line.Id
                   let lineTripRes = SetArrivalTimeToStation(station, lineTrip)
                   orderby lineTripRes.ArrivalTimeToStation
                   select lineTripRes;
        }

        BO.LineTrip SetArrivalTimeToStation(BO.Station station, BO.LineTrip lineTrip)
        {
            TimeSpan arrivalTime = TimeSpan.Zero;

            // Summarize the time of the line until it reaches the given station 
            foreach (var lineStation in GetAllLineStationsByLineID(lineTrip.LineId))
            {   
                arrivalTime += lineStation.TimeFromNext;  
                if (lineStation.Station == station.Code)
                    break;
            }
            
            lineTrip.ArrivalTimeToStation = lineTrip.CurrentTimeToStation = arrivalTime + lineTrip.StartAt;
            return lineTrip;
        }

        /// <summary>
        /// Get all line trip from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BO.LineTrip> GetAllLineTrips()
        {
            return from line in dl.GetAllLineTrips()
                   let lineBo = GetLineTrip(line.Id)
                   select lineBo;
        }

        /// <summary>
        /// Get all line trip by some predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<BO.LineTrip> GetAllLineTripsBy(Predicate<BO.LineTrip> predicate)
        {
            return from line in dl.GetAllLineTrips()
                   let lineBo = GetLineTrip(line.Id)
                   where predicate(lineBo)
                   select lineBo;
        }
        #endregion
    }
}
