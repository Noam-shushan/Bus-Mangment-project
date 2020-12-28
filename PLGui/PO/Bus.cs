using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace PO
{
    public class Bus : INotifyPropertyChanged
    {
        private BO.Enums.BusStatus _status;
        private SolidColorBrush _statusColor;
        private bool _isReady;
        private double _kilometers;
        public string LicenseNum { get; set; }
        public DateTime FromDate { get; set; }
        public double TotalTrip 
        {
            get => _kilometers;
            set
            {
                _kilometers += value;
                KilometersAfterTreatment += value;
                KilometersAfterFueling += value;
            }
        }
        public double FuelRemain { get; set; }
        public double KilometersAfterFueling { get; set; }
        public double KilometersAfterTreatment { get; set; }

        /// <summary>
        /// Flag to know if the bus is ready
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
        /// Color matching status
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
        /// Current status of the bus
        /// </summary>
        public BO.Enums.BusStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                IsReady = Status == BO.Enums.BusStatus.READY;
                switch (value)
                {
                    case BO.Enums.BusStatus.READY:
                        StatusColor = Brushes.Green;
                        break;
                    case BO.Enums.BusStatus.REFUELING:
                        StatusColor = Brushes.Orange;
                        break;
                    case BO.Enums.BusStatus.RIDE:
                        StatusColor = Brushes.Blue;
                        break;
                    case BO.Enums.BusStatus.TREATMENT:
                        StatusColor = Brushes.Red;
                        break;

                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Status"));
            }
        }

        public override string ToString()
        {
            return $"License Number: {LicenseNum}\n" +
                $"Total kilometers: {TotalTrip}\n" +
                $"Kilometers since last treatment: {KilometersAfterTreatment}\n" +
                $"Start activity: {FromDate.Date:dd/MM/yyyy}";
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
