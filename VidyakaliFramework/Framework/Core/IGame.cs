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
    public interface IGame
    {
        void risePlayerDieEvent(PictureBox pb);
        void riseEnemyDieEvent(PictureBox pb);
        void risePlayerProgressBarDieEvent(ProgressBar pbar);
        void riseEnemyProgressBarDieEvent(ProgressBar pbar);
    }
}
