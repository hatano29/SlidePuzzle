using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3年個人製作
{
    public partial class Form2 : Form
    {
        int time, count;
        public string FilePath = "./ranking.csv";
        public Form2(int time, int count)
        {
            InitializeComponent();

            this.Size = new Size(825, 422);
            label2.Location = new Point(27, 20);
            label3.Location = new Point(107, 99);
            label4.Location = new Point(129, 168);
            label1.Location = new Point(185, 235);
            label5.Location = new Point(396, 91);
            label6.Location = new Point(396, 160);
            textBox1.Location = new Point(301, 235);
            textBox1.Size = new Size(270, 45);
            button1.Location = new Point(605, 286);
            button1.Size = new Size(162, 57);

            this.time = time;
            this.count = count;

            if (!File.Exists(FilePath))     //ランキングファイルが存在しなかった場合作成
            {
                using (FileStream fs = File.Create(FilePath)) ;
            }

            label5.Text = time.ToString();
            label6.Text = count.ToString();

            label5.Visible = true;
            label6.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                this.Close();
                Form3 form3 = new Form3(time, count, textBox1.Text);
                form3.ShowDialog();
            }
            else
            {
                MessageBox.Show("名前を入力してください");
            }
            
        }
    }
}
