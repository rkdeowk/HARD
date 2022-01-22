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

        private ICommand _resetCommand;
        public ICommand ResetCommand
        {
            get { return (_resetCommand) ?? (_resetCommand = new RelayCommand(Reset)); }
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
                this.Name = Name;
                this.SerialNum = SerialNum;
                this.Location = Location;
                this.Maker = Maker;
                this.EquipName = EquipName;
                this.EquipID = EquipID;
                this.ReceivingDay = ReceivingDay;
                this.Description = Description;
            }

            public Product() { }
        }

        ObservableCollection<Product> original;

        private string logPath;
        private string backupPath;

        public ManageProductViewModel()
        {
            init();

            var db = DataHandler.ReadCsv(logPath);
            if (db.Count > 0) db.RemoveAt(0);

            BindingDataGrid(db);
        }

        private void BindingDataGrid(List<string> data)
        {

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
            List<string> item = new List<string>();

            item.Add("All");
            item.Add("Name");
            item.Add("SerialNum");
            item.Add("Location");
            item.Add("Maker");
            item.Add("EquipName");
            item.Add("EquipID");

            searchItem = new ObservableCollection<string>(item);
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
            if (selectedDgData == null) return;

            string msg =
                $"Name : {selectedDgData.Name}\n" +
                $"SerialNum : {selectedDgData.SerialNum}\n" +
                $"Location : {selectedDgData.Location}\n" +
                $"Maker : {selectedDgData.Maker}\n" +
                $"EquipName : {selectedDgData.EquipName}\n" +
                $"EquipID : {selectedDgData.EquipID}\n" +
                $"ReceivingDay : {selectedDgData.ReceivingDay}\n" +
                $"Description : {selectedDgData.Description}\n\n" + "정말 삭제하시겠습니까?";

            if (MessageBox.Show(msg, "정말로 삭제요?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                dgData.Remove(selectedDgData);
                original.Remove(selectedDgData);
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
                    case "All":
                        dgData = new ObservableCollection<Product>(original);
                        return;

                    case nameof(Product.Name):
                        if (original[i].Name == searchData) ov.Add(original[i]);
                        break;

                    case nameof(Product.SerialNum):
                        if (original[i].SerialNum == searchData) ov.Add(original[i]);
                        break;

                    case nameof(Product.Location):
                        if (original[i].Location == searchData) ov.Add(original[i]);
                        break;

                    case nameof(Product.Maker):
                        if (original[i].Maker == searchData) ov.Add(original[i]);
                        break;

                    case nameof(Product.EquipName):
                        if (original[i].EquipName == searchData) ov.Add(original[i]);
                        break;

                    case nameof(Product.EquipID):
                        if (original[i].EquipID == searchData) ov.Add(original[i]);
                        break;

                    default:
                        break;
                }
            }

            dgData = new ObservableCollection<Product>(ov);
        }

        private void Reset()
        {
            dgData = new ObservableCollection<Product>(original);
        }

        private Product ConvertStringToProduct(string str)
        {
            Product product = new Product();

            var s = str.Split(',');

            int idx = 0;
            product.Name = s[idx++];
            product.SerialNum = s[idx++];
            product.Location = s[idx++];
            product.Maker = s[idx++];
            product.EquipName = s[idx++];
            product.EquipID = s[idx++];
            product.ReceivingDay = s[idx++];
            product.Description = s[idx++];

            return product;
        }

        private bool CheckEqual(Product p1, Product p2)
        {
            if (p1.Name != p2.Name) return false;
            if (p1.SerialNum != p2.SerialNum) return false;
            if (p1.Location != p2.Location) return false;
            if (p1.Maker != p2.Maker) return false;
            if (p1.EquipName != p2.EquipName) return false;
            if (p1.EquipID != p2.EquipID) return false;
            if (p1.ReceivingDay != p2.ReceivingDay) return false;
            if (p1.Description != p2.Description) return false;
            return true;
        }
    }
}
