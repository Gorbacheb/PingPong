using Networking;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace PingPongServer
{
    // Type of collision with paddle
    public enum PaddleCollision
    {
        None,
        WithTop,
        WithFront,
        WithBottom
    };

    // This is the Paddle class for the Server
    public class Paddle
    {
        private DateTime _lastCollisiontime = DateTime.MinValue;
        private TimeSpan _minCollisionTimeGap = TimeSpan.FromSeconds(0.2);

        // Public data members
        public readonly PaddleSide Side;
        public int Score = 0;
        public Point Position = new Point();
        public int TopmostY { get; private set; }               // Bounds
        public int BottommostY { get; private set; }



        // Sets which side the paddle is
        public Paddle(PaddleSide side)
        {
            Side = side;
        }

        // What gets hit
        public Rectangle CollisionArea
        {
            get { return new Rectangle(Position, new Size(GameGeometry.PaddleSize)); }
        }

        // Puts the paddle in the middle of where it can move
        public void Initialize()
        {
            // Figure out where to place the paddle
            int x;
            if (Side == PaddleSide.Left)
                x = GameGeometry.GoalSize;
            else if (Side == PaddleSide.Right)
                x = GameGeometry.PlayArea.X - GameGeometry.PaddleSize.X * 2 - 10 - GameGeometry.GoalSize;
            else
                throw new Exception("Side is not `Left` or `Right`");

            Position = new Point(x, (GameGeometry.PlayArea.Y / 2) - (GameGeometry.PaddleSize.Y / 2));
            Score = 0;

            // Set bounds
            TopmostY = 0;
            BottommostY = GameGeometry.PlayArea.Y - GameGeometry.PaddleSize.Y;
        }

        // Sees what part of the Paddle collises with the ball (if it does)
        public bool Collides(Ball ball, out PaddleCollision typeOfCollision)
        {
            typeOfCollision = PaddleCollision.None;

            // Make sure enough time has passed for a new collisions
            // (this prevents a bug where a user can build up a lot of speed in the ball)
            if (DateTime.Now < (_lastCollisiontime.Add(_minCollisionTimeGap)))
                return false;

            // Top & bottom get first priority
            if (ball.CollisionArea.IntersectsWith(CollisionArea))
            {
                typeOfCollision = PaddleCollision.WithTop;
                _lastCollisiontime = DateTime.Now;
                return true;
            }

            if (ball.Position.Y <= 0 || ball.Position.Y + GameGeometry.BallSize.Y >= GameGeometry.PlayArea.Y)
            {
                typeOfCollision = PaddleCollision.WithBottom;
                _lastCollisiontime = DateTime.Now;
                return true;
            }

            // And check the front
            if (ball.Position.X <= 0 || ball.Position.X  + GameGeometry.BallSize.X >= GameGeometry.PlayArea.X)
            {
                typeOfCollision = PaddleCollision.WithFront;
                _lastCollisiontime = DateTime.Now;
                return true;
            }

            // Nope, nothing
            return false;
        }
    }
}
