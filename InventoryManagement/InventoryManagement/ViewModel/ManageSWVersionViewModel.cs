using InventoryManagement.Core;
using InventoryManagement.Data;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class ManageSWVersionViewModel : ObservableObject
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

        #endregion

        #region [ Binding ]

        private ObservableCollection<SWVersion> _dgData;
        public ObservableCollection<SWVersion> dgData
        {
            get { return _dgData; }
            set
            {
                _dgData = value;
                OnPropertyChanged();
            }
        }

        private SWVersion _selectedDgData;
        public SWVersion selectedDgData
        {
            get { return _selectedDgData; }
            set
            {
                if (value is null) return;

                Status = value.Status;
                Software = value.Software;
                Version = value.Version;
                Date = value.Date;
                Description = value.Description;

                _selectedDgData = value;
                OnPropertyChanged();
            }
        }

        private string _Status;
        public string Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                OnPropertyChanged();
            }
        }

        private string _Software;
        public string Software
        {
            get { return _Software; }
            set
            {
                _Software = value;
                OnPropertyChanged();
            }
        }

        private string _Version;
        public string Version
        {
            get { return _Version; }
            set
            {
                _Version = value;
                OnPropertyChanged();
            }
        }

        private string _Date;
        public string Date
        {
            get { return _Date; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _Date = string.Empty;
                }
                else
                {
                    _Date = DateTime.Parse(value).ToString("yyyy-MM/dd");
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

        public class SWVersion
        {
            public string Status { get; set; }
            public string Software { get; set; }
            public string Version { get; set; }
            public string Date { get; set; }
            public string Description { get; set; }

            public SWVersion(string Status, string Software, string Version, string Date, string Description)
            {
                this.Status = Status != null ? Status : "";
                this.Software = Software != null ? Software : "";
                this.Version = Version != null ? Version : "";
                this.Date = Date != null ? Date : "";
                this.Description = Description != null ? Description : "";
            }

            public SWVersion() { }
        }

        public class SearchItems
        {
            public string All, Status, Software;
        }

        ObservableCollection<SWVersion> original;

        public ManageSWVersionViewModel()
        {
            init();

            BindingDataGrid();
        }

        private void BindingDataGrid()
        {
            dgData = new ObservableCollection<SWVersion>();
            original = new ObservableCollection<SWVersion>();

            var data = Log.ReadCsv(nameof(SWVersion));
            if (data.Count > 0) data.RemoveAt(0);

            for (int i = 0; i < data.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(data[i])) continue;
                dgData.Add(DataHandler.ConvertStringToProduct<SWVersion>(data[i]));
                original.Add(DataHandler.ConvertStringToProduct<SWVersion>(data[i]));
            }
        }

        private void init()
        {
            dgData = new ObservableCollection<SWVersion>();
            original = new ObservableCollection<SWVersion>();

            MakeComboBox();
        }

        private void MakeComboBox()
        {
            searchItem = new ObservableCollection<string>(DataHandler.MakeComboBoxItemList<SearchItems>());
            selectedComboBox = "All";
        }

        private void Add()
        {
            if (string.IsNullOrWhiteSpace(Software))
            {
                MessageBox.Show($"Software를 입력해주세요");
                return;
            }

            var product = new SWVersion(Status, Software, Version, Date, Description);

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
            Log.WriteLog(nameof(SWVersion), original);

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
                selectedDgData = new SWVersion();
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
                dgData = new ObservableCollection<SWVersion>(original);
                return;
            }

            ObservableCollection<SWVersion> ov = new ObservableCollection<SWVersion>();

            for (int i = 0; i < original.Count; i++)
            {
                if (original[i].Software.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
                else if (original[i].Status.ToUpper().Contains(searchData.ToUpper())) ov.Add(original[i]);
            }

            dgData = new ObservableCollection<SWVersion>(ov);
        }

        private void Refresh()
        {
            BindingDataGrid();
        }
    }
}
