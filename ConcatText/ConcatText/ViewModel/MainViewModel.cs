using ConcatText.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace ConcatText.ViewModel
{
    class MainViewModel : ObservableObject
    {
        private ICommand openFolderCommand;
        public ICommand OpenFolderCommand
        {
            get { return (openFolderCommand) ?? (openFolderCommand = new RelayCommand(OpenFolder)); }
        }

        private string folderPath;
        public string FolderPath
        {
            get { return folderPath; }
            set
            {
                folderPath = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<string> fileList;
        public ObservableCollection<string> FileList
        {
            get { return fileList; }
            set
            {
                fileList = value;
                OnPropertyChanged();
            }
        }

        private string saveName;
        public string SaveName
        {
            get { return saveName; }
            set
            {
                saveName = value;
                OnPropertyChanged();
            }
        }

        private ICommand startCommand;
        public ICommand StartCommand
        {
            get { return (startCommand) ?? (startCommand = new RelayCommand(Start)); }
        }

        private string RootPath;
        public MainViewModel()
        {
            RootPath = @"C:\Users\rkdeo\Downloads\";
        }

        private void OpenFolder()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = RootPath;
                var result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    FolderPath = dialog.SelectedPath;
                    var fileList = Directory.GetFiles(FolderPath);
                    Array.Sort(fileList, new StringAsNumericComparer());
                    FileList = new ObservableCollection<string>(fileList);
                    RootPath = Path.GetDirectoryName(FileList[0]);
                }
            }
        }

        private void Start()
        {
            if (FileList == null || FileList.Count == 0) return;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            long totalSize = 0;
            var sb = new StringBuilder();
            foreach (var filePath in FileList)
            {
                var info = new FileInfo(filePath);

                totalSize += info.Length;

                using (var sr = new StreamReader(filePath, Encoding.UTF8))
                {
                    sb.AppendLine(sr.ReadToEnd());
                }
            }

            using (var sw = new StreamWriter($"{RootPath}{SaveName}.txt", false, Encoding.GetEncoding("ks_c_5601-1987")))
            {
                sw.Write(sb.ToString());
            }

            MessageBox.Show("completed!");
        }

        public class StringAsNumericComparer : IComparer<string>
        {
            public StringAsNumericComparer()
            { }

            public int Compare(object x, object y)
            {
                if ((x is string) && (y is string))
                {
                    return StringLogicalComparer.Compare((string)x, (string)y);
                }

                return -1;
            }

            public int Compare(string x, string y)
            {
                return StringLogicalComparer.Compare(x, y);
            }

            private class StringLogicalComparer
            {
                public static int Compare(string s1, string s2)
                {
                    //get rid of special cases
                    if ((s1 == null) && (s2 == null)) return 0;
                    else if (s1 == null) return -1;
                    else if (s2 == null) return 1;

                    if ((s1.Equals(string.Empty) && (s2.Equals(string.Empty)))) return 0;
                    else if (s1.Equals(string.Empty)) return -1;
                    else if (s2.Equals(string.Empty)) return -1;

                    //WE style, special case
                    bool sp1 = Char.IsLetterOrDigit(s1, 0);
                    bool sp2 = Char.IsLetterOrDigit(s2, 0);
                    if (sp1 && !sp2) return 1;
                    if (!sp1 && sp2) return -1;

                    int i1 = 0, i2 = 0; //current index
                    int r = 0; //temp result
                    while (true)
                    {
                        bool c1 = Char.IsDigit(s1, i1);
                        bool c2 = Char.IsDigit(s2, i2);
                        if (!c1 && !c2)
                        {
                            bool letter1 = Char.IsLetter(s1, i1);
                            bool letter2 = Char.IsLetter(s2, i2);
                            if ((letter1 && letter2) || (!letter1 && !letter2))
                            {
                                if (letter1 && letter2)
                                {
                                    r = Char.ToLower(s1[i1]).CompareTo(Char.ToLower(s2[i2]));
                                }
                                else
                                {
                                    r = s1[i1].CompareTo(s2[i2]);
                                }
                                if (r != 0) return r;
                            }
                            else if (!letter1 && letter2) return -1;
                            else if (letter1 && !letter2) return 1;
                        }
                        else if (c1 && c2)
                        {
                            r = CompareNum(s1, ref i1, s2, ref i2);
                            if (r != 0) return r;
                        }
                        else if (c1)
                        {
                            return -1;
                        }
                        else if (c2)
                        {
                            return 1;
                        }
                        i1++;
                        i2++;
                        if ((i1 >= s1.Length) && (i2 >= s2.Length))
                        {
                            return 0;
                        }
                        else if (i1 >= s1.Length)
                        {
                            return -1;
                        }
                        else if (i2 >= s2.Length)
                        {
                            return -1;
                        }
                    }
                }

                private static int CompareNum(string s1, ref int i1, string s2, ref int i2)
                {
                    int nzStart1 = i1, nzStart2 = i2; // nz = non zero
                    int end1 = i1, end2 = i2;

                    ScanNumEnd(s1, i1, ref end1, ref nzStart1);
                    ScanNumEnd(s2, i2, ref end2, ref nzStart2);
                    int start1 = i1; i1 = end1 - 1;
                    int start2 = i2; i2 = end2 - 1;

                    int nzLength1 = end1 - nzStart1;
                    int nzLength2 = end2 - nzStart2;

                    if (nzLength1 < nzLength2) return -1;
                    else if (nzLength1 > nzLength2) return 1;

                    for (int j1 = nzStart1, j2 = nzStart2; j1 <= i1; j1++, j2++)
                    {
                        int r = s1[j1].CompareTo(s2[j2]);
                        if (r != 0) return r;
                    }
                    // the nz parts are equal
                    int length1 = end1 - start1;
                    int length2 = end2 - start2;
                    if (length1 == length2) return 0;
                    if (length1 > length2) return -1;
                    return 1;
                }

                private static void ScanNumEnd(string s, int start, ref int end, ref int nzStart)
                {
                    nzStart = start;
                    end = start;
                    bool countZeros = true;
                    while (Char.IsDigit(s, end))
                    {
                        if (countZeros && s[end].Equals('0'))
                        {
                            nzStart++;
                        }
                        else countZeros = false;
                        end++;
                        if (end >= s.Length) break;
                    }
                }
            }
        }
    }
}
