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
    public class Keyboard : IMovement
    {
        private int speed;
        private Point formBoundary;
        private int offset;
        private string ArrowAction = null;

        public Keyboard(int speed, Point formBoundary,int offset)
        {
            this.speed = speed;
            this.formBoundary = formBoundary;
            this.offset = offset;
        }
        public void keyPressedByUser(Keys keyCode)
        {  
            
            if (keyCode == Keys.Left)
            {   
                 ArrowAction =DirectionType.left.ToString();
            }
            else if (keyCode == Keys.Right)
            {
                ArrowAction = DirectionType.right.ToString();
            }
            else if (keyCode == Keys.Up)
            {
                ArrowAction = DirectionType.up.ToString();
            }
            else if (keyCode == Keys.Down)
            {
                ArrowAction = DirectionType.down.ToString();
            }
           
        }
        public Point move(Point location)
        {
            if (ArrowAction != null)
            {
                if (ArrowAction == DirectionType.left.ToString())
                {
                    if (location.X + speed > 40)
                    {
                        location.X -= speed;
                    }
                }
                if (ArrowAction == DirectionType.right.ToString())
                {
                    if (location.X+ speed <= formBoundary.X)
                    {
                        location.X += speed;
                    }
                }
                if (ArrowAction == DirectionType.up.ToString())
                {
                    if (location.Y > 60)
                    {
                        location.Y -= speed;
                    }
                }
                if (ArrowAction == DirectionType.down.ToString())
                {
                    
                    if (location.Y+speed+140 <= formBoundary.Y)
                    {
                        location.Y += speed;
                    }
                }
            }
            ArrowAction = null;
            return location;
        }
    }
}
