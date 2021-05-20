using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AlarmClock
{
    public partial class Form1 : Form
    {
        bool flag = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1000;  // タイマの間隔を 1000[ms] に設定
            timer1.Start();          // タイマを起動
        }

        DateTime dNow = DateTime.Now;

        [DllImport("winmm.dll")]
        extern static bool PlaySound(string s1, IntPtr i1, int i2);
        private void timer1_Tick(object sender, EventArgs e)
        {
            dNow = DateTime.Now;
            label1.Text = dNow.ToString("F");  // 現在の時刻を表示
            //label3.Text = dNow.ToShortTimeString();
            if (flag)
            {
                if (dNow >= dateTime)
                {
                    PlaySound(@"C:\Users\Nakamura-ta\OneDrive\ドキュメント\SIC\中村\windows formApp\AlarmClock\アラーム.wav", IntPtr.Zero, 0);
                    flag = false;
                }
            }
        }

        string time = "";
        string format = "MMddHHmm";
        DateTime dateTime;
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            time = textBox1.Text;
            label2.Text = time;
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            dateTime = DateTime.ParseExact(time, format, null);

            if (MessageBox.Show("アラームを" + time + "に設定しますか？", "目覚まし時計",
                           MessageBoxButtons.YesNo,  // ボタンの一覧は MessageBoxButtons 参照
                           MessageBoxIcon.Question) == DialogResult.Yes)   // アイコンの一覧は  MessageBoxIcon 参照)
            {
                flag = true;
                /* [はい] ボタンの処理 */
                label3.Text = dateTime.ToString();
            }
        }
    }
}
