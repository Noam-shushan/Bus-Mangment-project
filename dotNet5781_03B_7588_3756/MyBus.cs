using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using dotNet5781_01_7588_3756;

namespace dotNet5781_03B_7588_3756
{
    public class MyBus : Bus, INotifyPropertyChanged
    {
        public enum Status { READY, RIDE, REFUELING, TREATMENT }
        private Status _currentStatus;
        private SolidColorBrush _statusColor;

        public SolidColorBrush StatusColor
        {
            get => _statusColor;
            set
            {
                _statusColor = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentStatus"));
            }
        }

        public Status CurrentStatus
        {
            get => _currentStatus;
            set
            {
                _currentStatus = value;
                IsReady = CurrentStatus == Status.READY;
                switch (value)
                {
                    case Status.READY:
                        StatusColor = Brushes.Green;
                        break;
                    case Status.REFUELING:
                        StatusColor = Brushes.Yellow;
                        break;
                    case Status.RIDE:
                        StatusColor = Brushes.Blue;
                        break;
                    case Status.TREATMENT:
                        StatusColor = Brushes.Red;
                        break;

                }
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("CurrentStatus"));
            }
        }
        public MyBus(string licenseNumber, DateTime? startActivity) : base(licenseNumber, startActivity) { }

        public MyBus(string licenseNumber, DateTime? startActivity,
            int kilometers, DateTime? lastTreatment, int kilometersAfterTreatment)
            : base(licenseNumber, startActivity)
        {
            CurrentStatus = Status.READY;
            _lastTreatment = lastTreatment;
            Kilometers = kilometers;
            KilometersAfterTreatment = kilometersAfterTreatment;
        }

        public bool IsReady { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
