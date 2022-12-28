using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snikeee
{
    public partial class Form1 : Form
    {
        private int _width = 520;
        private int _height = 420;
        Point _headLocation = new Point(120, 20);
        private int moveX, dirY;
        private int score = 0;
        private PictureBox[] snake = new PictureBox[400];
        private PictureBox food;
        private int _sizeOfSide = 20;
        private int r1, r2;

        public  Form1()
        {
            InitializeComponent();
            this.Width = _width;
            this.Height = _height;

            moveX = 1;
            dirY = 0;

            lbScore.Text = "Score: " + score;

            generateMap();
            Snakebody();

            food = new PictureBox();
            food.Size = new Size(_sizeOfSide, _sizeOfSide);

            updateFood();

            timer.Tick += new EventHandler(update);

            Speed();
            timer.Start();

            this.KeyDown += new KeyEventHandler(move);
        }

        private void Snakebody()
        {
            snake[0] = new PictureBox();
            snake[0].Location = _headLocation;
            snake[0].Size = new Size(_sizeOfSide, _sizeOfSide);
            snake[0].BackColor = Color.DarkGreen;
            this.Controls.Add(snake[0]);
        }

        private void updateFood()
        {
            Random r = new Random();
            Color[] colors = { Color.Red, Color.Yellow, Color.Orange, Color.Purple, Color.Blue };
            int indexOfColor = r.Next(colors.Length);
            food.BackColor = colors[indexOfColor];

            r1 = r.Next(120, _width - _sizeOfSide - 20);
            int tempI = r1 % _sizeOfSide;
            r1 -= tempI;
            r2 = r.Next(20, _height - _sizeOfSide - 20);
            int tempJ = r2 % _sizeOfSide;
            r2 -= tempJ;

            for (int i = 0; i < snake.Count(); i++)
            {
                if (snake[i] != null)
                {
                    if (snake[i].Location == new Point(r1, r2))
                    {
                        r1 = r.Next(120, _width - _sizeOfSide - 20);
                        tempI = r1 % _sizeOfSide;
                        r1 -= tempI;
                        r2 = r.Next(20, _height - _sizeOfSide - 20);
                        tempJ = r2 % _sizeOfSide;
                        r2 -= tempJ;
                        i = 0;
                    }
                }
            }
            
            food.Location = new Point(r1, r2);
            this.Controls.Add(food);
        }

        private void generateMap()
        {
            PictureBox picTop = new PictureBox();
            picTop.BackColor = Color.Black;
            picTop.Location = new Point(_width - 420, 0);
            picTop.Size = new Size(_width - 100, 20);
            this.Controls.Add(picTop);

            PictureBox picButtom = new PictureBox();
            picButtom.BackColor = Color.Black;
            picButtom.Location = new Point(_width - 420, _height - 20);
            picButtom.Size = new Size(_width - 100, 20);
            this.Controls.Add(picButtom);

            PictureBox picLeft = new PictureBox();
            picLeft.BackColor = Color.Black;
            picLeft.Location = new Point(_width - 420, 0);
            picLeft.Size = new Size(20, _height);
            this.Controls.Add(picLeft);

            PictureBox picRight = new PictureBox();
            picRight.BackColor = Color.Black;
            picRight.Location = new Point(_width - 20, 0);
            picRight.Size = new Size(20, _height);
            this.Controls.Add(picRight);
        }

        private void update(Object myObject, EventArgs eventsArgs)
        {
            _checkBorders();
            eatfood();
            moveSnake();

           
        }

        private void moveSnake()
        {
            for (int i = score; i >= 1; i--)
            {
                snake[i].Location = snake[i - 1].Location;
            }
            snake[0].Location = new Point
                (snake[0].Location.X + moveX * (_sizeOfSide),
                snake[0].Location.Y + dirY * (_sizeOfSide));
            SelfHarm();
        }
        private void SelfHarm()
        {
            for (int i1 = 1; i1 < score; i1++)
            {
                if (snake[0].Location == snake[i1].Location) //координаты тела и головы
                {
                    for (int j2 = i1; j2 <= score; j2++)
                        this.Controls.Remove(snake[j2]);

                    timer.Stop();
                    MessageBox.Show("Ты съел себя! Твои очки: " + score);
                    score = score - (score - i1 + 1);
                    MessageBox.Show("Вы можете продолжить со счетом: " + score + "\n Готовы?");
                    timer.Start();
                    Speed();
                    lbScore.Text = "Score: " + score;
                }
            }
        }

        private void eatfood()
        {
            if (snake[0].Location.X == r1 && snake[0].Location.Y == r2)
            {
                lbScore.Text = "Score: " + ++score;
                Speed();
                snake[score] = new PictureBox();
                snake[score].Location = new Point(snake[score - 1].Location.X + 20 * moveX, snake[score - 1].Location.Y - 20 * dirY);
                snake[score].Size = new Size(_sizeOfSide, _sizeOfSide);
                snake[score].BackColor = Color.DarkGreen;
                this.Controls.Add(snake[score]);

                updateFood();
            }
        }
        private void _checkBorders()
        {
            if (snake[0].Location.X < 110)
            {
                for (int i1 = 1; i1 <= score; i1++)
                {
                    this.Controls.Remove(snake[i1]);
                }
                timer.Stop();
                MessageBox.Show("Твои очки: " + score);
                timer.Start();
                score = 0;
                Speed();
                lbScore.Text = "Очки: " + score;
                moveX = 1;
            }

            if (snake[0].Location.X > _width - 39)
            {
                for (int i1 = 1; i1 <= score; i1++)
                {
                    this.Controls.Remove(snake[i1]);
                }
                timer.Stop();
                MessageBox.Show("Твои очки: " + score);
                timer.Start();
                score = 0;
                Speed();
                lbScore.Text = "Score: " + score;
                moveX = -1;
            }
            if (snake[0].Location.Y < 10)
            {
                for (int i1 = 1; i1 <= score; i1++)
                {
                    this.Controls.Remove(snake[i1]);
                }
                timer.Stop();
                MessageBox.Show("Твои очки: " + score);
                timer.Start();
                score = 0;
                Speed();
                lbScore.Text = "Очки: " + score;
                dirY = 1;
            }
            if (snake[0].Location.Y > _height - 30)
            {
                for (int i1 = 1; i1 <= score; i1++)
                {
                    this.Controls.Remove(snake[i1]);
                }
                timer.Stop();
                MessageBox.Show("Твои очки: " + score);
                timer.Start();
                score = 0;
                Speed();
                lbScore.Text = "Очки: " + score;
                dirY = -1;
            }
        }
        private void Speed()
        {
            int speed = 300;
            if ((0 <= score) && (score < 5))
            {
                speed = speed - 50;
            }
            else
            {
                if ((5 <= score) && (score < 10))
                    speed = speed - 100;
                else
                {
                    if ((10 <= score) && (score < 15))
                        speed = speed - 125;
                    else
                    {
                        if ((15 <= score) && (score < 20))
                            speed = speed - 150;
                        else speed = speed - 200;
                    }
                }
            }
            timer.Interval = speed;
        }
        private void move(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "Right":
                    moveX = 1;
                    dirY = 0;
                    break;
                case "Left":
                    moveX = -1;
                    dirY = 0;
                    break;
                case "Up":
                    dirY = -1;
                    moveX = 0;
                    break;
                case "Down":
                    dirY = 1;
                    moveX = 0;
                    break;
                default:
                    timer.Stop();
                    MessageBox.Show("Пауза! Ваш счет на данный момент: " + score + "\n Чтобы продолжить, нажмите Ok!");
                    timer.Start();
                    break;
            }
        }
    }
}