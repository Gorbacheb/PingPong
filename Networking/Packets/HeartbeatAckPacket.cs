using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Packets
{
    //сервер сообщает клиенту что знает, что он живой
    public class HeartbeatAckPacket : Packet
    {
        public HeartbeatAckPacket() : base(PacketType.HeartbeatAck)
        {
        }
    }
}
