using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Packets
{
    // Client tells the Server it's alive
    public class HeartbeatPacket : Packet
    {
        public HeartbeatPacket() : base(PacketType.Heartbeat)
        {
        }
    }
}
