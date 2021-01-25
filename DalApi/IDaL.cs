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
        IEnumerable<DO.Bus> GetAllBuss();
        IEnumerable<DO.Bus> GetAllBussBy(Predicate<DO.Bus> predicate);
        DO.Bus GetBus(int licenseNum);
        void AddBus(DO.Bus bus);
        void UpdateBus(DO.Bus bus);
        void RemoveBus(DO.Bus bus);
        #endregion

        #region Line
        IEnumerable<DO.Line> GetAllLines();
        IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate);
        DO.Line GetLine(int lineId);
        int AddLine(DO.Line newLine);
        void UpdateLine(DO.Line line);
        void RemoveLine(DO.Line line);
        #endregion

        #region Station
        IEnumerable<DO.Station> GetAllStations();
        IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate);
        DO.Station GetStation(int code);
        void AddStation(DO.Station newStation);
        void UpdateStation(DO.Station station);
        void RemoveStation(DO.Station station);
        #endregion

        #region LineStation
        IEnumerable<DO.LineStation> GetAllLineStation();
        IEnumerable<DO.LineStation> GetAllLineStationBy(Predicate<DO.LineStation> predicate);
        DO.LineStation GetLineStation(int stationCode, int lineId);
        void AddLineStation(DO.LineStation lineStation);
        void UpdateLineStation(DO.LineStation lineStation);
        void RemoveLineStation(DO.LineStation lineStation);
        void RemoveLineStationOnRemoveline(DO.LineStation lineStation);
        #endregion

        #region User
        IEnumerable<DO.User> GetAllUsers();
        IEnumerable<DO.User> GetAllUsersBy(Predicate<DO.User> predicate);
        DO.User GetUser(string userName);
        void AddUser(DO.User newUser);
        void UpdateUser(DO.User userName);
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
