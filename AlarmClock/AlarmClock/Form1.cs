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

namespace AlarmClock
{
    public partial class Form1 : Form
    {
        bool flag = false;
        bool datecheck = false;
        bool alarmClockCheck = false;

        DateTime dNow = DateTime.Now;

        string time = "";
        string format = "MMddHHmm";
        DateTime dateTime;

        SoundPlayer sound = new SoundPlayer();

        // [DllImport("winmm.dll")]
        // extern static bool PlaySound(string s1, IntPtr i1, int i2);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;  // タイマの間隔を 1000[ms] に設定
            timer1.Start();          // タイマを起動
            AlarmSet();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            dNow = DateTime.Now;
            label1.Text = dNow.ToString("MM/dd HH:mm:ss");  // 現在の時刻を表示
            if (flag)
            {
                if (dNow >= dateTime)
                {
                    sound.SoundLocation = @"C:\Users\nakamura-ta\ソース\AlarmClock\AlarmClock\AlarmClock\アラーム.wav";
                    sound.PlayLooping();
                    button4.Enabled = true;
                    flag = false;
                }
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            time = textBox1.Text;
            //label2.Text = time;
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
                button3.Hide();
                button4.Hide();
                button4.Enabled = false;
                label2.Hide();
                label3.Hide();
                label4.Hide();
            }
        }
    }
}
