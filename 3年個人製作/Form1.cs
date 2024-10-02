using System.Security.Cryptography.X509Certificates;

namespace _3年個人製作 //×製作、〇制作
{
    /*15ピースのスライドパズル*/
    public partial class Form1 : Form
    {
        public static int timer = 0;  //ゲーム開始からの経過時間
        public static int count = 0; //パネルの移動回数

        const int panel_kazu = 16;    //総パネル数
        const int panel_ue = 4;  //外側パネルとフォーム上部のスキマ
        const int panel_yoko = 4;//外側パネルとフォーム側部のスキマ
        public const int panel_sukima = 8; //パネル間
        public const int panel_size = 200; //パネルサイズ
        Font panel_number = new Font("Yu Gothic UI", 20f);//パネルに表示されるフォント
        int row = 0;    //パネルの行
        int col = 0;    //パネルの列
        public static bool sflg = false; //ゲームが開始しているか
        public static bool fflg = false;  //ゲーム終了しているかどうか
        public static bool now_shuffle = false; //シャッフル中かどうか

        public static panel[] panels = new panel[panel_kazu]; //Buttonを継承したフィールド
        public static Point[] panelPoint = new Point[panel_kazu];   //パネルの初期位置を保存,完成したかチェック
        panel_shuffle shuffle = new panel_shuffle(panel_kazu, panels);  //ゲーム開始時パネルを混ぜるためのインスタンス
        puzuule_image pimg = new puzuule_image("パズル画像.jpg",panel_size); //背景画像の設定に用いる

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(854, 1000);
            Start.Location = new Point(654, 850);
            Start.Size = new Size(150, 75);
            label1.Location = new Point(43, 875);
            label2.Location = new Point(300, 875);
            for (int i = 0; i < panel_kazu; i++)   //パズルのパネル生成
            {
                panels[i] = new panel();
                panels[i].Name = "p" + i.ToString();
                panels[i].Text = (i + 1).ToString();
                panels[i].Size = new Size(panel_size, panel_size);
                panels[i].Font = panel_number;
                panels[i].ForeColor = Color.White;
                if (i != 0 && i % 4 == 0) //今何行目か
                {
                    row++;
                    col = 0;
                }
                if (row == 0) //一番上の行
                {
                    if (col == 0)
                    {
                        panels[i].Location = new Point(panel_yoko, panel_ue);
                    }
                    else
                    {
                        panels[i].Location = new Point(panels[i - 1].Left + panel_size + panel_sukima, panel_ue);
                    }
                }
                else //2段目以降
                {
                    if (col == 0)
                    {
                        panels[i].Location = new Point(panel_yoko, panels[i - 4].Top + panel_size + panel_sukima);
                    }
                    else
                    {
                        panels[i].Location = new Point(panels[i - 1].Left + panel_size + panel_sukima, panels[i - 4].Top + panel_size + panel_sukima);
                    }
                }

                panelPoint[i] = new Point();
                panelPoint[i] = panels[i].Location; //初期位置記録、完成時参照

                col++;
                this.Controls.Add(panels[i]);
                panels[i].eventMaking();    //クリック時のイベント作成
                if (i == panel_kazu - 1) //16枚目のパネルは透明。
                {
                    panels[i].Visible = false;
                }
            }
            pimg.imgset(panels);
        }

