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
    public class Horizontal:IMovement
    {
        private int speed;
        private Point formBoundary;
        private string direction;
        private int offset;
        public Horizontal(int speed, Point formBoundary, string direction, int offset)
        {
            this.speed = speed;
            this.formBoundary = formBoundary;
            this.direction = direction;
            this.offset = offset;
        }
        public Point move(Point location)
        {
            if (location.X <= 0)
            {
                direction = "right";
            }
            else if (location.X + offset >= formBoundary.X)
            {
                direction = "left";
            }
            if (direction == "left")
            {
                location.X -= speed;
            }
            if (direction == "right")
            {
                location.X += speed;

            }
            return location;
        }
    }
}
