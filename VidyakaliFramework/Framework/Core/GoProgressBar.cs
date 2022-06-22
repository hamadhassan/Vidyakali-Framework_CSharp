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
    public class GoProgressBar
    {
        private ProgressBar pbar;
        private ObjectType otype;//object type
       
        public GoProgressBar(ObjectType otype,int value, int leftSize, int topSize)
        {
            pbar = new ProgressBar();
            pbar.Value = value;
            this.otype = otype;
            pbar.Size = new Size(leftSize, topSize);
        }

        public ProgressBar Pbar { get => pbar; set => pbar = value; }
        public ObjectType Otype { get => otype; set => otype = value; }

        public void updateLocation(int left,int top)
        {
            if (pbar != null)
            {
                Pbar.Left = left;
                Pbar.Top = top;
            }
        }
    }
}
