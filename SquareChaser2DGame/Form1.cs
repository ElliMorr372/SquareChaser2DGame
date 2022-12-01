using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//Elliana Morrison: December 1st, 2022
//A square chaser game that has a speed powerup, a point square,
//and two players that can move in any orentation

namespace SquareChaser2DGame
{
    public partial class Form1 : Form
    {
        Rectangle player1 = new Rectangle();
        Rectangle player2 = new Rectangle();
        Rectangle pointSquare = new Rectangle();
        Rectangle speedBoost = new Rectangle();

        SolidBrush pinkBrush = new SolidBrush(Color.Fuchsia);
        SolidBrush blueBrush = new SolidBrush(Color.Cyan);
        SolidBrush whiteBrush = new SolidBrush(Color.White);
        SolidBrush greenBrush = new SolidBrush(Color.Lime);
        Pen greenPen = new Pen(Color.Green);

        int player1Score = 0;
        int player2Score = 0;

        int player1Speed = 6;
        int player2Speed = 6;

        bool wDown = false;
        bool sDown = false;
        bool upArrowDown = false;
        bool downArrowDown = false;

        bool aDown = false;
        bool dDown = false;
        bool leftDown = false;
        bool rightDown = false;

        Random randGen = new Random();
        Stopwatch stopwatch = new Stopwatch();

        SoundPlayer Boost = new SoundPlayer(Properties.Resources.UFO);
        SoundPlayer Point = new SoundPlayer(Properties.Resources.Horn);
        SoundPlayer Win = new SoundPlayer(Properties.Resources.Tada);

        public Form1()
        {
            InitializeComponent();
            winLabel.Visible = false;
            restartButton.Visible = false;
            int p1XPosition = randGen.Next(25, 551);
            int p1YPosition = randGen.Next(25, 551);
            int p2XPosition = randGen.Next(25, 551);
            int p2YPosition = randGen.Next(25, 551);
            player1 = new Rectangle(p1XPosition, p1YPosition, 25, 25);
            player2 = new Rectangle(p2XPosition, p2YPosition, 25, 25);
            Draw_SpeedBooster();
            Draw_PointSquare();
        }

        private void Draw_SpeedBooster()
        {
            int boostXPosition = randGen.Next(25, 551);
            int boostYPosition = randGen.Next(25, 551);

            speedBoost = new Rectangle(boostXPosition, boostYPosition, 13, 13);
        }

        private void Draw_PointSquare()
        {
            int pointXPosition = randGen.Next(25, 551);
            int pointYPosition = randGen.Next(25, 551);
            pointSquare = new Rectangle(pointXPosition, pointYPosition, 13, 13);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = false;
                    break;
                case Keys.S:
                    sDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.A:
                    aDown = false;
                    break;
                case Keys.D:
                    dDown = false;
                    break;
                case Keys.Left:
                    leftDown = false;
                    break;
                case Keys.Right:
                    rightDown = false;
                    break;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    wDown = true;
                    break;
                case Keys.S:
                    sDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.A:
                    aDown = true;
                    break;
                case Keys.D:
                    dDown = true;
                    break;
                case Keys.Left:
                    leftDown = true;
                    break;
                case Keys.Right:
                    rightDown = true;
                    break;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(pinkBrush, player1);
            e.Graphics.FillRectangle(blueBrush, player2);
            e.Graphics.FillRectangle(whiteBrush, pointSquare);
            e.Graphics.DrawEllipse(greenPen, speedBoost);
            e.Graphics.FillEllipse(greenBrush, speedBoost);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //move player 1 up and down
            if (wDown == true && player1.Y > 0)
            {
                player1.Y -= player1Speed;
            }

            if (sDown == true && player1.Y < this.Height - player1.Height)
            {
                player1.Y += player1Speed;
            }

            //move player 1 left and right
            if (aDown == true && player1.X > 0)
            {
                player1.X -= player1Speed;
            }

            if (dDown == true && player1.X < this.Width - player1.Width)
            {
                player1.X += player1Speed;
            }

            //move player 2 up and down
            if (upArrowDown == true && player2.Y > 0)
            {
                player2.Y -= player2Speed;
            }

            if (downArrowDown == true && player2.Y < this.Height - player2.Height)
            {
                player2.Y += player2Speed;
            }

            //move player 2 left and right
            if (leftDown == true && player2.X > 0)
            {
                player2.X -= player2Speed;
            }

            if (rightDown == true && player2.X < this.Width - player2.Width)
            {
                player2.X += player2Speed;
            }

            //check  if  either player intersects with pointSquare

            if (player1.IntersectsWith(pointSquare))
            {
                player1Score++;
                Point.Play();
                p1ScoreLabel.Text = $"{player1Score}";
                Draw_PointSquare();
            }
            else if (player2.IntersectsWith(pointSquare))
            {
                player2Score++;
                Point.Play();
                p2ScoreLabel.Text = $"{player2Score}";
                Draw_PointSquare();
            }

            //check  if  either player intersects with speedBooster

            if (player1.IntersectsWith(speedBoost))
            {
                stopwatch.Start();
                Boost.Play();
                player1Speed = 9;
                Draw_SpeedBooster();
            }
            else if (player2.IntersectsWith(speedBoost))
            {
                stopwatch.Start();
                Boost.Play();
                player2Speed = 9;
                Draw_SpeedBooster();
            }

            if (stopwatch.ElapsedMilliseconds >= 4000)
            {
                player1Speed = 6;
                player2Speed = 6;
                stopwatch.Stop();
                stopwatch.Reset();
            }

            // check score and stop game if either player is at 3 
            if (player1Score == 5)
            {
                Win.Play();
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.Text = "Player  1  Wins!!";
                restartButton.Visible = true;
            }
            else if (player2Score == 5)
            {
                Win.Play();
                gameTimer.Enabled = false;
                winLabel.Visible = true;
                winLabel.Text = "Player  2  Wins!!";
                restartButton.Visible = true;
            }

            Refresh();
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            gameTimer.Enabled = true;
            player1Score = 0;
            player2Score = 0;
            winLabel.Text = "";
            winLabel.Visible = false;
            restartButton.Visible = false;
            int p1XPosition = randGen.Next(25, 551);
            int p1YPosition = randGen.Next(25, 551);
            int p2XPosition = randGen.Next(25, 551);
            int p2YPosition = randGen.Next(25, 551);
            player1 = new Rectangle(p1XPosition, p1YPosition, 25, 25);
            player2 = new Rectangle(p2XPosition, p2YPosition, 25, 25);
            Draw_SpeedBooster();
            Draw_PointSquare();

            this.Focus();
        }
    }
}
