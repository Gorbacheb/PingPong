using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Packets
{
    // Sent by the Server to tell the client they should play a sound effect
    public class PlaySoundEffectPacket : Packet
    {
        public string SFXName
        {
            get { return Encoding.UTF8.GetString(Payload); }
            set { Payload = Encoding.UTF8.GetBytes(value); }
        }

        public PlaySoundEffectPacket()
            : base(PacketType.PlaySoundEffect)
        {
            SFXName = "";
        }

        public PlaySoundEffectPacket(byte[] bytes)
            : base(bytes)
        {
        }

        public override string ToString()
        {
            return string.Format(
                "[Packet:{0}\n  timestamp={1}\n  payload size={2}" +
                "\n  SFXName={3}",
                this.Type, new DateTime(Timestamp), Payload.Length, SFXName);
        }
    }
}
