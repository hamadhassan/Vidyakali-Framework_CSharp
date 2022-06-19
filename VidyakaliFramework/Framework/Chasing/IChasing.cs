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
using Framework.Chasing;

namespace Framework.Chasing
{
    public interface IChasing
    {
        void performChasing(IGame game,GoPictureBox source1, GoPictureBox source2);
    }
}
