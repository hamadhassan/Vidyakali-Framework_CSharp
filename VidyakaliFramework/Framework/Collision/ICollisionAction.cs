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
    public interface ICollisionAction
    {
        void removePictureBoxObject(IGame game, GoPictureBox source1, GoPictureBox source2);
        void removeProgressBarObject(IGame game,GoProgressBar goProgressBar);
    }
}
