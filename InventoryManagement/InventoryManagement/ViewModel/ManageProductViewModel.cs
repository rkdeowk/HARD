using InventoryManagement.Core;
using InventoryManagement.Data;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

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

        private ICommand _historyCommand;
        public ICommand HistoryCommand
        {
            get { return (_historyCommand) ?? (_historyCommand = new RelayCommand(History)); }
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
                if (string.IsNullOrWhiteSpace(value)) return;
                _ReceivingDay = DateTime.Parse(value).ToString("yyyy-MM/dd");
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

            public Sensor(string Name, string SerialNum, string Location, string Maker, string EquipName, string EquipID, string ReceivingDay, string Description)
            {
                this.Name = Name != null ? Name : "";
                this.SerialNum = SerialNum != null ? SerialNum : "";
                this.Location = Location != null ? Location : "";
                this.Maker = Maker != null ? Maker : "";
                this.EquipName = EquipName != null ? EquipName : "";
                this.EquipID = EquipID != null ? EquipID : "";
                this.ReceivingDay = ReceivingDay != null ? ReceivingDay : "";
                this.Description = Description != null ? Description : "";
            }

            public Sensor() { }
        }

        public class SearchItems
        {
            public string All, Name, SerialNum, Location, Maker, EquipName, EquipID;
        }

        ObservableCollection<Sensor> original;

        public ManageProductViewModel()
        {
            init();

            BindingDataGrid();
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

            var product = new Sensor(Name, SerialNum, Location, Maker, EquipName, EquipID, ReceivingDay, Description);

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
        }

        private void Save()
        {
            Log.WriteLog(nameof(Sensor), original);

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
                for (int i = 0; i < original.Count; i++)
                {
                    if (DataHandler.CheckEqual(original[i], selectedDgData))
                    {
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
                        if (original[i].Name.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
                        else if (original[i].SerialNum.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
                        else if (original[i].Location.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
                        else if (original[i].Maker.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
                        else if (original[i].EquipName.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
                        else if (original[i].EquipID.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.Name):
                        if (original[i].Name.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.SerialNum):
                        if (original[i].SerialNum.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.Location):
                        if (original[i].Location.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.Maker):
                        if (original[i].Maker.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.EquipName):
                        if (original[i].EquipName.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
                        break;

                    case nameof(SearchItems.EquipID):
                        if (original[i].EquipID.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
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

        private void History()
        {

        }
    }
}
