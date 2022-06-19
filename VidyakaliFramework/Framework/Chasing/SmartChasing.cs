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

namespace Framework.Chasing
{
    public class SmartChasing:IChasing
    {
        private string direction="left";
        private int speed;
        private GoPictureBox enemy;
        private GoPictureBox playerMovement;
        public SmartChasing( int speed)
        {
            this.speed = speed;
        }

        public void performChasing(IGame game,GoPictureBox player, GoPictureBox enemy)
        {
            this.playerMovement = player;
            this.enemy = enemy;
            if (enemy.Pbx.Left > playerMovement.Pbx.Left)
            {
                enemy.Pbx.Left -= speed;
            }
            if (enemy.Pbx.Left < playerMovement.Pbx.Left)
            {
                enemy.Pbx.Left += speed;
            }
            if (enemy.Pbx.Top > playerMovement.Pbx.Top)
            {
                enemy.Pbx.Top -= speed;
            }
            if (enemy.Pbx.Top < playerMovement.Pbx.Top)
            {
                enemy.Pbx.Top += speed;
            }
        }
    }
}
