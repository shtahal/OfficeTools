using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OfficeTools
{
    public partial class Form1 : Form
    {
        private bool tomorrowFlag = false;
        public Form1()
        {
            InitializeComponent();
            this.selectHour.Value = DateTime.Now.Hour;
            this.selectMinu.Value = DateTime.Now.Minute;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String hour = this.selectHour.Value.ToString(), minute = this.selectMinu.Value.ToString();

            if (DateTime.Now.Hour < Convert.ToInt32(hour))
            {
                this.confirmOk();
                this.timer1.Start();
            }
            else if (DateTime.Now.Hour == Convert.ToInt32(hour))
            {
                if (DateTime.Now.Minute < Convert.ToInt32(minute))
                {
                    this.confirmOk();
                    this.timer1.Start();
                }
                else
                {
                    MessageBox.Show("下班时间需比当前时间晚！", "设置提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.selectHour.Value = DateTime.Now.Hour;
                    this.selectMinu.Value = DateTime.Now.Minute;
                }
            }
            else // 第二天
            {
                tomorrowFlag = true;
                //this.numericUpDown1.Value = DateTime.Now.Hour;
                //this.numericUpDown2.Value = DateTime.Now.Minute;
                this.confirmOk();
                this.timer1.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("重新设置下班时间！", "设置提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            this.timer1.Stop();
            tomorrowFlag = false;

            this.selectHour.Value = DateTime.Now.Hour;
            this.selectMinu.Value = DateTime.Now.Minute;
            this.selectHour.Enabled = true;
            this.selectMinu.Enabled = true;

            this.labelHour.Text = "0";
            this.labelMinu.Text = "0";
            this.labelHour2.Text = "0";
            this.labelMinu2.Text = "0";
            this.labelSec.Text = "0";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            String hour = this.selectHour.Value.ToString(), minute = this.selectMinu.Value.ToString();

            String year = DateTime.Now.Year.ToString(), month = DateTime.Now.Month.ToString(), day = DateTime.Now.Day.ToString(); ;
            if (tomorrowFlag)
            {
                day = DateTime.Now.AddDays(1).Day.ToString();
            }

            String planTimeStr1 = year + "/" + month + "/" + day;
            String planTimeStr2 = hour + ":" + minute + ":00";
            DateTime planTime = Convert.ToDateTime(planTimeStr1 + " " + planTimeStr2);

            DateTime curTime = DateTime.Now;
            TimeSpan ts = planTime - curTime;

            if (ts.TotalMilliseconds > 0)
            {
                this.labelHour2.Text = ts.Hours.ToString();
                this.labelMinu2.Text = ts.Minutes.ToString();
                this.labelSec.Text = ts.Seconds.ToString();
            }
            else
            {
                this.timer1.Stop();
                tomorrowFlag = false;
                this.selectHour.Enabled = true;
                this.selectMinu.Enabled = true;

                //MessageBox.Show("下班了！下班了！下班了！", "设置提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                using (var soundPlayer = new SoundPlayer(Environment.CurrentDirectory + "\\zhangyuge.wav"))
                {
                    soundPlayer.Play();
                }

                this.Show();
                this.TopMost = true;
                WindowState = FormWindowState.Normal;
                this.Focus();

                Form2 form2 = new Form2();
                form2.Activate();
                form2.Visible = true;
                form2.TopMost = true;
            }
        }

        private void confirmOk()
        {
            this.labelHour.Text = this.selectHour.Value.ToString();
            this.labelMinu.Text = this.selectMinu.Value.ToString();
            this.selectHour.Enabled = false;
            this.selectMinu.Enabled = false;
            MessageBox.Show("下班时间已设置！", "设置提醒", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            this.notifyIcon.Visible = false;
            this.Show();
            this.TopMost = true;
            WindowState = FormWindowState.Normal;
            this.Focus();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //this.ShowInTaskbar = false;
                this.Visible = false;
                this.notifyIcon.Visible = true;
                this.notifyIcon.ShowBalloonTip(2, "提示", "点击图标恢复", ToolTipIcon.Info);
            }
            
        }

        private void Click_Update(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("");
        }
    }
}
