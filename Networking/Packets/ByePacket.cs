using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Packets
{
    // Sent by either the Client or the Server to end the game/connection
    public class ByePacket : Packet
    {
        public ByePacket() : base(PacketType.Bye)
        {
        }
    }
}
