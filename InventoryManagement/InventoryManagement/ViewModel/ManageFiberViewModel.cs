using InventoryManagement.Core;
using InventoryManagement.Data;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace InventoryManagement.ViewModel
{
    public class ManageFiberViewModel : ObservableObject
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

        private ObservableCollection<Fiber> _dgData;
        public ObservableCollection<Fiber> dgData
        {
            get { return _dgData; }
            set
            {
                _dgData = value;
                OnPropertyChanged();
            }
        }

        private Fiber _selectedDgData;
        public Fiber selectedDgData
        {
            get { return _selectedDgData; }
            set
            {
                if (value is null) return;

                CISCode = value.CISCode;
                Type = value.Type;
                InputSpec = value.InputSpec;
                OutputSpec = value.OutputSpec;
                Length = value.Length;
                Quantity = value.Quantity;
                Description = value.Description;

                _selectedDgData = value;
                OnPropertyChanged();
            }
        }

        private string _CISCode;
        public string CISCode
        {
            get { return _CISCode; }
            set
            {
                _CISCode = value;
                OnPropertyChanged();
            }
        }

        private string _Type;
        public string Type
        {
            get { return _Type; }
            set
            {
                _Type = value;
                OnPropertyChanged();
            }
        }

        private string _InputSpec;
        public string InputSpec
        {
            get { return _InputSpec; }
            set
            {
                _InputSpec = value;
                OnPropertyChanged();
            }
        }

        private string _OutputSpec;
        public string OutputSpec
        {
            get { return _OutputSpec; }
            set
            {
                _OutputSpec = value;
                OnPropertyChanged();
            }
        }

        private string _Length;
        public string Length
        {
            get { return _Length; }
            set
            {
                _Length = value;
                OnPropertyChanged();
            }
        }

        private string _Quantity;
        public string Quantity
        {
            get { return _Quantity; }
            set
            {
                _Quantity = value;
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

        public class Fiber
        {
            public string CISCode { get; set; }
            public string Type { get; set; }
            public string InputSpec { get; set; }
            public string OutputSpec { get; set; }
            public string Length { get; set; }
            public string Quantity { get; set; }
            public string Description { get; set; }

            public Fiber(string CISCode, string Type, string InputSpec, string OutputSpec, string Length, string Quantity, string Description)
            {
                this.CISCode = CISCode != null ? CISCode : "";
                this.Type = Type != null ? Type : "";
                this.InputSpec = InputSpec != null ? InputSpec : "";
                this.OutputSpec = OutputSpec != null ? OutputSpec : "";
                this.Length = Length != null ? Length : "";
                this.Quantity = Quantity != null ? Quantity : "";
                this.Description = Description != null ? Description : "";
            }

            public Fiber() { }
        }

        public class SearchItems
        {
            public string All, CISCode;
        }

        ObservableCollection<Fiber> original;

        public ManageFiberViewModel()
        {
            init();

            BindingDataGrid();
        }

        private void BindingDataGrid()
        {
            dgData = new ObservableCollection<Fiber>();
            original = new ObservableCollection<Fiber>();

            var data = Log.ReadCsv(nameof(Fiber));
            if (data.Count > 0) data.RemoveAt(0);

            for (int i = 0; i < data.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(data[i])) continue;
                dgData.Add(DataHandler.ConvertStringToProduct<Fiber>(data[i]));
                original.Add(DataHandler.ConvertStringToProduct<Fiber>(data[i]));
            }
        }

        private void init()
        {
            dgData = new ObservableCollection<Fiber>();
            original = new ObservableCollection<Fiber>();

            MakeComboBox();
        }

        private void MakeComboBox()
        {
            searchItem = new ObservableCollection<string>(DataHandler.MakeComboBoxItemList<SearchItems>());
            selectedComboBox = "All";
        }

        private void Add()
        {
            if (string.IsNullOrWhiteSpace(CISCode))
            {
                MessageBox.Show($"CIS Code을 입력해주세요");
                return;
            }

            var product = new Fiber(CISCode, Type, InputSpec, OutputSpec, Length, Quantity, Description);

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
            Log.WriteLog(nameof(Fiber), original);

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
                selectedDgData = new Fiber();
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
                dgData = new ObservableCollection<Fiber>(original);
                return;
            }

            ObservableCollection<Fiber> ov = new ObservableCollection<Fiber>();

            for (int i = 0; i < original.Count; i++)
            {
                if (original[i].CISCode.ToUpper() == searchData.ToUpper()) ov.Add(original[i]);
            }

            dgData = new ObservableCollection<Fiber>(ov);
        }

        private void Refresh()
        {
            BindingDataGrid();
        }
    }
}
