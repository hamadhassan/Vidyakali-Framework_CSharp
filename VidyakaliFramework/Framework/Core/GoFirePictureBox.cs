using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Framework.Movement;
using Framework.Collision;
using Framework.Core;
namespace Framework.Core
{
    public class GoFirePictureBox
    {
        private PictureBox pbx;
        private string direction;
        private int speed;
        public GoFirePictureBox(Image img, string direction,int speed, int Playerleft, int Playertop)
        {
            pbx = new PictureBox();
            pbx.Image = img;
            pbx.Left = Playerleft;
            pbx.Top = Playertop;
            pbx.SizeMode = PictureBoxSizeMode.AutoSize;
            pbx.BackColor = Color.Transparent;
            this.direction = direction;
            this.speed = speed;
        }

        public PictureBox Pbx { get => pbx; set => pbx = value; }
        public void fire()
        {
            if (direction == "left")
            {
                Pbx.Left -= speed;
            }
            else if (direction == "right")
            {
                pbx.Left += speed;
            }
            else if (direction == "up")
            {
                pbx.Top -= speed;
            }
            else if (direction == "down")
            {
                pbx.Top += speed;
            }
            //if (keyCode == Keys.A && direction == "left")
            //{
            //    Pbx.Left -= speed;
            //}
            //else if (keyCode == Keys.D && direction == "right")
            //{
            //    pbx.Left += speed;
            //}
            //else if (keyCode == Keys.W)
            //{
            //    pbx.Top -= speed;
            //}
            //else if (keyCode == Keys.S)
            //{
            //    pbx.Top += speed;
            //}
        }
    }
}
