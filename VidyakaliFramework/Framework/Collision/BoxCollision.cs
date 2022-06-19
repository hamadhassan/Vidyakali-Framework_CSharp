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
    public class BoxCollision:ICollisionAction
    {
        GoPictureBox nextLevelbox = null;
        public void removePictureBoxObject(IGame game, GoPictureBox source1, GoPictureBox source2)
        {
            if (source1.Otype == ObjectType.nextLevel)
            {
                nextLevelbox = source1;
            }
            else
            {
                nextLevelbox = source2;
            }
            game.risePlayerDieEvent(nextLevelbox.Pbx);
        }
        public void removeProgressBarObject(IGame game, GoProgressBar goProgressBar) { }
    }
}
