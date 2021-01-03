using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public class BadUsernameException : Exception
    {
        string _username;
        
        public BadUsernameException(string username, string message) 
            : base(message) => _username = username;
        
        public override string ToString() => base.ToString() + $", bad username: {_username}";
    }

    public class BadLineStationException : Exception
    {
        int _stationCode;
        int _lineId;

        public BadLineStationException(int stationCode, int lineId, string message)
            : base(message)
        {
            _stationCode = stationCode;
            _lineId = lineId;
        }

        public override string ToString() => 
            base.ToString() + $", bad Line Station: {_stationCode}, {_lineId}";
    }
    
    public class BadStationException : Exception
    {
        int _stationCode;

        public BadStationException(int stationCode, string message)
            : base(message) => _stationCode = stationCode;

        public override string ToString() =>
            base.ToString() + $", bad Station Code: {_stationCode}";
    }

    public class BadLineException : Exception
    {
        int _id;

        public BadLineException(int id, string message)
            : base(message) => _id = id;

        public override string ToString() =>
            base.ToString() + $", bad Line Id: {_id}";
    }

    public class BadBusException : Exception
    {
        int _licenseNum;

        public BadBusException(int licenseNum, string message)
            : base(message) => _licenseNum = licenseNum;

        public override string ToString() =>
            base.ToString() + $", bad bus License Number: {_licenseNum}";
    }

    public class BadAdjacentStationsException : Exception
    {
        int _station1;
        int _station2;

        public BadAdjacentStationsException(int station1, int station2, string message)
            : base(message)
        {
            _station1 = station1;
            _station2 = station2;
        }

        public override string ToString() =>
            base.ToString() + $", bad adjacent stations: {_station1}, {_station2}";
    }
}
