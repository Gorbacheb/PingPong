using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Networking
{
    public static class GameGeometry
    {
        public static readonly Point PlayArea = new Point(800, 570);    // Client area
        public static readonly Point ScreenCenter  = new Point(PlayArea.X / 2, PlayArea.Y / 2);
        public static readonly Point BallSize = new Point(16, 16);        // Size of Ball
        public static readonly Point PaddleSize = new Point(18, 168);     // Size of the Paddles
        public static readonly int GoalSize = 12;                       // Width behind paddle
        public static readonly int PaddleSpeed = 8;                // Speed of the paddle, (pixels/sec)
    }
}
