using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class BadUsernameException : Exception
    {
        string _username;

        public BadUsernameException(string username, string message)
            : base(message) => _username = username;

        public override string ToString() => base.ToString() + $", bad username: {_username}";
    }

    public class BadBusException : Exception
    {
        int _licenseNum;

        public BadBusException(int licenseNum, string message)
            : base(message) => _licenseNum = licenseNum;

        public override string ToString() =>
            base.ToString() + $", bad bus License Number: {_licenseNum}";
    }

    public class BadLineException : Exception
    {
        int _id;

        public BadLineException(int id, string message)
            : base(message) => _id = id;

        public override string ToString() =>
            base.ToString() + $", bad Line Id: {_id}";
    }

    public class BadStationException : Exception
    {
        int _stationCode;

        public BadStationException(int stationCode, string message)
            : base(message) => _stationCode = stationCode;

        public override string ToString() =>
            base.ToString() + $", bad Station Code: {_stationCode}";
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
}
