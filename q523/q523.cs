/************************************************
** q523.cs
** Party Quest 523: Distract Forgemaster Garfrost
 * Go Faster!
** Patrick Xie, 5/23/2011
************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WMPLib;

namespace q523
{
    public partial class q523 : Form
    {
        /*Global Variables*/
        private Bitmap myCanvas;
        int direction;
        int previous;
        List<Snake> wholeSnake = new List<Snake>();
        List<Food> allFoods = new List<Food>();
        int x = 245;
        int y = 245;
        int xNew;
        int yNew;
        int score;//overall game score
        bool ate;
        Random rand = new Random();
        double time = 0;//for timer
        bool started;//if the game started or not; for NewGame
        bool keepPlaying;
        WMPLib.WindowsMediaPlayer player = new WindowsMediaPlayer();

        public q523()
        {
            InitializeComponent();
        }


        private void q523_Load(object sender, EventArgs e)
        {
            myCanvas = new Bitmap(500, 500,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(myCanvas);
            g.Clear(Color.Black);
            wholeSnake.Add(new Snake());
            wholeSnake[0].add(g,x,y);
            time = 60.00;//Sets timer to 60 seconds
            started = false;

            keepPlaying = true;
            player.URL = @"M2-1.WAV";
            player.controls.play();

            score = 0;//Initial score
            for (int i = 0; i < 5; i++)//Adds the food to game
            {
                xNew = rand.Next(0, 49);
                yNew = rand.Next(0, 49);
                allFoods.Add(new Food());
                allFoods[i].add(g, xNew, yNew);
            }
            ate = false;
        }

        private void q523_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.DrawImage(myCanvas, 0, 24, myCanvas.Width, myCanvas.Height);
        }

        private void aniTimer_Tick(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(myCanvas);
            SolidBrush ateBrush = new SolidBrush(Color.Black);

            /*detection hit*/
            for (int j = 0; j < allFoods.Count; j++)
            {
                if (!allFoods[j].getDead())
                {
                    if (wholeSnake[0].IsPointInside(new Point(allFoods[j].getfX(), allFoods[j].getfY())))
                    {
                        allFoods[j].remove(g);
                        ate = true;
                        score+=10;
                        if (score % 100 == 0 && score != 0)
                        {
                            time += 3.00;
                        }
                    }
                    
                }
            }
            /*Moves the head in each tick*/
            if (direction == 1)
            {
                wholeSnake[0].move(g, wholeSnake[0].getSX(), wholeSnake[0].getSY() - 10, 1);
            }
            if (direction == 2)
            {
                wholeSnake[0].move(g, wholeSnake[0].getSX(), wholeSnake[0].getSY() + 10, 2);
            }
            if (direction == 3)
            {
                wholeSnake[0].move(g, wholeSnake[0].getSX() - 10, wholeSnake[0].getSY(), 3);
            }
            if (direction == 4)
            {
                wholeSnake[0].move(g, wholeSnake[0].getSX() + 10, wholeSnake[0].getSY(), 4);
            }
            /*If head is touches the border Game Over*/
            if (wholeSnake[0].getSX() < 0 || wholeSnake[0].getSX() > 490 || wholeSnake[0].getSY() < 0 || wholeSnake[0].getSY() > 490)
            {
                aniTimer.Stop();
                gameoverLabel.Show();
                keepPlaying = false;
                player.controls.stop();
            }

            /*Food*/
            if (ate)
            {
                xNew = rand.Next(0,49);
                yNew = rand.Next(0,49);
                allFoods.Add(new Food());
                allFoods[allFoods.Count - 1].add(g, xNew, yNew);
            }
            ate = false;
            this.Refresh();
            scoreLabel.Text = score.ToString() ;

            /*Code for the timer*/
            time -= 0.05;
            timerLabel.Text = String.Format("{0:0.00}", time);
            if (time <= 0.00)
            {
                aniTimer.Stop();
                gameoverLabel.Show();
                keepPlaying = false;
                player.controls.stop();
            }

            /*Repeat Music*/
            if (player.playState != WMPPlayState.wmppsPlaying && keepPlaying)
            {
                player.controls.play();

            }
        }

        /*Key down Code for Arrows*/
        private void q523_KeyDown(object sender, KeyEventArgs e)
        {
            string result = e.KeyData.ToString();
            switch (result)
            {
                case "Up":
                    if (previous != 2)
                    {
                        direction = 1;
                    }
                    break;
                case "Down":
                    if (previous != 1)
                    {
                        direction = 2;
                    }
                    break;
                case "Left":
                    if (previous != 4)
                    {
                        direction = 3;
                    }
                    break;
                case "Right":
                    if (previous != 3)
                    {
                        direction = 4;
                    }
                    break;
                default:
                    break;
            }
            previous = direction;
            /*When key is pressed, starts the game/ticker*/
            if (!started)
            {
                aniTimer.Start();
                started = true;
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Graphics g = Graphics.FromImage(myCanvas);
            /*Sets high score*/
            if (score > int.Parse(highscoreLabel.Text))
            {
                highscoreLabel.Text = score.ToString();
            }
            g.Clear(Color.Black);
            keepPlaying = true;
            wholeSnake.Clear();
            wholeSnake.Add(new Snake());
            wholeSnake[0].add(g, x, y);
            time = 60.00;
            started = false;
            gameoverLabel.Hide();
            score = 0;
            allFoods.Clear();
            /*Adds food again*/
            for (int i = 0; i < 5; i++)
            {
                xNew = rand.Next(0, 49);
                yNew = rand.Next(0, 49);
                allFoods.Add(new Food());
                allFoods[i].add(g, xNew, yNew);
            }
            ate = false;
            
        }

        /*Opens the about form giving instructions*/
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.Count == 1)
            {
                AboutBox1 about = new AboutBox1();
                about.Show();
            }
        }

    }

    /*Food Class*/
    public class Food
    {
        private SolidBrush foodBrush = new SolidBrush(Color.Yellow);
        private SolidBrush removeBrush = new SolidBrush(Color.Black);
        private int fX;
        private int fY;
        bool dead;

        /*default constructor*/
        public Food()
        {
            fX = 0;
            fY = 0;
            dead = false;
        }

        /*Adds food at specific coords*/
        public void add(Graphics g, int newX, int newY)
        {
            fX = newX;
            fY = newY;
            g.FillEllipse(foodBrush, 10 * fX, 10 * fY, 10, 10);
        }
        public int getfX()
        {
            return 10*fX;
        }
        public int getfY()
        {
            return 10*fY;
        }
        public bool getDead()
        {
            return dead;
        }
        /*Removes the touched food and sets to never record again*/
        public void remove(Graphics g)
        {
            g.FillEllipse(removeBrush, 10 * fX, 10 * fY, 10, 10);
            dead = true;
        }

    }
    /*Snake Class*/
    public class Snake
    {
        private SolidBrush snakeBrush = new SolidBrush(Color.Green);
        private SolidBrush gone = new SolidBrush(Color.Black);
        private int width;
        private int height;
        private int sX;
        private int sY;

        /*default constructor*/
        public Snake()
        {
            width = 10;
            height = 10;
        }

        /*Adds the Head*/
        public void add(Graphics g, int xLoc, int yLoc)
        {
            sX = xLoc;
            sY = yLoc;
            g.FillEllipse(snakeBrush, sX, sY, width, height);
        }

        /*Moves with directional dependent on arrow hits*/
        public void move(Graphics g, int xLoc, int yLoc, int dir)
        {
            sX = xLoc;
            sY = yLoc;
            g.FillEllipse(snakeBrush, xLoc, yLoc, width, height);
            switch (dir)//moves on what direction is given
            {
                case 1:
                    yLoc += 10;
                    g.FillEllipse(gone, xLoc, yLoc, width, height);
                    break;
                case 2:
                    yLoc -= 10;
                    g.FillEllipse(gone, xLoc, yLoc, width, height);
                    break;
                case 3:
                    xLoc += 10;
                    g.FillEllipse(gone, xLoc, yLoc, width, height);
                    break;
                case 4:
                    xLoc -= 10;
                    g.FillEllipse(gone, xLoc, yLoc, width, height);
                    break;
                default:
                    break;
            }
        }

        /*returns x value*/
        public int getSX()
        {
            return sX;
        }

        /*returns y value*/
        public int getSY()
        {
            return sY;
        }

        /*checks if circle is touching another circle*/
        public bool IsPointInside(Point p)
        {
            double t;
            double dis = 0;
            t = ((p.X - sX) * ((sX - 5) - sX) + (p.Y - sY) * ((sY - 5) - sY)) /
                (Math.Pow((sX-5) - sX, 2) + Math.Pow((sY-5) - sY, 2));
            if (t <= 0)
            {
                dis = Math.Sqrt(Math.Pow(p.X - sX, 2) + Math.Pow(p.Y - sY, 2));
            }
            if (t > 0 && t < 1)
            {
                dis = Math.Sqrt(Math.Pow(p.X - sX - t * (sX-5) + t * sX, 2) +
                    Math.Pow(p.Y - sY - t * (sY-5) + t * sY, 2));
            }
            if (t >= 1)
            {
                dis = Math.Sqrt(Math.Pow(p.X - (sX-5), 2) + Math.Pow(p.Y - (sY-5), 2));
            }
            return (dis < 10);
        }

    }

}