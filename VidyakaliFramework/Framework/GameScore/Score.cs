using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.GameScore
{
    public class Score
    {
        private static float gamePoint;
        public static void updateScore(float increment)
        {
            gamePoint += increment;
        }
        public static float GamePoint { get => gamePoint; set => gamePoint = value; }
    }
}
