using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Bookmarker.ViewModel
{
    public class MainBookmarkerViewModel : ObservableObject
    {
        #region [ Command ]

        private ICommand _openCommand;
        public ICommand OpenCommand
        {
            get { return (_openCommand) ?? (_openCommand = new RelayCommand(Open)); }
        }

        private ICommand _addCommand;
        public ICommand AddCommand
        {
            get { return (_addCommand) ?? (_addCommand = new RelayCommand(Add)); }
        }

        private ICommand _removeCommand;
        public ICommand RemoveCommand
        {
            get { return (_removeCommand) ?? (_removeCommand = new RelayCommand(Remove)); }
        }

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get { return (_saveCommand) ?? (_saveCommand = new RelayCommand(Save)); }
        }

        private ICommand _loadCommand;
        public ICommand LoadCommand
        {
            get { return (_loadCommand) ?? (_loadCommand = new RelayCommand(Load)); }
        }

        #endregion

        #region [ Binding ]

        private ObservableCollection<ConfigStruct> _dgData;
        public ObservableCollection<ConfigStruct> dgData
        {
            get { return _dgData; }
            set
            {
                _dgData = value;
                OnPropertyChanged();
            }
        }

        private ConfigStruct _selectedDgData;
        public ConfigStruct selectedDgData
        {
            get { return _selectedDgData; }
            set
            {
                _selectedDgData = value;
                OnPropertyChanged();
            }
        }

        private bool _inputIsChrome;
        public bool inputIsChrome
        {
            get { return _inputIsChrome; }
            set
            {
                _inputIsChrome = value;
                OnPropertyChanged();
            }
        }

        private string _inputNickname;
        public string inputNickname
        {
            get { return _inputNickname; }
            set
            {
                _inputNickname = value;
                OnPropertyChanged();
            }
        }

        private string _inputPath;
        public string inputPath
        {
            get { return _inputPath; }
            set
            {
                _inputPath = value;
                OnPropertyChanged();
            }
        }

        private bool _dgRowIsEnable;
        public bool dgRowIsEnable
        {
            get { return _dgRowIsEnable; }
            set
            {
                _dgRowIsEnable = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public MainBookmarkerViewModel()
        {
            DataGridBinding();
        }

        private void Reset()
        {
            dgData = new ObservableCollection<ConfigStruct>();

            inputIsChrome = false;
            inputNickname = string.Empty;
            inputPath = string.Empty;
        }

        private void Open()
        {
            var folderDialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true
            };

            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                inputPath = folderDialog.FileName;
            }
        }

        private void Add()
        {
            if (string.IsNullOrWhiteSpace(inputNickname) || string.IsNullOrWhiteSpace(inputPath)) return;

            var input = new ConfigStruct(inputIsChrome, inputNickname, inputPath);

            Config.Instance.ConfigData.Add(input);

            dgData.Add(input);
        }

        private void Remove()
        {
            if (selectedDgData == null) return;

            Config.Instance.ConfigData.Remove(selectedDgData);

            dgData.Remove(selectedDgData);
        }

        private void Save()
        {
            Config.Instance.Save();
        }

        private void Load()
        {
            Config.Instance.Load();

            DataGridBinding();
        }

        private void DataGridBinding()
        {
            dgData = new ObservableCollection<ConfigStruct>(Config.Instance.ConfigData);
        }

        enum WebBrowser
        {
            chrome,
            iexplore
        }

        public void Operation(ConfigStruct data)
        {
            try
            {
                if (data == null) return;

                var process = new Process();

                if (data.path.ToUpper().Contains("HTTP"))
                {
                    var webBrowser = data.isChrome ? nameof(WebBrowser.chrome) : nameof(WebBrowser.iexplore);

                    process.StartInfo.FileName = webBrowser;
                    process.StartInfo.Arguments = $@"{data.path}";
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
                else
                {
                    process.StartInfo.FileName = $@"{data.path}";
                    process.StartInfo.UseShellExecute = true;
                    process.Start();
                }
            }
            catch
            {
                MessageBox.Show($"경로를 다시 한 번 확인해주세요.\n  - Nickname : {data.nickname}\n  - 경로 : {data.path}");
            }
        }
    }
}
