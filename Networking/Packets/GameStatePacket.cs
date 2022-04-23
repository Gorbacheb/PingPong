using System;
using System.Drawing;


namespace Networking.Packets
{
    // Sent by the server to thd Clients to update the game information
    public class GameStatePacket : Packet
    {
        // Payload array offets
        private static readonly int _leftYIndex = 0;
        private static readonly int _rightYIndex = 4;
        private static readonly int _ballPositionIndex = 8;
        private static readonly int _leftScoreIndex = 16;
        private static readonly int _rightScoreIndex = 20;

        // The Left Paddle's Y position
        public int LeftY
        {
            get { return BitConverter.ToInt32(Payload, _leftYIndex); }
            set { BitConverter.GetBytes(value).CopyTo(Payload, _leftYIndex); }
        }

        // Right Paddle's Y Position
        public int RightY
        {
            get { return BitConverter.ToInt32(Payload, _rightYIndex); }
            set { BitConverter.GetBytes(value).CopyTo(Payload, _rightYIndex); }
        }

        // Ball position
        public Point BallPosition
        {
            get
            {
                return new Point(
                    BitConverter.ToInt32(Payload, _ballPositionIndex),
                    BitConverter.ToInt32(Payload, _ballPositionIndex + sizeof(float))
                );
            }
            set
            {
                BitConverter.GetBytes(value.X).CopyTo(Payload, _ballPositionIndex);
                BitConverter.GetBytes(value.Y).CopyTo(Payload, _ballPositionIndex + sizeof(float));
            }
        }

        // Left Paddle's Score
        public int LeftScore
        {
            get { return BitConverter.ToInt32(Payload, _leftScoreIndex); }
            set { BitConverter.GetBytes(value).CopyTo(Payload, _leftScoreIndex); }
        }

        // Right Paddle's Score
        public int RightScore
        {
            get { return BitConverter.ToInt32(Payload, _rightScoreIndex); }
            set { BitConverter.GetBytes(value).CopyTo(Payload, _rightScoreIndex); }
        }

        public GameStatePacket() : base(PacketType.GameState)
        {
            // Allocate data for the payload (we really shouldn't hardcode this in...)
            Payload = new byte[24];

            // Set default data
            LeftY = 0;
            RightY = 0;
            BallPosition = new Point();
            LeftScore = 0;
            RightScore = 0;
        }

        public GameStatePacket(byte[] bytes) : base(bytes)
        {
        }

        public override string ToString()
        {
            return string.Format(
                "[Packet:{0}\n  timestamp={1}\n  payload size={2}" +
                "\n  LeftY={3}" +
                "\n  RightY={4}" +
                "\n  BallPosition={5}" +
                "\n  LeftScore={6}" +
                "\n  RightScore={7}]",
                this.Type, new DateTime(Timestamp), Payload.Length, LeftY, RightY, BallPosition, LeftScore, RightScore);
        }
    }
}
