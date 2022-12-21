using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kurs
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                path_list.Items.Add(file);

                string f_list_name = file_name_for_list(file);
                list.Items.Add(f_list_name);

                string f_name = file_name_for_auto(file);

                Add_Auto(file, f_name);

                Txt_Save();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = list.SelectedIndex;
            if (id >= 0)
            {
                string file = path_list.Items[id].ToString();
                string f_name = file_name_for_auto(file);
                Del_Auto(f_name);

                path_list.Items.RemoveAt(id);
                list.Items.RemoveAt(id);

                Txt_Save();
            }
        }

        string file_name_for_auto(string file)
        {
            string f_name = "";

            for (int i = 0; i < file.Length; i++)
            {
                if (file[i] == '.')
                {
                    return f_name;
                }
                f_name += file[i];
                if (file[i] == '\\')
                {
                    f_name = "";
                }
            }

            return f_name;
        }
        string file_name_for_list(string file)
        {
            string f_name = "";

            for (int i = 0; i < file.Length; i++)
            {
                f_name += file[i];
                if (file[i] == '\\')
                {
                    f_name = "";
                }
            }

            return f_name;
        }

        void Add_Auto(string f_path, string f_name)
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

            key.SetValue(f_name, f_path);
            key.Close();
        }
        void Del_Auto(string f_name)
        {
            var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);

            key.DeleteValue(f_name, false);
            key.Close();
        }

        void Txt_Save()
        {
            using (StreamWriter writer = new StreamWriter("save_path.txt", false))
            {
                for (int i = 0; i < path_list.Items.Count; i++)
                {
                    writer.WriteLineAsync(path_list.Items[i].ToString());
                }
            }
            using (StreamWriter writer = new StreamWriter("save_name.txt", false))
            {
                for (int i = 0; i < list.Items.Count; i++)
                {
                    writer.WriteLineAsync(list.Items[i].ToString());
                }
            }
        }
        void Txt_Load()
        {
            if (File.Exists("save_path.txt"))
            {
                using (StreamReader reader = new StreamReader("save_path.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        path_list.Items.Add(line);
                    }
                }
                using (StreamReader reader = new StreamReader("save_name.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        list.Items.Add(line);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Txt_Load();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("!Рекомендуется выбирать файлы без точки в названии, не учит точку перед форматом файла");
        }
    }
}
