using Networking;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PingPongServer
{
    public class Ball
    {
        private Random _random = new Random();

        // Public data members
        public Point Position = GameGeometry.ScreenCenter;
        public Point Speed = new Point(3, 3);

        // What gets hit
        public Rectangle CollisionArea
        {
            get { return new Rectangle(Position, new Size(new Point(2, 2))); }
        }

        // this is used to reset the postion of the ball to the center of the board
        public void Initialize()
        {
            // Center the ball
            Position = GameGeometry.ScreenCenter;

            // Set the velocity
            Speed = new Point(3, 3);

            // Randomize direction
            if (_random.Next() % 2 == 1)
                Speed.X *= -1;
            if (_random.Next() % 2 == 1)
                Speed.Y *= -1;
        }

        public void ServerSideUpdate()
        {
            Position.X += (int)(Speed.X);
            Position.Y += (int)(Speed.Y);
        }


        public void RandomizeSpeed()
        {
            var speedX = (int)(Speed.X * _random.Next(1, 60) / 1.0) % 7;
            var speedY = (int)(Speed.Y * _random.Next(1, 60) / 1.0) % 7;
            if (Math.Abs(speedX) >= 3)
            {
                Speed.X = speedX;
            }
            if (Math.Abs(speedY) >= 3)
            {
                Speed.Y = speedY;
            }
        }
    }
}
