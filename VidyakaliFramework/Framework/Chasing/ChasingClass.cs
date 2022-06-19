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
    public class ChasingClass
    {
        private ObjectType g1;
        private ObjectType g2;
        private IChasing behaviour;
        public ChasingClass(ObjectType g1, ObjectType g2, IChasing behaviour)
        { 
            this.g1 = g1;
            this.g2 = g2;
            this.behaviour = behaviour;
        }
        public ObjectType G1 { get => g1; set => g1 = value; }
        public ObjectType G2 { get => g2; set => g2 = value; }
        public IChasing Behaviour { get => behaviour; set => behaviour = value; }
    }
}
