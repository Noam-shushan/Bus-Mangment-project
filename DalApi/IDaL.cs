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
        void RemmoveBus(DO.Bus bus);
        #endregion

        #region Line
        IEnumerable<DO.Line> GetAllLines();
        IEnumerable<DO.Line> GetAllLinesBy(Predicate<DO.Line> predicate);
        DO.Line GetLine(int lineId);
        void AddLine(DO.Line newLine);
        void UpdateLine(DO.Line line);
        void RemmoveLine(DO.Line line);
        #endregion

        #region Station
        IEnumerable<DO.Station> GetAllStations();
        IEnumerable<DO.Station> GetAllStationsBy(Predicate<DO.Station> predicate);
        DO.Station GetStation(int code);
        void AddStation(DO.Station newStation);
        void UpdateStation(DO.Station station);
        void RemmoveStation(DO.Station station);
        #endregion

        #region LineStation
        IEnumerable<DO.LineStation> GetAllLineStation();
        IEnumerable<DO.LineStation> GetAllLineStationBy(Predicate<DO.LineStation> predicate);
        DO.LineStation GetLineStation(int stationCode, int lineId);
        void AddLineStation(DO.LineStation lineStation);
        void UpdateLineStation(DO.LineStation lineStation);
        void RemmoveLineStation(DO.LineStation lineStation);
        #endregion

        #region User
        IEnumerable<DO.User> GetAllUsers();
        IEnumerable<DO.User> GetAllUsersBy(Predicate<DO.User> predicate);
        DO.User GetUser(string userName);
        void AddUser(DO.User newUser);
        void UpdateUser(DO.User userName);
        void RemmoveUser(DO.User user);
        #endregion

        #region AdjacentStations
        //IEnumerable<DO.AdjacentStations> GetAllAdjacentStations();
        //IEnumerable<DO.AdjacentStations> GetAllAdjacentStationsBy(Predicate<DO.AdjacentStations> predicate);
        //void AddAdjacentStations(DO.AdjacentStations adjacentStations);
        //void UpdateAdjacentStations(DO.AdjacentStations adjacentStations);
        #endregion
    }
}
