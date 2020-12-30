using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi
{
    public interface IBL
    {
        #region User
        BO.User GetUser(string userName);
        IEnumerable<BO.User> GetAllUsers();
        IEnumerable<BO.User> GetAllUsersBy(Predicate<BO.User> predicate);
        void AddUser(BO.User newUser);
        void RemmoveUser(BO.User user);
        void UpdateUser(BO.User user);
        #endregion

        #region Bus
        IEnumerable<BO.Bus> GetAllBuss();
        IEnumerable<BO.Bus> GetAllBussBy(Predicate<BO.Bus> predicate);
        BO.Bus GetBus(int licenseNum);
        void AddBus(BO.Bus bus);
        void UpdateBus(BO.Bus bus);
        void RemmoveBus(BO.Bus bus);
        #endregion

        #region Line
        IEnumerable<BO.Line> GetAllLines();
        IEnumerable<BO.Line> GetAllLinesBy(Predicate<BO.Line> predicate);
        BO.Line GetLine(int lineId);
        void AddLine(BO.Line newLine);
        void UpdateLine(BO.Line line);
        void RemmoveLine(BO.Line line);
        #endregion

        #region Station
        IEnumerable<BO.Station> GetAllStations();
        IEnumerable<BO.Station> GetAllStationsBy(Predicate<BO.Station> predicate);
        BO.Station GetStation(int code);
        void AddStation(BO.Station newStation);
        void UpdateStation(BO.Station station);
        void RemmoveStation(BO.Station station);
        #endregion

        IEnumerable<BO.LineStation> GetAllLineStationsByLineID(int lineId);
        IEnumerable<BO.Line> GetAllLinePassByStation(int stationCode);

    }
}
