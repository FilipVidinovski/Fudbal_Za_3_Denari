using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        #region buttonList
        /*
        0 - Levels
        1 - Quit
        2 - Level 1
        3 - Level 2
        4 - Level 3
        5 - Level 4
        6 - back
        7 - Pause
        8 - Resume
        9 - Restart
        10 - Main Menu
        11 - Next Level
         */
        #endregion
        #region gameObjects
        PictureBox table = new PictureBox();
        PictureBox leftGoal = new PictureBox();
        PictureBox rightGoal = new PictureBox();
        PictureBox turnCounter = new PictureBox();
        TextBox scoreBox = new TextBox();
        Coin coin1 = new Coin();
        Coin coin2 = new Coin();
        Coin coin3 = new Coin();

        bool gameIsInProgress = false, gameIsPaused = false;
        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.BackColor = Color.White;

            generateMenu();
            setupGameItems();
        }

        private void generateMenu()
        {
            FudbalZa3DenariLogo.Size = new Size(500, 250);
            FudbalZa3DenariLogo.Location = new Point((this.Width - FudbalZa3DenariLogo.Width) / 2, 50);
            FudbalZa3DenariLogo.Image = Properties.Resources.Logo_temp;
            this.Controls.Add(FudbalZa3DenariLogo);

            PictureBox buttonLevels = new PictureBox();
            styleButton(buttonLevels);
            buttonLevels.Location = new Point((this.Width - buttonLevels.Width) / 2, 350);
            buttonLevels.Image = Properties.Resources.levels;
            buttonLevels.Click += new EventHandler(buttonLevels_Click);
            buttonList.Add(buttonLevels);
            this.Controls.Add(buttonLevels);
            buttonLevels.Show();

            PictureBox buttonQuit = new PictureBox();
            styleButton(buttonQuit);
            buttonQuit.Location = new Point((this.Width - buttonQuit.Width) / 2, 420);
            buttonQuit.Image = Properties.Resources.Quit;
            buttonQuit.Click += new EventHandler(buttonQuit_Click);
            buttonList.Add(buttonQuit);
            this.Controls.Add(buttonQuit);
            buttonQuit.Show();

            PictureBox buttonlevel1 = new PictureBox();
            styleButton(buttonlevel1);
            buttonlevel1.Location = new Point((this.Width / 8) - (buttonlevel1.Width / 2), 350);
            buttonlevel1.Image = Properties.Resources.level1;
            buttonlevel1.Click += new EventHandler(buttonlevel1_Click);
            buttonList.Add(buttonlevel1);
            this.Controls.Add(buttonlevel1);

            PictureBox buttonlevel2 = new PictureBox();
            styleButton(buttonlevel2);
            buttonlevel2.Location = new Point((this.Width / 8) + (this.Width / 4) - (buttonlevel2.Width / 2), 350);
            buttonlevel2.Image = Properties.Resources.level2;
            buttonlevel2.Click += new EventHandler(buttonlevel2_Click);
            buttonList.Add(buttonlevel2);
            this.Controls.Add(buttonlevel2);

            PictureBox buttonlevel3 = new PictureBox();
            styleButton(buttonlevel3);
            buttonlevel3.Location = new Point((this.Width / 8) + (this.Width / 4) * 2 - (buttonlevel3.Width / 2), 350);
            buttonlevel3.Image = Properties.Resources.level3;
            buttonlevel3.Click += new EventHandler(buttonlevel3_Click);
            buttonList.Add(buttonlevel3);
            this.Controls.Add(buttonlevel3);

            PictureBox buttonlevel4 = new PictureBox();
            styleButton(buttonlevel4);
            buttonlevel4.Location = new Point((this.Width / 8) + (this.Width / 4) * 3 - (buttonlevel4.Width / 2), 350);
            buttonlevel4.Image = Properties.Resources.level4;
            buttonlevel4.Click += new EventHandler(buttonlevel4_Click);
            buttonList.Add(buttonlevel4);
            this.Controls.Add(buttonlevel4);

            PictureBox buttonBack = new PictureBox();
            styleButton(buttonBack);
            buttonBack.Location = new Point(this.Width / 2 - buttonBack.Width / 2, 550);
            buttonBack.Image = Properties.Resources.Back;
            buttonBack.Click += new EventHandler(buttonBack_Click);
            buttonList.Add(buttonBack);
            this.Controls.Add(buttonBack);

            PictureBox buttonPause = new PictureBox();
            styleButton(buttonPause);
            buttonPause.Location = new Point(this.Width-buttonPause.Width, 5);
            buttonPause.Image = Properties.Resources.Pauza;
            buttonPause.Click += new EventHandler(buttonPause_Click);
            buttonList.Add(buttonPause);
            this.Controls.Add(buttonPause);

            PictureBox buttonResume = new PictureBox();
            styleButton(buttonResume);
            buttonResume.Location = new Point((this.Width / 8) * 3 - (buttonResume.Width / 2), 550);
            buttonResume.Image = Properties.Resources.Resume;
            buttonResume.Click += new EventHandler(buttonPause_Click);
            buttonList.Add(buttonResume);
            this.Controls.Add(buttonResume);
        }

        private void setupGameItems()
        {
            table.Size = new Size(1000, 500);
            table.Location = new Point(140, 110);
            table.Image = Properties.Resources.table;
            this.Controls.Add(table);


        }

        

        private void styleButton(PictureBox button)
        {
            button.Size = new Size(200, 50);
            button.BackColor = Color.Black;
            button.Hide();
        }

        private void buttonLevels_Click(object sender, EventArgs e)
        {
            hideMenu();
            showLevels();
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

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonlevel1_Click(object sender, EventArgs e)
        {
            hideLevels();
            setupLevel();

            level1();
        }

        

        private void buttonlevel2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void buttonlevel3_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void buttonlevel4_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            showMenu();
            hideLevels();
        }

        private void showMenu()
        {
            FudbalZa3DenariLogo.Show();
            buttonList[0].Show();
            buttonList[1].Show();
        }
        private void hideLevels()
        {
            buttonList[2].Hide();
            buttonList[3].Hide();
            buttonList[4].Hide();
            buttonList[5].Hide();
            buttonList[6].Hide();
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void setupLevel()
        {
            
        }

        private void level1()
        {

        }
    }
}
