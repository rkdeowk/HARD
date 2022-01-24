using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace InventoryManagement.Data
{
    public static class DataHandler
    {
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
    }
}