        private void button1_Click(object sender, EventArgs e)  //スタートボタン、パネルシャッフル
        {
            now_shuffle = true;
            if (sflg == false)
            {
                Start.Enabled = false;
                Start.Text = "リセット";
                sflg = true;
                shuffle.shuffle();    //パネルを混ぜる
                Start.Enabled = true;
                timer1.Start();
            }
            else
            {
                timer1.Stop();
                for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].Location = panelPoint[i];
                }
                sflg = false;
                timer = 0;
                count = 0;
                label1.Text = "時間 : ";
                label2.Text = "移動回数 : ";
                Start.Text = "スタート";
            }
            now_shuffle = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(sflg == true) {
                timer++;
                label1.Text = "時間 : " + (timer / 10).ToString();
                label2.Text = "移動回数 : " + count.ToString();
            }
            else
            {
                timer1.Stop();
                sflg = true;
            }     
        }
    }

    public class panel : Button
    {
        public Boolean slide = false;//スライドできたかどうか、シャッフルの際使用
        public void eventMaking()           //ボタンにイベント作成
        {
            this.Click += new EventHandler(SlideCheck);
        }

        public void SlideCheck(object? sender, EventArgs e) //空いてる場所が近くにあるかを確認
        {
            slide = false;
            if(Form1.sflg == true)
            {
                int x, y;
                if (this.Left == Form1.panels[15].Left)   //上→下or下→上
                {
                    if (this.Top + Form1.panel_size + Form1.panel_sukima == Form1.panels[15].Top
                            || this.Top - Form1.panel_size - Form1.panel_sukima == Form1.panels[15].Top)
                    {
                        Slide();
                    }
                }
                else if (this.Top == Form1.panels[15].Top) //左→右or右or左
                {
                    if (this.Left + Form1.panel_size + Form1.panel_sukima == Form1.panels[15].Left
                           || this.Left - Form1.panel_size - Form1.panel_sukima == Form1.panels[15].Left)
                    {
                        Slide();
                    }
                }
            }         
        }
        private void Slide()　//パネルを空いている場所へスライド
        {
            int x, y;
            y = this.Top;   //移動前の位置記録
            x = this.Left;

            this.Top = Form1.panels[15].Top;
            this.Left = Form1.panels[15].Left;

            Form1.panels[15].Top = y;
            Form1.panels[15].Left = x;

            slide = true;

            if (Form1.now_shuffle == false) //シャッフル中に完成扱いにならないように
            {
                Form1.count++;
                p_complete();
            }
            
            /*移動時のアニメーション
             * while (true)
            {
                if(this.Top < Form1.panels[15].Top)
                {
                    this.Top += 4;
                }else if(this.Top > Form1.panels[15].Top)
                {
                    this.Top -= 4;
                }else if(this.Left < Form1.panels[15].Left)
                {
                    this.Left += 4;
                }else if(this.Left > Form1.panels[15].Left)
                {
                    this.Left -= 4;
                }
                Thread.Sleep(1);
                if (this.Top == Form1.panels[15].Top && this.Left == Form1.panels[15].Left)
                {
                    Form1.panels[15].Left = x;
                    Form1.panels[15].Top = y;
                    break;
                }
               
            }*/
        }
        public Boolean counter()
        {
            return slide;
        }
        private void p_complete()   //すべてのパネルが正しい位置にあるかチェック
        {
            bool complete = true;
            for(int i = 0; i < Form1.panels.Length; i++)
            {
                if (Form1.panels[i].Location != Form1.panelPoint[i])
                {
                    complete = false; break;
                }
            }
            if(complete == true)
            {
                Form1.sflg = false;

                MessageBox.Show("完成");
                Form2 nyuryoku = new Form2(Form1.timer / 10,Form1.count);
                nyuryoku.ShowDialog();                
            }
        }
    }

    public class panel_shuffle  //ゲーム開始時のパネルの並びを作る。
    {
        int shuffle_MAX = 100; //100回シャッフルする
        int shuffle_kaisu = 0;  //現在何回混ぜたか
        int panel_kazu;
        Random r = new Random();
        int random;
        public Boolean nowshuffle = false;
        panel[] p;
        public panel_shuffle(int panel_kazu, panel[] p)
        {
            this.panel_kazu = panel_kazu;
            this.p = p;
        }

        public void shuffle()   //ランダムで100回移動させる
        {
            nowshuffle = true;
            shuffle_kaisu = 0;
            while (true)
            {
                random= r.Next(panel_kazu); 
                p[random].PerformClick();   //ランダムにパネルクリック
                if (p[random].slide == true)    //クリック時パネルが動いた場合
                {
                    shuffle_kaisu++;
                    if(shuffle_kaisu >= shuffle_MAX)
                    {
                        break;
                    }
                }
            }
            nowshuffle = false;
        }
    }

    public class puzuule_image
    {
        String file_name;
        int panel_size;
        int x = 0, y = 0;  //画像を切り抜く位置
        int col = 0;
        public puzuule_image(String filename,int panel_size) {
            this.file_name = filename;
            this.panel_size = panel_size;
        }

        public void imgset(panel[] p)   //画像を切りぬきパネルに順次セット
        {
            Bitmap img = new Bitmap(file_name);

            for(int i = 0; i < p.Length - 1; i++)
            {
                Rectangle rect = new Rectangle(x, y, panel_size, panel_size);
                p[i].BackgroundImage = img.Clone(rect, img.PixelFormat);
                col++;
                x += panel_size;
                if(col > 3)
                {
                    col = 0;
                    x = 0;
                    y += panel_size;
                }
            }
            img.Dispose();
        }
    }
}
