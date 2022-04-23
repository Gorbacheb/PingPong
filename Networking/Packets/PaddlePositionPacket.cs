using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Packets
{
    // Sent by the client to tell the server it's Y Position for the Paddle
    public class PaddlePositionPacket : Packet
    {
        // The Paddle's Y position
        public int Y
        {
            get { return BitConverter.ToInt32(Payload, 0); }
            set { BitConverter.GetBytes(value).CopyTo(Payload, 0); }
        }

        public PaddlePositionPacket() : base(PacketType.PaddlePosition)
        {
            Payload = new byte[sizeof(float)];

            // Default value is zero
            Y = 0;
        }

        public PaddlePositionPacket(byte[] bytes) : base(bytes)
        {
        }

        public override string ToString()
        {
            return string.Format("[Packet:{0}\n  timestamp={1}\n  payload size={2}" +
                "\n  Y={3}]",
                this.Type, new DateTime(Timestamp), Payload.Length, Y);
        }
    }
}
