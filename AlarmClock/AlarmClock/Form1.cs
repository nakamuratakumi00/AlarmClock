using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Drawing.Drawing2D;

namespace AlarmClock
{
    public partial class Form1 : Form
    {
        //アラームがセットされたか
        bool flag = false;

        //入力された日付が正確かどうかのチェック
        bool datecheck = false;

        //アラームがセットされた時に出すtextboxとlabel
        bool alarmClockCheck = false;

        //現在の時刻
        DateTime dNow = DateTime.Now;

        //入力された日付と時間をDateTime型に変換するための変数
        string time = "";
        string format = "MMddHHmm";
        DateTime dateTime;

        //サウンドを入れておく変数
        SoundPlayer sound = new SoundPlayer();
        String pass = @"D:\SIC\研修\AlarmClock\AlarmClock\AlarmClock\アラーム.wav";


        public Form1()
        {
            InitializeComponent();
            //backgroundWorker1.RunWorkerAsync();
            this.Load += Form1_Load;

            radius = (int)(Math.Sqrt(Width * Width + Height * Height) / 2);
            ox = Width / 2;
            oy = Height / 2;
            Region = new Region(new GraphicsPath());

        }

        private int radius { get; set; }
        private int ox { get; set; }
        private int oy { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;  // タイマの間隔を 1000[ms] に設定
            timer1.Start();          // タイマを起動
            label1.Hide();
            AlarmSet();

            Animator.Animate(300, (frame, resolution) =>
            {
                if (!Visible || IsDisposed) return false;
                var graphicsPath = new GraphicsPath();
                var r = radius * frame / resolution;
                graphicsPath.AddEllipse(new Rectangle(ox - r, oy - r, r * 2, r * 2));
                Region = new Region(graphicsPath);
                if (frame == resolution) Region = null;
                return true;
            });
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            dNow = DateTime.Now;
            label1.Text = dNow.ToString("MM/dd HH:mm:ss");  // 現在の時刻を表示
            if (flag)
            {
                if (dNow >= dateTime)
                {
                    sound.SoundLocation = pass;
                    sound.PlayLooping();
                    button4.Enabled = true;
                    flag = false;
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            time = textBox1.Text;
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DayCheck();

            if (datecheck)
            {
                dateTime = DateTime.ParseExact(time, format, null);

                if (MessageBox.Show("アラームを" + dateTime + "に設定しますか？", "目覚まし時計",
                               MessageBoxButtons.YesNo,  // ボタンの一覧は MessageBoxButtons 参照
                               MessageBoxIcon.Question) == DialogResult.Yes)   // アイコンの一覧は  MessageBoxIcon 参照)
                {
                    flag = true;
                    /* [はい] ボタンの処理 */
                    label3.Text = dateTime.ToString("MM/dd HH:mm:ss") + "にアラームをセットしました。";
                    textBox1.Text = "";
                    alarmClockCheck = false;
                    AlarmSet();
                    button3.Show();
                    button4.Show();
                    label3.Show();
                }
            }


        }
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Hide();
            alarmClockCheck = true;
            AlarmSet();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("設定したアラームを変更しますか？", "アラームの変更",
                       MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                AlarmSet();
                sound.Stop();
                alarmClockCheck = true;
                AlarmSet();
            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            sound.Stop();
            button4.Enabled = false;
            AlarmSet();
            alarmClockCheck = true;
            AlarmSet();

        }
        private void button5_Click(object sender, EventArgs e)
        {
            TimeHyouji();
        }
        void DayCheck()
        {
            DateTime dt;
            var ci = CultureInfo.CurrentCulture;
            var dts = DateTimeStyles.None;
            if (DateTime.TryParseExact(time, format, ci, dts, out dt))
            {
                if (dt < dNow)
                {
                    MessageBox.Show("その時刻にアラームはセット出来ません。", "エラーです。",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    datecheck = false;
                    textBox1.Text = "";
                    return;
                }
                datecheck = true;
            }
            else
            {
                MessageBox.Show("正しい数値を入力してください。", "エラーです。",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                datecheck = false;
                textBox1.Text = "";
                return;
            }
        }
        void AlarmSet()
        {
            if (alarmClockCheck)
            {
                textBox1.Show();
                button1.Show();
                label2.Show();
                label4.Show();
            }
            else
            {
                textBox1.Hide();
                button1.Hide();
                button2.Hide();
                button3.Hide();
                button4.Hide();
                button4.Enabled = false;
                label2.Hide();
                label3.Hide();
                label4.Hide();
            }
        }
        void TimeHyouji()
        {
            label1.Show();
            button2.Show();
            label6.Hide();
            button5.Hide();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!backgroundWorker1.CancellationPending)
            {
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.CancelAsync();
            Application.DoEvents();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics grfx = e.Graphics;
            int X_max = this.ClientSize.Width - 1;
            int Y_max = this.ClientSize.Height - 1;
            grfx.FillRectangle(new SolidBrush(Color.Black), 0, 0, X_max + 1, Y_max + 1);

            Random rand = new Random();  // 時間に応じて決まるシード値で初期化
            for (int N = 1; N <= 250; N++)
            {
                grfx.FillRectangle(
                    new SolidBrush(Color.FromArgb(175, 255, 255, 255)),
                    rand.Next(X_max),  // 0 以上で X_max より小さい乱数(整数)
                    rand.Next(Y_max),  // 0 以上で Y_max より小さい乱数(整数)
                    2, 2);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Interval = 2000;
 
            if (timer.Interval == 2000)
            {
                Invalidate();
               
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
