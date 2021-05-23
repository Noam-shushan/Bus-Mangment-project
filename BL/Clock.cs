using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Notify();
    }

    public interface IObserver
    {
        void Update(ISubject subject);
    }

    public class Clock2 : ISubject, IObserver
    {
        List<IObserver> _observers;

        TimeSpan _currentTime;
        public TimeSpan CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                Notify();
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
        public static Clock2 Instance { get; } = new Clock2();

        Clock2() 
        {
            _observers = new List<IObserver>();
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Notify()
        {
            _observers.ForEach(o => o.Update(this));
        }

        public void Update(ISubject subject)
        {
            if(subject is Clock2)
            {
                CurrentClock += CurrentTime;
            }
        }
        #endregion
    }

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
