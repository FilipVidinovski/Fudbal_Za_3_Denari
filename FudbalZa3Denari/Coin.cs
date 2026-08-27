using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FudbalZa3Denari
{
    internal class Coin
    {
        PictureBox coin = new PictureBox();
        Timer coinTimer = new Timer();
        public int coinLeft = 640, coinTop = 360;
        Vector2 location = new Vector2();
        Vector2 Velocity = new Vector2();


        public void generateCoin(Form form)
        {
            coin.Size = new System.Drawing.Size(30, 30);
            coin.BackColor = System.Drawing.Color.Transparent;
            coin.Image = Properties.Resources.coin;
            coin.Location = new System.Drawing.Point(coinLeft, coinTop);
            coin.BringToFront();
        }

        private void coinTimer_Tick(object sender, EventArgs e)
        {
            location.X += coin.Left;
            location.Y += coin.Top;

            if (location.X+Velocity.X < 140 || location.X+Velocity.X > 1140)
            {
                Velocity.X = -Velocity.X;
            }
            if (location.Y+Velocity.Y < 110 || location.Y+Velocity.Y > 610)
            {
                Velocity.Y = -Velocity.Y;
            }

            coin.Left = (int)location.X;
            coin.Top = (int)location.Y;

            Velocity.X *= 0.99f;
            Velocity.Y *= 0.99f;

            location.X += Velocity.X;
            location.Y += Velocity.Y;
        }
    }
}
