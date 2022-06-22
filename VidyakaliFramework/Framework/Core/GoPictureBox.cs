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
    public class GoPictureBox
    {
        private PictureBox pbx;
        private IMovement movement;
        private ObjectType otype;//object type
        
        public GoPictureBox(ObjectType otype, Image img, int left, int top, IMovement movement)
        {
            this.otype = otype;
            pbx = new PictureBox();
            pbx.Image = img;
            pbx.Left = left;
            pbx.Top = top;
            pbx.SizeMode = PictureBoxSizeMode.AutoSize;
            pbx.BackColor = Color.Transparent;
            this.movement = movement;
        }
        public GoPictureBox(ObjectType otype, Image img, int left, int top)
        {
            this.otype = otype;
            pbx = new PictureBox();
            pbx.Image = img;
            pbx.Left = left;
            pbx.Top = top;
            pbx.SizeMode = PictureBoxSizeMode.AutoSize;
            pbx.BackColor = Color.Transparent;
        }
        public IMovement Movement { get => movement; set => movement = value; }
        public ObjectType Otype { get => otype; set => otype = value; }
        public PictureBox Pbx { get => pbx; set => pbx = value; }

        public void updateImage(Image img)
        {
            pbx.Image = img;
        }
        public void updateLocation(int gravity)
        {
            if (pbx!=null)
            {
                pbx.Location = movement.move(pbx.Location);
            }
        }
    }
}
