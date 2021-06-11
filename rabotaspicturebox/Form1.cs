using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Media;

namespace rabotaspicturebox
{
    public partial class Form1 : Form
    {
        Graphics myGr;
        Graphics myGr2;
        int X = 200; int Y = 200; int dX, dY;
        int x2 = 0; int y2 = 0;
        int schet = 0;//создание переменных

        private DateTime tl1;
        private DateTime tl2;//создание переменных времени

        private void timer1_Tick(object sender, EventArgs e)
        {
            Method(X, Y);
            X += dX;Y += dY;
            if (X >= pictureBox1.Width - 50) { dX *= -1; }
            if (X <= 0) { dX *= -1; }
            if (Y >= pictureBox1.Height - 50) { dY *= -1; }
            if (Y <= 0) { dY *= -1; }//действие при столкновении круга с полями формы
            Method2(x2, y2);
            Method3(X, Y, x2, y2);
        }

        public void Method(int x,int y)
        {
            myGr = pictureBox1.CreateGraphics();
            myGr.DrawEllipse(Pens.Blue, x, y, 50, 50); // рисуем круг
            Thread.Sleep(50);
            myGr.Clear(Color.White);
        }

        public void Method2(int x,int y)
        {
            myGr2 = pictureBox1.CreateGraphics();
            Rectangle rect = new Rectangle(x, y, 50, 50);
            myGr.DrawRectangle(new Pen(Color.Red), rect);
        }//отрисовка квадрата

        public Form1()
        {
            InitializeComponent();

            timer1.Interval = 90;
            timer1.Start();

            Random rand = new Random();
            dX = rand.Next(-8, 8);
            dY = rand.Next(-8, 8);

            tl1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            tl2 = tl1.AddMinutes((double)5);
            tl2 = tl2.AddSeconds((double)0);//обЪявление переменных
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show(
        "Для начала игры соприкоснитесь квадратом с кругом",
        "Сообщение",
        MessageBoxButtons.OK,
        MessageBoxIcon.Information);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                x2 -= 10;
            }
            if (e.KeyCode == Keys.Right)
            {
                x2 += 10;
            }
        }//передвижение квадрата влево и вправо

        public void Method3(int X, int Y, int x2, int y2)
        {
            if (((x2 >= (X - 50)) & (x2 <= (X + 50))) & (((y2 >= (Y - 50) & (y2 <= (Y + 50))))))
            {
                label1.Text = ("Счёт:"+schet++);
                dX *= -1;
                dY *= -1;
            }
        }//повышение счёта при столкновении круга и квадрата

        private void timer2_Tick(object sender, EventArgs e)
        {
            EndGame eg = new EndGame();

            tl2 = tl2.AddSeconds(-1);
            if (tl2.Minute < 9)
                label2.Text = "0" + tl2.Minute.ToString() + ":";
            else
                label2.Text = tl2.Minute.ToString() + ":";

            if (tl2.Second < 9)
                label2.Text += "0" + tl2.Second.ToString();
            else
                label2.Text += tl2.Second.ToString();

            if (Equals(tl1, tl2))
            {
                timer2.Enabled = false;
                timer1.Enabled = false;
                if (MessageBox.Show("Время истекло", "Таймер", MessageBoxButtons.OK) == DialogResult.OK)
                {
                    eg.Score = schet;//передача счёта в форму с базой данных
                    eg.ShowDialog();

                    this.Hide();
                }
            }
        }//таймер для отсчёта игрового времени

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
    }
}
