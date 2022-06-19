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
    public class EnemyCollision : ICollisionAction
    {
        GoPictureBox enemy=null;
        public void removePictureBoxObject(IGame game, GoPictureBox source1, GoPictureBox source2)
        {
            if (source1.Otype == ObjectType.enemyIdel || source1.Otype == ObjectType.enemyRun)
            {
                enemy = source1;
            }
            else
            {
                enemy = source2;
            }
            game.riseEnemyDieEvent(enemy.Pbx);
        }
        public void removeProgressBarObject(IGame game,GoProgressBar enemy)
        {
            game.riseEnemyProgressBarDieEvent(enemy.Pbar);
        }
    }
}
