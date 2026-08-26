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
        public int coinLeft, coinTop;
        Vector2 location = new Vector2();
        Vector2 xVelocity = new Vector2();
        Vector2 yVelocity = new Vector2();


        public void generateCoin(Form form)
        {

        }

        private void coinTimer_Tick(object sender, EventArgs e)
        {
            location.X += coin.Left;
            location.Y += coin.Top;

            
        }
    }
}
