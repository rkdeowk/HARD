using HARD.Config;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace HARD.ViewModel.Linker
{
    public class Linker_ViewModel : INotifyPropertyChanged
    {
        #region [ Binding ]

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #region [ Control ]

        private string textBoxNickName;
        public string TextBoxNickName
        {
            get { return textBoxNickName; }
            set
            {
                textBoxNickName = value;
                OnPropertyChanged(nameof(TextBoxNickName));
            }
        }

        private string textBoxDirPath;
        public string TextBoxDirPath
        {
            get { return textBoxDirPath; }
            set
            {
                textBoxDirPath = value;
                OnPropertyChanged(nameof(TextBoxDirPath));
            }
        }

        private Linker_Config.DirInfo selectedDirInfo;
        public Linker_Config.DirInfo SelectedDirInfo
        {
            get { return selectedDirInfo; }
            set
            {
                selectedDirInfo = value;
                OnPropertyChanged(nameof(SelectedDirInfo));
            }
        }

        private ObservableCollection<Linker_Config.DirInfo> directoryInfo;
        public ObservableCollection<Linker_Config.DirInfo> DirectoryInfo
        {
            get { return directoryInfo; }
            set
            {
                directoryInfo = value;
                OnPropertyChanged(nameof(DirectoryInfo));
            }
        }

        #endregion

        #region [ Command ]

        private ICommand openCommand;
        public ICommand OpenCommand
        {
            get
            {
                return (openCommand) ?? (openCommand = new DelegateComand(Open));
            }
        }

        private ICommand addCommand;
        public ICommand AddCommand
        {
            get
            {
                return (addCommand) ?? (addCommand = new DelegateComand(Add));
            }
        }

        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                return (saveCommand) ?? (saveCommand = new DelegateComand(Save));
            }
        }

        private ICommand openDirectoryInfoCommand;
        public ICommand OpenDirectoryInfoCommand
        {
            get
            {
                return (openDirectoryInfoCommand) ?? (openDirectoryInfoCommand = new DelegateComand(OpenDirectoryInfo));
            }
        }

        private ICommand deleteDirectoryInfoCommand;
        public ICommand DeleteDirectoryInfoCommand
        {
            get
            {
                return (deleteDirectoryInfoCommand) ?? (deleteDirectoryInfoCommand = new DelegateComand(DeleteDirectoryInfo));
            }
        }

        #endregion

        #endregion

        public Linker_ViewModel()
        {
            var k = Linker_Config.Instance.ConfigData;
            DirectoryInfo = new ObservableCollection<Linker_Config.DirInfo>(k);
        }

        private void Open()
        {
            CommonOpenFileDialog folderDialog = new CommonOpenFileDialog()
            {
                InitialDirectory = "",
                IsFolderPicker = true
            };

            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                TextBoxDirPath = folderDialog.FileName;
            }
        }

        private void Add()
        {
            if (string.IsNullOrWhiteSpace(textBoxDirPath))
            {
                MessageBox.Show("폴더 경로를 입력해주세요");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBoxNickName))
            {
                MessageBox.Show("별칭을 입력해주세요");
                return;
            }

            Linker_Config.Instance.ConfigData.Add(new Linker_Config.DirInfo(TextBoxNickName, TextBoxDirPath));

            DirectoryInfo = new ObservableCollection<Linker_Config.DirInfo>(Linker_Config.Instance.ConfigData);
            TextBoxNickName = "";
        }

        private void Save()
        {
            Linker_Config.Instance.Save();

            MessageBox.Show("저장 완료");
        }

        private void OpenDirectoryInfo()
        {
            if (!Directory.Exists(SelectedDirInfo.DirPath))
            {
                MessageBox.Show($"{SelectedDirInfo.DirPath} 폴더가 존재하지 않습니다");
                return;
            }

            Process.Start(SelectedDirInfo.DirPath);
        }

        private void DeleteDirectoryInfo()
        {
            Linker_Config.Instance.ConfigData.Remove(SelectedDirInfo);

            DirectoryInfo = new ObservableCollection<Linker_Config.DirInfo>(Linker_Config.Instance.ConfigData);
        }
    }
}
