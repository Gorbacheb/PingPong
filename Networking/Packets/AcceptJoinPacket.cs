using PingPong;
using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Packets
{
    // Server Accept Request Join, assigns a paddle
    public class AcceptJoinPacket : Packet
    {
        // Paddle side
        public PaddleSide Side
        {
            get { return (PaddleSide)BitConverter.ToUInt32(Payload, 0); }
            set { Payload = BitConverter.GetBytes((uint)value); }
        }

        public AcceptJoinPacket() : base(PacketType.AcceptJoin)
        {
            Payload = new byte[sizeof(PaddleSide)];

            // Set a dfeault paddle of None
            Side = PaddleSide.None;
        }

        public AcceptJoinPacket(byte[] bytes) : base(bytes)
        {
        }
    }
}
