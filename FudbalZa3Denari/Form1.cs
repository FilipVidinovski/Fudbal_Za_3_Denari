using FudbalZa3Denari.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FudbalZa3Denari
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        PictureBox FudbalZa3DenariLogo = new PictureBox();
        PictureBox turnOrder = new PictureBox();
        TextBox score = new TextBox();
        List<PictureBox> obsticles = new List<PictureBox>();
        List<PictureBox> buttonList = new List<PictureBox>(11);
        /*
         * 0 - Levels
         * 1 - Quit
         * 2 - Level 1
         * 3 - Level 2
         * 4 - Level 3
         * 5 - Level 4
         * 6 - Back
         * 7 - Pause
         * 8 - Resume
         * 9 - Restart
         * 10 - Main Menu
         */
        private Coin coin1;
        private Coin coin2;
        private Coin coin3;

        private GameManager gameManager;

        Timer gameTimer = new Timer();

        bool gameIsInProgress = false;
        bool gameIsPaused = false;

        byte CurrentLevel = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.BackgroundImage = Resources.table;

            this.MouseMove += Form1_MouseMove;

            setupTimer();
            generateMenu();
        }

        private void setupTimer()
        {
            gameTimer.Interval = 16;
            gameTimer.Tick += new EventHandler(gameTimer_Tick);
        }

        private void generateMenu()
        {
            FudbalZa3DenariLogo.Size = new Size(500, 250);
            FudbalZa3DenariLogo.Location = new Point((this.Width - FudbalZa3DenariLogo.Width) / 2, 50);
            FudbalZa3DenariLogo.Image = Resources.Logo_temp;
            this.Controls.Add(FudbalZa3DenariLogo);

            turnOrder.Size = new Size(300, 100);
            turnOrder.Location = new Point((this.Width - turnOrder.Width) / 2, 1);
            turnOrder.Enabled = false;
            turnOrder.SizeMode = PictureBoxSizeMode.StretchImage;
            turnOrder.Hide();
            this.Controls.Add(turnOrder);

            score.Size = new Size(250, 40);
            score.Location = new Point((this.Width - score.Width) / 2, 615);
            score.ReadOnly = true;
            score.Enabled = false;
            score.TextAlign = HorizontalAlignment.Center;
            score.Font = new Font("Arial", 16, FontStyle.Bold);
            score.BorderStyle = BorderStyle.None;
            score.BackColor = Color.White;
            score.Hide();
            this.Controls.Add(score);

            //0
            PictureBox buttonLevels = new PictureBox();
            styleButton(buttonLevels);
            buttonLevels.Location = new Point((this.Width - buttonLevels.Width) / 2, 350);
            buttonLevels.Image = Resources.levels;
            buttonLevels.Click += new EventHandler(buttonLevels_Click);
            buttonList.Add(buttonLevels);
            this.Controls.Add(buttonLevels);
            buttonLevels.Show();

            //1
            PictureBox buttonQuit = new PictureBox();
            styleButton(buttonQuit);
            buttonQuit.Location = new Point((this.Width - buttonQuit.Width) / 2, 420);
            buttonQuit.Image = Resources.Quit;
            buttonQuit.Click += new EventHandler(buttonQuit_Click);
            buttonList.Add(buttonQuit);
            this.Controls.Add(buttonQuit);
            buttonQuit.Show();

            //2
            PictureBox buttonlevel1 = new PictureBox();
            styleButton(buttonlevel1);
            buttonlevel1.Location = new Point((this.Width / 8) - (buttonlevel1.Width / 2), 350);
            buttonlevel1.Image = Resources.level1;
            buttonlevel1.Click += new EventHandler(buttonlevel1_Click);
            buttonList.Add(buttonlevel1);
            this.Controls.Add(buttonlevel1);

            //3
            PictureBox buttonlevel2 = new PictureBox();
            styleButton(buttonlevel2);
            buttonlevel2.Location = new Point((this.Width / 8) + (this.Width / 4) - (buttonlevel2.Width / 2), 350);
            buttonlevel2.Image = Resources.level2;
            buttonlevel2.Click += new EventHandler(buttonlevel2_Click);
            buttonList.Add(buttonlevel2);
            this.Controls.Add(buttonlevel2);

            //4
            PictureBox buttonlevel3 = new PictureBox();
            styleButton(buttonlevel3);
            buttonlevel3.Location = new Point((this.Width / 8) + (this.Width / 4) * 2 - (buttonlevel3.Width / 2), 350);
            buttonlevel3.Image = Resources.level3;
            buttonlevel3.Click += new EventHandler(buttonlevel3_Click);
            buttonList.Add(buttonlevel3);
            this.Controls.Add(buttonlevel3);

            //5
            PictureBox buttonlevel4 = new PictureBox();
            styleButton(buttonlevel4);
            buttonlevel4.Location = new Point((this.Width / 8) + (this.Width / 4) * 3 - (buttonlevel4.Width / 2), 350);
            buttonlevel4.Image = Properties.Resources.level4;
            buttonlevel4.Click += new EventHandler(buttonlevel4_Click);
            buttonList.Add(buttonlevel4);
            this.Controls.Add(buttonlevel4);

            //6
            PictureBox buttonBack = new PictureBox();
            styleButton(buttonBack);
            buttonBack.Location = new Point(this.Width / 2 - buttonBack.Width / 2, 550);
            buttonBack.Image = Properties.Resources.Back;
            buttonBack.Click += new EventHandler(buttonBack_Click);
            buttonList.Add(buttonBack);
            this.Controls.Add(buttonBack);

            //7
            PictureBox buttonPause = new PictureBox();
            styleButton(buttonPause);
            buttonPause.Location = new Point(this.Width - buttonPause.Width, 5);
            buttonPause.Image = Resources.Pauza;
            buttonPause.Click += new EventHandler(buttonPause_Click);
            buttonList.Add(buttonPause);
            this.Controls.Add(buttonPause);

            //8
            PictureBox buttonResume = new PictureBox();
            styleButton(buttonResume);
            buttonResume.Location = new Point((this.Width / 8) * 2 - (buttonResume.Width / 2), 550);
            buttonResume.Image = Resources.Resume;
            buttonResume.Click += new EventHandler(buttonResume_Click);
            buttonList.Add(buttonResume);
            this.Controls.Add(buttonResume);

            //9
            PictureBox buttonRestart = new PictureBox();
            styleButton(buttonRestart);
            buttonRestart.Location = new Point((this.Width / 8) * 4 - (buttonRestart.Width / 2), 550);
            buttonRestart.Image = Resources.restart;
            buttonRestart.Click += new EventHandler(buttonRestart_Click);
            buttonList.Add(buttonRestart);
            this.Controls.Add(buttonRestart);

            //10
            PictureBox buttonMainMenu = new PictureBox();
            styleButton(buttonMainMenu);
            buttonMainMenu.Location = new Point((this.Width / 8) * 6 - (buttonMainMenu.Width / 2), 550);
            buttonMainMenu.Image = Resources.main_menu;
            buttonMainMenu.Click += new EventHandler(buttonMainMenu_Click);
            buttonList.Add(buttonMainMenu);
            this.Controls.Add(buttonMainMenu);
        }

        private void styleButton(PictureBox button)
        {
            button.Size = new Size(200, 50);
            button.BackColor = Color.Black;
            button.Hide();
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (gameManager == null)
            {
                return;
            }

            if (!gameIsInProgress)
            {
                return;
            }

            gameManager.Update();

            UpdateGameUI();

            this.Invalidate();
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (gameManager == null)
            {
                return;
            }

            if (coin1 == null || coin2 == null || coin3 == null)
            {
                return;
            }

            DrawCoin(e.Graphics, coin1);
            DrawCoin(e.Graphics, coin2);
            DrawCoin(e.Graphics, coin3);

            Point mousePosition = this.PointToClient(MousePosition);

            DrawAiming(e.Graphics, coin1, mousePosition);
            DrawAiming(e.Graphics, coin2, mousePosition);
            DrawAiming(e.Graphics, coin3, mousePosition);
        }

        private void DrawCoin(Graphics graphics, Coin coin)
        {
            if (coin.Image == null)
            {
                return;
            }

            graphics.DrawImage(coin.Image, coin.Position.X - Coin.Radius, coin.Position.Y - Coin.Radius, Coin.Radius * 2, Coin.Radius * 2);
        }

        private void DrawAiming(Graphics graphics, Coin coin, Point mousePosition)
        {
            if (!coin.IsAiming)
                return;

            using (Pen pen = new Pen(Color.Gray, 2f))
            {
                graphics.DrawEllipse(pen, coin.Position.X - Coin.Radius - 4f, coin.Position.Y - Coin.Radius - 4f, (Coin.Radius + 4f) * 2f, (Coin.Radius + 4f) * 2f);
                graphics.DrawEllipse(pen, coin.Position.X - Coin.Radius - 64f, coin.Position.Y - Coin.Radius - 64f, (Coin.Radius + 4f) * 8f, (Coin.Radius + 4f) * 8f);
                graphics.DrawLine(pen, coin.Position.X, coin.Position.Y, mousePosition.X, mousePosition.Y);
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (gameManager == null)
            {
                return;
            }

            gameManager.HandleMouseClick(e.Button, e.Location);

            UpdateGameUI();

            this.Invalidate();
        }


        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (gameManager == null)
            {
                return;
            }

            if (!gameManager.AnyCoinMoving())
            {
                this.Invalidate();
            }
        }


        private void buttonLevels_Click(object sender, EventArgs e)
        {
            hideMenu();
            showLevels();
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonlevel1_Click(object sender, EventArgs e)
        {
            hideLevels();
            setupLevel(1);
        }

        private void buttonlevel2_Click(object sender, EventArgs e)
        {
            hideLevels();
            setupLevel(2);
        }

        private void buttonlevel3_Click(object sender, EventArgs e)
        {
            hideLevels();
            setupLevel(3);
        }

        private void buttonlevel4_Click(object sender, EventArgs e)
        {
            hideLevels();
            setupLevel(4);
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            hideLevels();
            showMenu();
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (gameManager == null)
            {
                return;
            }

            gameIsPaused = true;
            gameIsInProgress = false;
            gameTimer.Stop();

            showPauseMenu();
        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {
            hidePauseMenu();

            gameIsPaused = false;
            gameIsInProgress = true;

            setupLevel(CurrentLevel);
        }

        private void buttonResume_Click(object sender, EventArgs e)
        {
            hidePauseMenu();

            gameIsPaused = false;
            gameIsInProgress = true;

            gameTimer.Start();

            this.Invalidate();
        }

        private void buttonMainMenu_Click(object sender, EventArgs e)
        {
            showMenu();
        }

        private void showMenu()
        {
            gameIsInProgress = false;
            gameIsPaused = false;

            gameTimer.Stop();

            coin1 = null;
            coin2 = null;
            coin3 = null;
            gameManager = null;

            hidePauseMenu();
            buttonList[7].Hide();
            clearObsticles();

            turnOrder.Hide();
            score.Hide();

            this.BackgroundImage = Resources.table;

            FudbalZa3DenariLogo.Show();
            buttonList[0].Show();
            buttonList[1].Show();

            this.Invalidate();
        }


        private void hideMenu()
        {
            FudbalZa3DenariLogo.Hide();
            buttonList[0].Hide();
            buttonList[1].Hide();
        }

        private void showLevels()
        {
            buttonList[2].Show();
            buttonList[3].Show();
            buttonList[4].Show();
            buttonList[5].Show();
            buttonList[6].Show();
        }

        private void hideLevels()
        {
            buttonList[2].Hide();
            buttonList[3].Hide();
            buttonList[4].Hide();
            buttonList[5].Hide();
            buttonList[6].Hide();
        }

        private void showPauseMenu()
        {
            buttonList[8].Show();
            buttonList[9].Show();
            buttonList[10].Show();
        }

        private void hidePauseMenu()
        {
            buttonList[8].Hide();
            buttonList[9].Hide();
            buttonList[10].Hide();
        }

        private void setupLevel(int level)
        {

            switch (level)
            {
                case 1:
                    level1();
                    break;

                case 2:
                    level2();
                    break;

                case 3:
                    level3();
                    break;

                default:
                    level4();
                    break;
            }

            buttonList[7].Show();

            coin1 = new Coin(640, 290, Resources.pixil_frame_0);
            coin2 = new Coin(640, 350, Resources.pixil_frame_0);
            coin3 = new Coin(640, 410, Resources.pixil_frame_0);


            gameManager = new GameManager(coin1, coin2, coin3, obsticles);

            gameIsPaused = false;
            gameIsInProgress = true;

            UpdateGameUI();

            gameTimer.Start();

            this.Invalidate();
        }

        private void level1()
        {
            CurrentLevel = 1;

            clearObsticles();
        }

        private void level2()
        {
            CurrentLevel = 2;

            clearObsticles();

            PictureBox box1 = new PictureBox();
            box1.Size = new Size(100, 200);
            box1.Location = new Point(340, 260);
            box1.Image = Resources.box_100x200;
            this.Controls.Add(box1);
            box1.Enabled = false;
            box1.Show();
            obsticles.Add(box1);

            PictureBox box2 = new PictureBox();
            box2.Size = new Size(100, 200);
            box2.Location = new Point(800, 260);
            box2.Image = Resources.box_100x200;
            this.Controls.Add(box2);
            box2.Enabled = false;
            box2.Show();
            obsticles.Add(box2);

        }

        private void level3()
        {
            CurrentLevel = 3;

            clearObsticles();

            PictureBox box1 = new PictureBox();
            box1.Size = new Size(200, 100);
            box1.Location = new Point(140 + 100 + 50, 110 + 100);
            box1.Image = Resources.box_200x100;
            this.Controls.Add(box1);
            box1.Enabled = false;
            box1.Show();
            obsticles.Add(box1);

            PictureBox box2 = new PictureBox();
            box2.Size = new Size(200, 100);
            box2.Location = new Point(140 + 100 + 50, 110 + 100 + 200);
            box2.Image = Resources.box_200x100;
            this.Controls.Add(box2);
            box2.Enabled = false;
            box2.Show();
            obsticles.Add(box2);

            PictureBox box3 = new PictureBox();
            box3.Size = new Size(200, 100);
            box3.Location = new Point(140 + 100 + 200 + 200 + 200 - 50, 110 + 100);
            box3.Image = Resources.box_200x100;
            this.Controls.Add(box3);
            box3.Enabled = false;
            box3.Show();
            obsticles.Add(box3);

            PictureBox box4 = new PictureBox();
            box4.Size = new Size(200, 100);
            box4.Location = new Point(140 + 100 + 200 + 200 + 200 - 50, 110 + 100 + 200);
            box4.Image = Resources.box_200x100;
            this.Controls.Add(box4);
            box4.Enabled = false;
            box4.Show();
            obsticles.Add(box4);

        }

        private void level4()
        {
            CurrentLevel = 4;

            clearObsticles();

            PictureBox box1 = new PictureBox();
            box1.Size = new Size(100, 100);
            box1.Location = new Point(140 + 100 + 50, 110);
            box1.Image = Resources.box_100x100;
            this.Controls.Add(box1);
            box1.Enabled = false;
            box1.Show();
            obsticles.Add(box1);

            PictureBox box2 = new PictureBox();
            box2.Size = new Size(100, 100);
            box2.Location = new Point(1140 - 100 - 50 - 100, 110);
            box2.Image = Resources.box_100x100;
            this.Controls.Add(box2);
            box2.Enabled = false;
            box2.Show();
            obsticles.Add(box2);

            PictureBox box3 = new PictureBox();
            box3.Size = new Size(100, 100);
            box3.Location = new Point(140 + 100 + 50, 610 - 100);
            box3.Image = Resources.box_100x100;
            this.Controls.Add(box3);
            box3.Enabled = false;
            box3.Show();
            obsticles.Add(box3);

            PictureBox box4 = new PictureBox();
            box4.Size = new Size(100, 100);
            box4.Location = new Point(1140 - 100 - 50 - 100, 610 - 100);
            box4.Image = Resources.box_100x100;
            this.Controls.Add(box4);
            box4.Enabled = false;
            box4.Show();
            obsticles.Add(box4);

            PictureBox box5 = new PictureBox();
            box5.Size = new Size(100, 100);
            box5.Location = new Point(140 + 100 + 50 + 100, 310);
            box5.Image = Resources.box_100x100;
            this.Controls.Add(box5);
            box5.Enabled = false;
            box5.Show();
            obsticles.Add(box5);

            PictureBox box6 = new PictureBox();
            box6.Size = new Size(100, 100);
            box6.Location = new Point(1140 - 100 - 50 - 100 - 100, 310);
            box6.Image = Resources.box_100x100;
            this.Controls.Add(box6);
            box6.Enabled = false;
            box6.Show();
            obsticles.Add(box6);


        }

        private void clearObsticles()
        {
            foreach (PictureBox obsticle in obsticles)
            {
                obsticle.Dispose();
            }
            obsticles = new List<PictureBox>();
        }

    private void UpdateGameUI()
        {
            if (gameManager == null)
            {
                return;
            }

            score.Text = "Player 1: " + gameManager.PlayerOneScore + "    Player 2: " + gameManager.PlayerTwoScore;

            if (gameManager.CurrentTurn == GameManager.TurnState.PlayerOne)
            {
                this.BackgroundImage = Resources.table_player_1;

                if (gameManager.FlicksRemaining == 3)
                {
                    turnOrder.Image = Resources.Player_1_Flicks_Left_3;
                }
                else if (gameManager.FlicksRemaining == 2)
                {
                    turnOrder.Image = Resources.Player_1_Flicks_Left_2;
                }
                else if (gameManager.FlicksRemaining == 1)
                {
                    turnOrder.Image = Resources.Player_1_Flicks_Left_1;
                }
            }
            else if (gameManager.CurrentTurn == GameManager.TurnState.PlayerTwo)
            {
                this.BackgroundImage = Resources.table_player_2;

                if (gameManager.FlicksRemaining == 3)
                {
                    turnOrder.Image = Resources.Player_2_Flicks_Left_3;
                }
                else if (gameManager.FlicksRemaining == 2)
                {
                    turnOrder.Image = Resources.Player_2_Flicks_Left_2;
                }
                else if (gameManager.FlicksRemaining == 1)
                {
                    turnOrder.Image = Resources.Player_2_Flicks_Left_1;
                }
            }

            turnOrder.Show();
            score.Show();
        }

    }
}
