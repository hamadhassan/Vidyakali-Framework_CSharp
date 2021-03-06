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

namespace Framework.Collision
{
    public class PLayerCollision : ICollisionAction
    {
        GoPictureBox player=null;
        public void removePictureBoxObject(IGame game, GoPictureBox source1, GoPictureBox source2)
        {
            if (source1.Otype == ObjectType.player)
            {
                player = source1;
            }
            else
            {
                player = source2;
            }
            game.risePlayerDieEvent(player.Pbx);
        }
        public void removeProgressBarObject(IGame game, GoProgressBar player)
        {
            game.risePlayerProgressBarDieEvent(player.Pbar);
        }
    }
}
