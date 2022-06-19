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
namespace Framework.Movement
{
    public class Fire:IMovement
    {
        private int speed;
        private Point formBoundary;
        private int offsetLeft;
        private int offsetTop;
        private string bulletDirection;
        private string ArrowAction=null;
        public Fire(int speed, Point formBoundary, int offsetLeft, int offsetTop, string bulletDirection)
        {
            this.speed = speed;
            this.formBoundary = formBoundary;
            this.offsetLeft = offsetLeft;
            this.offsetTop = offsetTop;
            this.bulletDirection = bulletDirection;
        }
        public void addBullet(IGame game,PictureBox box)
        {
            game.riseFireCreateEvent(box);
        }
        public void keyPressedByUserForFire(Keys keyCode,PictureBox obj)
        {
            if (keyCode == Keys.A)
            {
               ArrowAction = DirectionType.left.ToString();
                

            }
            else if (keyCode == Keys.D)
            {
                ArrowAction = DirectionType.right.ToString();
            }
            else if (keyCode == Keys.W)
            {
                ArrowAction = DirectionType.up.ToString();
            }
            else if (keyCode == Keys.S)
            {
                ArrowAction = DirectionType.down.ToString();
            }
        }
        public Point move(Point location)
        {
            if (ArrowAction == DirectionType.left.ToString())
            {
                location.X -= speed;
            }
            if (ArrowAction == DirectionType.right.ToString() )
            {
                location.X += speed;
            }
            if (ArrowAction == DirectionType.up.ToString() )
            {
                location.Y -= speed;
            }
            if (ArrowAction == DirectionType.down.ToString())
            {
                location.Y += speed;
            }
            return location;
        }
    }
}
