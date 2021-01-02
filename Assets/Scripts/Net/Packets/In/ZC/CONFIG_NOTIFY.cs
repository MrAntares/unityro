﻿public partial class ZC {

    [PacketHandler(HEADER, "ZC_CONFIG_NOTIFY", SIZE)]
    public class CONFIG_NOTIFY : InPacket {

        public const PacketHeader HEADER = PacketHeader.ZC_CONFIG_NOTIFY;
        public const int SIZE = 3;

        public bool Read(BinaryReader br) {
            return true;
        }
    }
}