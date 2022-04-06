using InventoryManagement.Core;
using InventoryManagement.Data;
using InventoryManagement.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using static InventoryManagement.ViewModel.ManageProductHistoryViewModel;

namespace InventoryManagement.ViewModel
{
    public class ManageProductViewModel : ObservableObject
    {
        #region [ Command ]

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get { return (_addCommand) ?? (_addCommand = new RelayCommand(Add)); }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return (_saveCommand) ?? (_saveCommand = new RelayCommand(Save)); }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get { return (_deleteCommand) ?? (_deleteCommand = new RelayCommand(Delete)); }
        }

        private ICommand _exportCommand;
        public ICommand ExportCommand
        {
            get { return (_exportCommand) ?? (_exportCommand = new RelayCommand(Export)); }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get { return (_searchCommand) ?? (_searchCommand = new RelayCommand(Search)); }
        }

        private ICommand _refreshCommand;
        public ICommand RefreshCommand
        {
            get { return (_refreshCommand) ?? (_refreshCommand = new RelayCommand(Refresh)); }
        }

        private ICommand _doubleClickCommand;
        public ICommand DoubleClickCommand
        {
            get { return (_doubleClickCommand) ?? (_doubleClickCommand = new RelayCommand(DoubleClick)); }
        }

        #endregion

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

        private Sensor _selectedDgData;
        public Sensor selectedDgData
        {
            get { return _selectedDgData; }
            set
            {
                if (value is null) return;

                Name = value.Name;
                SerialNum = value.SerialNum;
                Location = value.Location;
                Maker = value.Maker;
                EquipName = value.EquipName;
                EquipID = value.EquipID;
                ReceivingDay = value.ReceivingDay;
                Description = value.Description;
                MacAddress = value.MacAddress;
                ViewerVersion = value.ViewerVersion;
                AppVersion = value.AppVersion;
                SOMVersion = value.SOMVersion;

                _selectedDgData = value;
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

        private ObservableCollection<string> _searchItem;
        public ObservableCollection<string> searchItem
        {
            get { return _searchItem; }
            set
            {
                _searchItem = value;
                OnPropertyChanged();
            }
        }

        private string _selectedComboBox;
        public string selectedComboBox
        {
            get { return _selectedComboBox; }
            set
            {
                _selectedComboBox = value;
                OnPropertyChanged();
            }
        }

        private string _searchData;
        public string searchData
        {
            get { return _searchData; }
            set
            {
                _searchData = value;
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

        public class SearchItems
        {
            public string All, Name, SerialNum, Location, Maker, EquipName, EquipID;
        }

        ObservableCollection<Sensor> original;
        List<string> history;

        public ManageProductViewModel()
        {
            init();

            BindingDataGrid();

            if (File.Exists(Log.GetHistoryLogPath(nameof(Sensor))))
            {
                history = Log.ReadCsvHistory(nameof(Sensor));
            }
        }

        private void BindingDataGrid()
        {
            dgData = new ObservableCollection<Sensor>();
            original = new ObservableCollection<Sensor>();

            var data = Log.ReadCsv(nameof(Sensor));
            if (data.Count > 0) data.RemoveAt(0);

            for (int i = 0; i < data.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(data[i])) continue;
                dgData.Add(DataHandler.ConvertStringToProduct<Sensor>(data[i]));
                original.Add(DataHandler.ConvertStringToProduct<Sensor>(data[i]));
            }
        }

        private void init()
        {
            dgData = new ObservableCollection<Sensor>();
            original = new ObservableCollection<Sensor>();
            history = new List<string>();

            MakeComboBox();
        }

        private void MakeComboBox()
        {
            searchItem = new ObservableCollection<string>(DataHandler.MakeComboBoxItemList<SearchItems>());
            selectedComboBox = "All";
        }

        private void Add()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show($"Name을 입력해주세요");
                return;
            }
            else if (string.IsNullOrWhiteSpace(SerialNum))
            {
                MessageBox.Show($"SerialNum을 입력해주세요");
                return;
            }

            var product = new Sensor(Name, SerialNum, Location, Maker, EquipName, EquipID, ReceivingDay, Description, MacAddress, ViewerVersion, AppVersion, SOMVersion, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            foreach (var item in original)
            {
                if (DataHandler.CheckEqual(item, product))
                {
                    MessageBox.Show("중복되는 물품이 있으니 다른 값으로 변경해주세요");
                    return;
                }
            }

            dgData.Add(product);
            original.Add(product);

            PropertyInfo[] fields = typeof(Sensor).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            history.Add(DataHandler.ToCsvProperty(",", fields, product));
        }

        private void Save()
        {
            Log.WriteLog(nameof(Sensor), original);
            Log.WriteLog(nameof(History), history);

            MessageBox.Show("Saved");
        }

        private void Delete()
        {
            if (DataHandler.CheckNull(selectedDgData))
            {
                MessageBox.Show("지울 파일을 선택해주세요");
                return;
            }

            if (MessageBox.Show("정말 삭제하시겠습니까?", "정말로 삭제요?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                dgData.Remove(selectedDgData);

                Sensor ori = new Sensor();
                for (int i = 0; i < original.Count; i++)
                {
                    if (DataHandler.CheckEqual(original[i], selectedDgData))
                    {
                        ori = original[i];
                        original.RemoveAt(i);
                    }
                }

                selectedDgData = new Sensor();
            }
        }

        private void Export()
        {
            DataHandler.ExportFile(original);
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(searchData))
            {
                dgData = new ObservableCollection<Sensor>(original);
                return;
            }

            ObservableCollection<Sensor> ov = new ObservableCollection<Sensor>();

            for (int i = 0; i < original.Count; i++)
            {
                switch (selectedComboBox)
                {
                    case nameof(SearchItems.All):
                        if (original[i].Name.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        else if (original[i].SerialNum.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        else if (original[i].Location.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        else if (original[i].Maker.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        else if (original[i].EquipName.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        else if (original[i].EquipID.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.Name):
                        if (original[i].Name.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.SerialNum):
                        if (original[i].SerialNum.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.Location):
                        if (original[i].Location.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.Maker):
                        if (original[i].Maker.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.EquipName):
                        if (original[i].EquipName.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.EquipID):
                        if (original[i].EquipID.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                        break;

                    default:
                        break;
                }
            }

            dgData = new ObservableCollection<Sensor>(ov);
        }

        private void Refresh()
        {
            BindingDataGrid();
        }

        private void DoubleClick()
        {
            if (selectedDgData is null || string.IsNullOrWhiteSpace(selectedDgData.SerialNum)) return;

            ManageProductHistory container = new ManageProductHistory()
            {
                Title = $"{nameof(Sensor)} History",
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };

            container.GetHistory(selectedDgData.SerialNum);

            container.ShowDialog();
        }
    }
}
