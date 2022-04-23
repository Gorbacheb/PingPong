using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Packets
{
    // Ack packet for the one above
    public class AcceptJoinAckPacket : Packet
    {
        public AcceptJoinAckPacket() : base(PacketType.AcceptJoinAck)
        {
        }
    }
}
