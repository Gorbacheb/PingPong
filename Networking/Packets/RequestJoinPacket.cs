using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Packets
{
    // Client Join Request
    public class RequestJoinPacket : Packet
    {
        public RequestJoinPacket() : base(PacketType.RequestJoin)
        {
        }
    }


}
