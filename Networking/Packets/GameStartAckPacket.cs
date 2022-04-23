using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Packets
{
    // Ack for the packet above
    public class GameStartAckPacket : Packet
    {
        public GameStartAckPacket() : base(PacketType.GameStartAck)
        {
        }
    }
}
