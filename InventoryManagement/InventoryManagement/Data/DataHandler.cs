using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;

namespace InventoryManagement.Data
{
    public static class Log
    {
        public static string GetLogPath(string item)
        {
            return $@"{Environment.CurrentDirectory}\log_{item}.csv";
        }
        public static string GetBackupLogPath(string item)
        {
            return $@"{Environment.CurrentDirectory}\backupLog\{DateTime.Now:yyyy-MM-dd}_log_{item}.csv";
        }

        public static void MakeBackupLogDir()
        {
            Directory.CreateDirectory($@"{Environment.CurrentDirectory}\backupLog");
        }

        public static List<string> ReadCsv(string item)
        {
            return DataHandler.ReadCsv(GetLogPath(item));
        }

        public static void WriteLog<T>(string item, IEnumerable<T> data)
        {
            DataHandler.WriteCsv(GetLogPath(item), data);
            DataHandler.WriteCsv(GetBackupLogPath(item), data);
        }
    }

    public static class DataHandler
    {
        #region [ Read and Write ]

        public static List<string> ReadCsv(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path);

                return new List<string>();
            }

            List<string> data = new List<string>();

            try
            {
                using (var sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        data.Add(sr.ReadLine());
                    }
                }
            }
            catch (Exception)
            {
                Thread.Sleep(timeout);

                using (var sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        data.Add(sr.ReadLine());
                    }
                }
            }

            return data;
        }

        public static string ToCsv<T>(IEnumerable<T> objectlist, string separator = ",")
        {
            Type t = typeof(T);
            PropertyInfo[] fields = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            string header = string.Join(separator, t.GetProperties(BindingFlags.Instance | BindingFlags.Public).Select(x => x.Name).ToArray());

            StringBuilder csvdata = new StringBuilder();
            csvdata.AppendLine(header);

            foreach (var o in objectlist)
                csvdata.AppendLine(ToCsvProperty(separator, fields, o));

            return csvdata.ToString();
        }

        public static string ToCsvProperty(string separator, PropertyInfo[] fields, object o)
        {
            StringBuilder linie = new StringBuilder();

            foreach (var f in fields)
            {
                if (linie.Length > 0)
                    linie.Append(separator);

                var x = f.GetValue(o);

                if (x != null)
                    linie.Append(x.ToString());
            }

            return linie.ToString();
        }

        static int timeout = 500;
        public static void WriteCsv(string path, string data)
        {
            try
            {
                using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.WriteLine(data);
                }
            }
            catch (Exception)
            {
                Thread.Sleep(timeout);

                using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.WriteLine(data);
                }
            }
        }

        public static void WriteCsv<T>(string path, IEnumerable<T> objectlist)
        {
            try
            {
                using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.WriteLine(ToCsv(objectlist));
                }
            }
            catch (Exception)
            {
                Thread.Sleep(timeout);

                using (var sw = new StreamWriter(path, false, Encoding.UTF8))
                {
                    sw.WriteLine(ToCsv(objectlist));
                }
            }
        }

        #endregion

        public static List<string> MakeComboBoxItemList<T>()
        {
            var comboBoxItem = new List<string>();

            var fieldInfos = typeof(T).GetFields();
            foreach (var field in fieldInfos)
            {
                comboBoxItem.Add(field.Name);
            }

            return comboBoxItem;
        }

        public static void ExportFile<T>(IEnumerable<T> original)
        {
            if (MessageBox.Show("저장해야 반영되니, 저장을 안하셨으면 저장 먼저 해주시기 바랍니다.\n 저장하셨습니까?", "정말요?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Csv file (*.csv)|*.csv";
                if (sfd.ShowDialog() == true)
                {
                    WriteCsv(sfd.FileName, original);
                }
            }
        }

        public static T ConvertStringToProduct<T>(string str)
        {
            T product = (T)Activator.CreateInstance(typeof(T));
            var s = str.Split(',');
            int idx = 0;

            var propertyInfos = typeof(T).GetProperties();
            foreach (var property in propertyInfos)
            {
                property.SetValue(product, s[idx++]);
            }

            return product;
        }

        public static bool CheckEqual<T>(T t1, T t2)
        {
            var propertyInfos = typeof(T).GetProperties();

            foreach (var property in propertyInfos)
            {
                var a = property.GetValue(t1, null) == null ? "" : property.GetValue(t1, null).ToString();
                var b = property.GetValue(t1, null) == null ? "" : property.GetValue(t2, null).ToString();
                if (a != b) return false;

            }

            return true;
        }

        public static bool CheckNull<T>(T product)
        {
            if (product == null) return true;

            var propertyInfos = typeof(T).GetProperties();

            foreach (var property in propertyInfos)
            {
                if (property.GetValue(product, null) != null) return false;
            }

            return true;
        }
    }
}