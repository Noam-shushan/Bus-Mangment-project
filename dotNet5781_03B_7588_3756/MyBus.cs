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
    /// <summary>
    /// 
    /// </summary>
    public class MyBus : Bus, INotifyPropertyChanged
    {
        public enum Status { READY, RIDE, REFUELING, TREATMENT }
        private Status _currentStatus;
        private SolidColorBrush _statusColor;
        private bool _isReady;

        /// <summary>
        /// 
        /// </summary>
        public bool IsReady 
        {
            get => _isReady;
            set
            {
                _isReady = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsReady"));
            } 
        }
        /// <summary>
        /// 
        /// </summary>
        public SolidColorBrush StatusColor
        {
            get => _statusColor;
            set
            {
                _statusColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusColor"));
            }
        }
        /// <summary>
        /// 
        /// </summary>
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
                        StatusColor = Brushes.Orange;
                        break;
                    case Status.RIDE:
                        StatusColor = Brushes.Blue;
                        break;
                    case Status.TREATMENT:
                        StatusColor = Brushes.Red;
                        break;

                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentStatus"));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="licenseNumber"></param>
        /// <param name="startActivity"></param>
        public MyBus(string licenseNumber, DateTime? startActivity) 
            : base(licenseNumber, startActivity) 
        {
            CurrentStatus = Status.READY;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="licenseNumber"></param>
        /// <param name="startActivity"></param>
        /// <param name="kilometers"></param>
        /// <param name="lastTreatment"></param>
        /// <param name="kilometersAfterTreatment"></param>
        public MyBus(string licenseNumber, DateTime? startActivity,
            int kilometers, DateTime? lastTreatment, int kilometersAfterTreatment)
            : base(licenseNumber, startActivity)
        {
            CurrentStatus = Status.READY;
            LastTreatment = lastTreatment;
            Kilometers = kilometers;
            KilometersAfterTreatment = kilometersAfterTreatment;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
