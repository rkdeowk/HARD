using InventoryManagement.Core;
using InventoryManagement.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        #endregion

        #region [ Binding ]

        private ObservableCollection<Product> _dgData;
        public ObservableCollection<Product> dgData
        {
            get { return _dgData; }
            set
            {
                _dgData = value;
                OnPropertyChanged();
            }
        }

        private Product _selectedDgData;
        public Product selectedDgData
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

        public class Product
        {
            public string Name { get; set; }
            public string SerialNum { get; set; }
            public string Location { get; set; }
            public string Maker { get; set; }
            public string EquipName { get; set; }
            public string EquipID { get; set; }
            public string ReceivingDay { get; set; }
            public string Description { get; set; }

            public Product(string Name, string SerialNum, string Location, string Maker, string EquipName, string EquipID, string ReceivingDay, string Description)
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

            public Product() { }
        }

        public class SearchItems
        {
            public string All, Name, SerialNum, Location, Maker, EquipName, EquipID;
        }

        ObservableCollection<Product> original;

        private string logPath;
        private string backupPath;

        public ManageProductViewModel()
        {
            init();

            BindingDataGrid();
        }

        private void BindingDataGrid()
        {
            dgData = new ObservableCollection<Product>();
            original = new ObservableCollection<Product>();

            var data = DataHandler.ReadCsv(logPath);
            if (data.Count > 0) data.RemoveAt(0);

            for (int i = 0; i < data.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(data[i])) continue;
                dgData.Add(ConvertStringToProduct(data[i]));
                original.Add(ConvertStringToProduct(data[i]));
            }
        }

        private void init()
        {
            logPath = $@"{Environment.CurrentDirectory}\log.csv";
            backupPath = @$"{Environment.CurrentDirectory}\backupLog";

            dgData = new ObservableCollection<Product>();
            original = new ObservableCollection<Product>();

            Directory.CreateDirectory(backupPath);

            MakeComboBox();
        }

        private void MakeComboBox()
        {
            List<string> comboBoxItem = new List<string>();

            var fieldInfos = typeof(SearchItems).GetFields();
            foreach (var field in fieldInfos)
            {
                comboBoxItem.Add(field.Name);
            }

            searchItem = new ObservableCollection<string>(comboBoxItem);
            selectedComboBox = "All";
        }

        private void Add()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show($"Name을 입력해주세요");
                return;
            }

            var product = new Product(Name, SerialNum, Location, Maker, EquipName, EquipID, ReceivingDay, Description);

            foreach (var item in original)
            {
                if (CheckEqual(item, product))
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
            DataHandler.WriteCsv(logPath, original);

            var k = @$"{backupPath}\{DateTime.Now.ToString("yyyy-MM-dd")}_log.csv";
            DataHandler.WriteCsv(k, original);

            MessageBox.Show("Saved");
        }

        private void Delete()
        {
            if (CheckNull(selectedDgData))
            {
                MessageBox.Show("지울 파일을 선택해주세요");
                return;
            }

            if (MessageBox.Show("정말 삭제하시겠습니까?", "정말로 삭제요?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                dgData.Remove(selectedDgData);
                original.Remove(selectedDgData);
                selectedDgData = new Product();
            }
        }

        private void Export()
        {
            if (MessageBox.Show("저장해야 반영되니, 저장을 안하셨으면 저장 먼저 해주시기 바랍니다.\n 저장하셨습니까?", "정말요?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Csv file (*.csv)|*.csv";
                if (sfd.ShowDialog() == true)
                {
                    DataHandler.WriteCsv(sfd.FileName, original);
                }
            }
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(searchData))
            {
                dgData = new ObservableCollection<Product>(original);
                return;
            }

            ObservableCollection<Product> ov = new ObservableCollection<Product>();

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

            dgData = new ObservableCollection<Product>(ov);
        }

        private void Refresh()
        {
            BindingDataGrid();
        }

        private Product ConvertStringToProduct(string str)
        {
            Product product = new Product();
            var s = str.Split(',');
            int idx = 0;

            var propertyInfos = typeof(Product).GetProperties();
            foreach (var property in propertyInfos)
            {
                property.SetValue(product, s[idx++]);
            }

            return product;
        }

        private bool CheckEqual(Product p1, Product p2)
        {
            var propertyInfos = typeof(Product).GetProperties();

            foreach (var property in propertyInfos)
            {
                var a = property.GetValue(p1, null) == null ? "" : property.GetValue(p1, null).ToString();
                var b = property.GetValue(p1, null) == null ? "" : property.GetValue(p2, null).ToString();
                if (a != b) return false;

            }
            return true;
        }

        private bool CheckNull(Product product)
        {
            if (product == null) return true;

            var propertyInfos = typeof(Product).GetProperties();

            foreach (var property in propertyInfos)
            {
                if (property.GetValue(product, null) != null) return false;
            }

            return true;
        }
    }
}
