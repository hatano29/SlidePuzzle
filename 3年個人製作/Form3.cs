using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Formats.Asn1.AsnWriter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Xml.Linq;

namespace _3年個人製作
{
    public partial class Form3 : Form
    {
        public static string FilePath = "./ranking.csv";
        List<List<string>> list = new List<List<string>>();
        public Form3(int time, int count, string name)
        {
            InitializeComponent();

            this.Size = new Size(822, 506);
            label1.Location = new Point(481, 31);
            comboBox1.Location = new Point(578, 28);
            comboBox1.Size = new Size(182, 33);
            dataGridView1.Location = new Point(130, 98);
            dataGridView1.Size = new Size(528, 327);

            if (!File.Exists(FilePath))     //ファイルが存在しない場合作成
            {
                using (FileStream fs = File.Create(FilePath)) ;
            }

            if (name != "" && count >= 0 && time >= 0)
            {
                using (StreamWriter sw = new StreamWriter(FilePath, true, Encoding.UTF8))
                {
                    sw.WriteLine(name + "," + time.ToString() + "," + count.ToString());      //ファイル書き込み
                }
            }

            using (StreamReader data = new StreamReader(File.OpenRead(FilePath)))
            {
                while (!data.EndOfStream)
                {
                    var line = data.ReadLine();
                    var values = line.Split(",");   // カンマ区切り

                    // 1行ずつ格納
                    List<string> col = new List<string>();
                    foreach (var val in values)
                    {
                        col.Add(val);
                    }
                    list.Add(col);
                }
            }
            var sortlist = list.OrderBy(e => int.Parse(e[1]));
            int rank = 1;
            foreach (var row in sortlist)
            {
                dataGridView1.Rows.Add(rank, row[0], row[1], row[2]);
                rank++;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index;
            index = comboBox1.SelectedIndex;

            dataGridView1.Rows.Clear();

            if (index == 0)
            {
                var sortlist = list.OrderBy(e => int.Parse(e[1]));
                int rank = 1;
                foreach (var row in sortlist)
                {
                    dataGridView1.Rows.Add(rank, row[0], row[1], row[2]);
                    rank++;
                }
            }
            else if (index == 1)
            {
                var sortlist = list.OrderBy(e => int.Parse(e[2]));
                int rank = 1;
                foreach (var row in sortlist)
                {
                    dataGridView1.Rows.Add(rank, row[0], row[1], row[2]);
                    rank++;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}
