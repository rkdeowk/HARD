using InventoryManagement.Core;
using InventoryManagement.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace InventoryManagement.ViewModel
{
    public class ManageProductHistoryViewModel : ObservableObject
    {
        #region [ Binding ]

        private ObservableCollection<Sensor> _dgData;
        public ObservableCollection<Sensor> dgData
        {
            get { return _dgData; }
            set
            {
                _dgData = value;
                OnPropertyChanged();
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged();
            }
        }

        private string _SerialNum;
        public string SerialNum
        {
            get { return _SerialNum; }
            set
            {
                _SerialNum = value;
                OnPropertyChanged();
            }
        }

        private string _Location;
        public string Location
        {
            get { return _Location; }
            set
            {
                _Location = value;
                OnPropertyChanged();
            }
        }

        private string _Maker;
        public string Maker
        {
            get { return _Maker; }
            set
            {
                _Maker = value;
                OnPropertyChanged();
            }
        }

        private string _EquipName;
        public string EquipName
        {
            get { return _EquipName; }
            set
            {
                _EquipName = value;
                OnPropertyChanged();
            }
        }

        private string _EquipID;
        public string EquipID
        {
            get { return _EquipID; }
            set
            {
                _EquipID = value;
                OnPropertyChanged();
            }
        }

        private string _ReceivingDay;
        public string ReceivingDay
        {
            get { return _ReceivingDay; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _ReceivingDay = string.Empty;
                }
                else
                {
                    _ReceivingDay = DateTime.Parse(value).ToString("yyyy-MM/dd");
                }
                OnPropertyChanged();
            }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set
            {
                _Description = value;
                OnPropertyChanged();
            }
        }

        private string _MacAddress;
        public string MacAddress
        {
            get { return _MacAddress; }
            set
            {
                _MacAddress = value;
                OnPropertyChanged();
            }
        }

        private string _ViewerVersion;
        public string ViewerVersion
        {
            get { return _ViewerVersion; }
            set
            {
                _ViewerVersion = value;
                OnPropertyChanged();
            }
        }

        private string _AppVersion;
        public string AppVersion
        {
            get { return _AppVersion; }
            set
            {
                _AppVersion = value;
                OnPropertyChanged();
            }
        }

        private string _SOMVersion;
        public string SOMVersion
        {
            get { return _SOMVersion; }
            set
            {
                _SOMVersion = value;
                OnPropertyChanged();
            }
        }

        private string _Date;
        public string Date
        {
            get { return _Date; }
            set
            {
                _Date = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public class Sensor
        {
            public string Name { get; set; }
            public string SerialNum { get; set; }
            public string Location { get; set; }
            public string Maker { get; set; }
            public string EquipName { get; set; }
            public string EquipID { get; set; }
            public string ReceivingDay { get; set; }
            public string Description { get; set; }
            public string MacAddress { get; set; }
            public string ViewerVersion { get; set; }
            public string AppVersion { get; set; }
            public string SOMVersion { get; set; }
            public string Date { get; set; }

            public Sensor(string Name, string SerialNum, string Location, string Maker, string EquipName, string EquipID, string ReceivingDay, string Description, string MacAddress, string ViewerVersion, string AppVersion, string SOMVersion, string Date)
            {
                this.Name = Name != null ? Name : "";
                this.SerialNum = SerialNum != null ? SerialNum : "";
                this.Location = Location != null ? Location : "";
                this.Maker = Maker != null ? Maker : "";
                this.EquipName = EquipName != null ? EquipName : "";
                this.EquipID = EquipID != null ? EquipID : "";
                this.ReceivingDay = ReceivingDay != null ? ReceivingDay : "";
                this.Description = Description != null ? Description : "";
                this.MacAddress = MacAddress != null ? MacAddress : "";
                this.ViewerVersion = ViewerVersion != null ? ViewerVersion : "";
                this.AppVersion = AppVersion != null ? AppVersion : "";
                this.SOMVersion = SOMVersion != null ? SOMVersion : "";
                this.Date = Date != null ? Date : "";
            }

            public Sensor() { }
        }

        private List<string> history;

        private string SerialNumber;
        public ManageProductHistoryViewModel()
        {
            SerialNumber = "";
        }

        public void GetHistoryData(string str)
        {
            SerialNumber = str;

            if (!File.Exists(Log.GetHistoryLogPath(nameof(Sensor))))
            {
                history = Log.ReadCsv(nameof(Sensor));
            }
            else
            {
                history = Log.ReadCsvHistory(nameof(Sensor));
            }

            BindingDataGrid();
        }

        private void BindingDataGrid()
        {
            dgData = new ObservableCollection<Sensor>();

            for (int i = 0; i < history.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(history[i])) continue;
                var sensor = DataHandler.ConvertStringToProduct<Sensor>(history[i]);
                if(sensor.SerialNum == SerialNumber)
                {
                    dgData.Add(sensor);
                }
            }
        }
    }
}
