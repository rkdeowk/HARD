using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MoveFiles
{
    public partial class Form1 : Form
    {
        public string src_path = "E:\\";

        public Form1()
        {
            InitializeComponent();

            double file_size = 0.15;

            var src_files = dir_search(src_path).Select(x => x.FullName).ToArray();
            src_files = src_files.Where(x => !x.Contains("private")).ToArray();

            var target_list = new List<string>();
            var remove_list = new List<string>();
            foreach (var src in src_files)
            {
                if (src.Count(x => x == '\\') >= 2)
                {
                    target_list.Add(src);
                }
            }

            src_files = target_list.ToArray();

            var fail_list = new List<string>();
            foreach (var src in src_files)
            {
                if (file_size <= new FileInfo(src).Length / 1024.0 / 1024 / 1024)
                {
                    try
                    {
                        string des_path = $"{src_path}{Path.GetFileName(src)}";

                        if (!File.Exists(des_path))
                        {
                            File.Move(src, des_path);
                        }
                        remove_list.Add(Path.GetDirectoryName(src));
                    }
                    catch (Exception e)
                    {
                        fail_list.Add(src);
                    }
                }
            }

            remove_list = remove_list.Distinct().ToList();

            foreach (var rm in remove_list)
            {
                try
                {
                    Directory.Delete(rm, true);
                }
                catch (Exception)
                {
                    MessageBox.Show($"don't remove {rm}");
                }
            }

            Close();
        }

        private List<FileInfo> dir_search(string dir, List<FileInfo> temp = null)
        {
            if (temp == null) temp = new List<FileInfo>();
            DirectoryInfo di = new DirectoryInfo(dir);

            foreach (var d in Directory.GetDirectories(dir))
            {
                if (d.Contains("RECYCLE.BIN")) continue;
                if (d.Contains("System Volume Information")) continue;

                dir_search(d, temp);
            }
            foreach (var item in di.GetFiles())
            {
                temp.Add(item);
            }

            return temp;
        }
    }
}