using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public interface IDaL
    {
        #region Bus
        /// <summary>
        /// Get all Buss from my Data Source
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Bus> GetAllBuss();

        /// <summary>
        /// Get all buss by condition from my Data Source
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<DO.Bus> GetAllBussBy(Predicate<DO.Bus> predicate);

        /// <summary>
        /// Get a singal bus from my Data Source
        /// </summary>
        /// <param name="licenseNum">license number ,the entity key></param>
        /// <returns></returns>
        DO.Bus GetBus(int licenseNum);

        /// <summary>
        /// Add a new bus to my Data Source
        /// </summary>
        /// <param name="bus"></param>
        void AddBus(DO.Bus bus);

        /// <summary>
        /// Update Bus in my Data Source
        /// </summary>
        /// <param name="bus"></param>
        void UpdateBus(DO.Bus bus);

        /// <summary>
        /// Remove bus from my Data Source
        /// </summary>
        /// <param name="bus"></param>
        void RemoveBus(DO.Bus bus);
        #endregion

        #region Line
        /// <summary>
        /// Get all line from the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Line> GetAllLines();

        /// <summary>
        /// Get all line from the database by some coondition
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate);

        /// <summary>
        /// Get a single line from the database
        /// </summary>
        /// <param name="lineId"></param>
        /// <returns></returns>
        DO.Line GetLine(int lineId);

        /// <summary>
        /// Add new line to the database
        /// </summary>
        /// <param name="line"></param>
        /// <returns>the id of this line that came from the run number in the database</returns>
        int AddLine(DO.Line newLine);

        /// <summary>
        /// Upadate line
        /// </summary>
        /// <param name="line"></param>
        void UpdateLine(DO.Line line);

        /// <summary>
        /// Remove line
        /// </summary>
        /// <param name="line"></param>
        void RemoveLine(DO.Line line);
        #endregion

        #region Station
        /// <summary>
        /// Get all station from the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.Station> GetAllStations();

        /// <summary>
        /// Get all the station from the database by some predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate);

        /// <summary>
        /// Get a single station from the database
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        DO.Station GetStation(int code);

        /// <summary>
        /// Add new station to the database
        /// </summary>
        /// <param name="station"></param>
        void AddStation(DO.Station newStation);

        /// <summary>
        /// Update station
        /// </summary>
        /// <param name="station"></param>
        void UpdateStation(DO.Station station);

        /// <summary>
        /// Remove station from the database
        /// </summary>
        /// <param name="station"></param>
        void RemoveStation(DO.Station station);
        #endregion

        #region LineStation
        /// <summary>
        /// Get all line stations from the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.LineStation> GetAllLineStation();

        /// <summary>
        /// Get all line stations from the database by some predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<DO.LineStation> GetAllLineStationBy(Predicate<DO.LineStation> predicate);

        /// <summary>
        /// Get a single line station by is key (station code, line id)
        /// </summary>
        /// <param name="stationCode"></param>
        /// <param name="lineId"></param>
        /// <returns></returns>
        DO.LineStation GetLineStation(int stationCode, int lineId);

        /// <summary>
        /// Add new line station
        /// </summary>
        /// <param name="lineStation"></param>
        void AddLineStation(DO.LineStation lineStation);

        /// <summary>
        /// Upadte a line station
        /// </summary>
        /// <param name="lineStation"></param>
        void UpdateLineStation(DO.LineStation lineStation);

        /// <summary>
        /// Remove line station
        /// </summary>
        /// <param name="lineStation"></param>
        void RemoveLineStation(DO.LineStation lineStation);

        /// <summary>
        /// In deleting a line we want to delete all the stations
        /// As opposed to a specific deletion of a station
        /// Therefore we will not set conditions and we will not change other
        /// stations on the line because everyone will delete
        /// </summary>
        /// <param name="lineStation"></param>
        void RemoveLineStationOnRemoveline(DO.LineStation lineStation);
        #endregion

        #region User
        /// <summary>
        /// Get all users from the database
        /// </summary>
        /// <returns></returns>
        IEnumerable<DO.User> GetAllUsers();

        /// <summary>
        /// Get all the usesrs from the database by some predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<DO.User> GetAllUsersBy(Predicate<DO.User> predicate);

        /// <summary>
        /// Get a single user from the database
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        DO.User GetUser(string userName);

        /// <summary>
        /// Add new user to the database
        /// </summary>
        /// <param name="newUser"></param>
        void AddUser(DO.User newUser);

        /// <summary>
        /// Upadate user
        /// </summary>
        /// <param name="userName"></param>
        void UpdateUser(DO.User userName);

        /// <summary>
        /// Remove user
        /// </summary>
        /// <param name="user"></param>
        void RemoveUser(DO.User user);
        #endregion

        #region AdjacentStations
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStations();
        IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate);
        DO.AdjacentStations GetAdjacentStations(int station1, int station2);
        void AddAdjacentStations(DO.AdjacentStations adjacentStations);
        void RemoveAdjacentStations(DO.AdjacentStations adjacentStations);
        void UpdateAdjacentStations(DO.AdjacentStations adjacentStations);
        #endregion

        #region LineTrip
        int AddLineTrip(DO.LineTrip lineTrip);
        DO.LineTrip GetLineTrip(int id);
        IEnumerable<DO.LineTrip> GetAllLineTrips();
        IEnumerable<DO.LineTrip> GetAllLineTripsBy(Predicate<DO.LineTrip> predicate); 
        #endregion

    }
}
