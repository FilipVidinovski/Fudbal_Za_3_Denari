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
        List<PictureBox> buttonList = new List<PictureBox>(11);
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

            setupGameItems();
            generateMenu();
        }

        private void setupGameItems()
        {
            gameTimer.Interval = 16;
            gameTimer.Tick += new EventHandler(gameTimer_Tick);
        }

        private void generateMenu()
        {
            FudbalZa3DenariLogo.Size = new Size(500, 250);
            FudbalZa3DenariLogo.Location =new Point((this.Width - FudbalZa3DenariLogo.Width) / 2,50);
            FudbalZa3DenariLogo.Image = Resources.Logo_temp;
            this.Controls.Add(FudbalZa3DenariLogo);

            PictureBox buttonLevels = new PictureBox();
            styleButton(buttonLevels);
            buttonLevels.Location =new Point((this.Width - buttonLevels.Width) / 2,350);
            buttonLevels.Image = Resources.levels;
            buttonLevels.Click += new EventHandler(buttonLevels_Click);
            buttonList.Add(buttonLevels);
            this.Controls.Add(buttonLevels);
            buttonLevels.Show();

            PictureBox buttonQuit = new PictureBox();
            styleButton(buttonQuit);
            buttonQuit.Location = new Point((this.Width - buttonQuit.Width) / 2,420);
            buttonQuit.Image = Resources.Quit;
            buttonQuit.Click += new EventHandler(buttonQuit_Click);
            buttonList.Add(buttonQuit);
            this.Controls.Add(buttonQuit);
            buttonQuit.Show();

            PictureBox buttonlevel1 = new PictureBox();
            styleButton(buttonlevel1);
            buttonlevel1.Location = new Point((this.Width / 8) - (buttonlevel1.Width / 2), 350);
            buttonlevel1.Image = Resources.level1;
            buttonlevel1.Click += new EventHandler(buttonlevel1_Click);
            buttonList.Add(buttonlevel1);
            this.Controls.Add(buttonlevel1);


            PictureBox buttonlevel2 = new PictureBox();
            styleButton(buttonlevel2);
            buttonlevel2.Location = new Point((this.Width / 8) + (this.Width / 4) - (buttonlevel2.Width / 2),350);
            buttonlevel2.Image = Resources.level2;
            buttonlevel2.Click += new EventHandler(buttonlevel2_Click);
            buttonList.Add(buttonlevel2);
            this.Controls.Add(buttonlevel2);


            PictureBox buttonlevel3 = new PictureBox();
            styleButton(buttonlevel3);
            buttonlevel3.Location = new Point((this.Width / 8) + (this.Width / 4) * 2 - (buttonlevel3.Width / 2), 350);
            buttonlevel3.Image = Resources.level3;
            buttonlevel3.Click += new EventHandler(buttonlevel3_Click);
            buttonList.Add(buttonlevel3);
            this.Controls.Add(buttonlevel3);


            PictureBox buttonlevel4 = new PictureBox();
            styleButton(buttonlevel4);
            buttonlevel4.Location =new Point((this.Width / 8) +(this.Width / 4) * 3 -(buttonlevel4.Width / 2),350);
            buttonlevel4.Image =Properties.Resources.level4;
            buttonlevel4.Click +=new EventHandler(buttonlevel4_Click);
            buttonList.Add(buttonlevel4);
            this.Controls.Add(buttonlevel4);


            PictureBox buttonBack = new PictureBox();
            styleButton(buttonBack);
            buttonBack.Location = new Point(this.Width / 2 -buttonBack.Width / 2,550);
            buttonBack.Image =Properties.Resources.Back;
            buttonBack.Click +=new EventHandler(buttonBack_Click);
            buttonList.Add(buttonBack);
            this.Controls.Add(buttonBack);


            PictureBox buttonPause = new PictureBox();
            styleButton(buttonPause);
            buttonPause.Location = new Point(this.Width - buttonPause.Width,5);
            buttonPause.Image = Resources.Pauza;
            buttonPause.Click += new EventHandler(buttonPause_Click);
            buttonList.Add(buttonPause);
            this.Controls.Add(buttonPause);


            PictureBox buttonResume = new PictureBox();
            styleButton(buttonResume);
            buttonResume.Location = new Point((this.Width / 8) * 2 -(buttonResume.Width / 2), 550);
            buttonResume.Image = Resources.Resume;
            buttonResume.Click += new EventHandler(buttonResume_Click);
            buttonList.Add(buttonResume);
            this.Controls.Add(buttonResume);


            PictureBox buttonRestart = new PictureBox();
            styleButton(buttonRestart);
            buttonRestart.Location = new Point((this.Width / 8) * 4 - (buttonRestart.Width / 2), 550);
            buttonRestart.Image = Resources.restart;
            buttonRestart.Click += new EventHandler(buttonRestart_Click);
            buttonList.Add(buttonRestart);
            this.Controls.Add(buttonRestart);


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
                return;

            if (!gameIsInProgress)
                return;

            gameManager.Update();

            this.Invalidate();
        }

        private void Form1_Paint(object sender,PaintEventArgs e)
        {
            if (gameManager == null)
                return;

            if (coin1 == null || coin2 == null || coin3 == null)
                return;

            DrawCoin(e.Graphics, coin1);
            DrawCoin(e.Graphics, coin2);
            DrawCoin(e.Graphics, coin3);

            Point mousePosition =
                this.PointToClient(MousePosition);

            DrawAiming(e.Graphics, coin1, mousePosition);
            DrawAiming(e.Graphics, coin2, mousePosition);
            DrawAiming(e.Graphics, coin3, mousePosition);
        }

        private void DrawCoin(Graphics graphics,Coin coin)
        {
            if (coin.Image == null)
                return;

            graphics.DrawImage(coin.Image,coin.Position.X - Coin.Radius,coin.Position.Y - Coin.Radius,Coin.Radius * 2,Coin.Radius * 2);
        }

        private void DrawAiming(Graphics graphics,Coin coin,Point mousePosition)
        {
            if (!coin.IsAiming)
                return;

            using (Pen pen = new Pen(Color.LightGray, 2f))
            {
                graphics.DrawEllipse(pen,coin.Position.X - Coin.Radius - 4f,coin.Position.Y - Coin.Radius - 4f,(Coin.Radius + 4f) * 2f,(Coin.Radius + 4f) * 2f);

                graphics.DrawEllipse(pen,coin.Position.X - Coin.Radius - 64f,coin.Position.Y - Coin.Radius - 64f,(Coin.Radius + 4f) * 8f,(Coin.Radius + 4f) * 8f);

                graphics.DrawLine(pen,coin.Position.X,coin.Position.Y,mousePosition.X,mousePosition.Y);
            }
        }



        private void Form1_MouseClick(object sender,MouseEventArgs e)
        {
            if (gameManager == null)
                return;

            gameManager.HandleMouseClick(e.Button,e.Location);

            this.Invalidate();
        }



        private void Form1_MouseMove(object sender,MouseEventArgs e)
        {
            this.Invalidate();
        }


        private void buttonLevels_Click(object sender,EventArgs e)
        {
            hideMenu();
            showLevels();
        }

        private void buttonQuit_Click(object sender,EventArgs e)
        {
            Application.Exit();
        }

        private void buttonlevel1_Click(object sender,EventArgs e)
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

        private void buttonlevel4_Click(object sender,EventArgs e)
        {
            hideLevels();
            setupLevel(4);
        }

        private void buttonBack_Click(object sender,EventArgs e)
        {
            hideLevels();
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
            hideLevel();

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

        private void buttonPause_Click(object sender,EventArgs e)
        {
            if (gameManager == null)
                return;

            gameIsPaused = true;
            gameIsInProgress = false;

            gameTimer.Stop();

            showPauseMenu();
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

        private void buttonRestart_Click(object sender,EventArgs e)
        {
            hidePauseMenu();

            gameIsPaused = false;
            gameIsInProgress = true;

            setupLevel(CurrentLevel);
        }

        private void buttonResume_Click(object sender,EventArgs e)
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

            coin1 = new Coin(640,290,Resources.coin);
            coin2 = new Coin(640,350,Resources.coin);
            coin3 = new Coin(640,410,Resources.coin);

            List<PictureBox> obstacles = new List<PictureBox>();

            gameManager =new GameManager(coin1,coin2,coin3,obstacles);

            gameIsPaused = false;
            gameIsInProgress = true;

            gameTimer.Start();


            this.Invalidate();
        }

        private void hideLevel()
        {
            buttonList[7].Hide();
        }

        private void level1()
        {
            CurrentLevel = 1;
        }

        private void level2()
        {
            CurrentLevel = 2;
        }

        private void level3()
        {
            CurrentLevel = 3;
        }

        private void level4()
        {
            CurrentLevel = 4;
        }
    }
}
