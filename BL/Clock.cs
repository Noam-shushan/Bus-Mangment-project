using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Clock
    {
        TimeSpan _currentTime;
        public TimeSpan CurrentTime 
        { 
            get => _currentTime;
            set
            {
                _currentTime = value;
                CurrentClock += _currentTime;
            } 
        }
        public DateTime CurrentClock { get; set; }
        public int Rate { get; set; }
        public bool IsClockRunning { get; set; }
        public TimeSpan Time
        { 
            get => CurrentClock.TimeOfDay; 
        }

        #region singelton
        public static Clock Instance { get; } = new Clock();

        Clock() { } 
        #endregion



    }
}
