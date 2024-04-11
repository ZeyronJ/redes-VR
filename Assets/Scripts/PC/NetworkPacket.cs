using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace NSU
{
    public class NetworkPacket
    {
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public byte[] Data { get; set; }

        // +
    }

    public class IcmpPacket
    {
        public byte Type;
        public byte Code;
        public ushort Checksum;
        public ushort Identifier;
        public ushort SequenceNumber;

        public static byte[] Serialize(IcmpPacket packet)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    writer.Write(packet.Type);
                    writer.Write(packet.Code);
                    writer.Write(packet.Checksum);
                    writer.Write(packet.Identifier);
                    writer.Write(packet.SequenceNumber);
                    // se puede poner mas campos segun sea necesario

                    // calculo dle checksum
                    ms.Position = 0;
                    ushort checksum = CalculateChecksum(ms.ToArray());
                    packet.Checksum = checksum;
                    ms.Position = 2;
                    writer.Write(checksum);

                    return ms.ToArray();
                }
            }
        }

        private static ushort CalculateChecksum(byte[] data)
        {
            long sum = 0;

            for (int i = 0; i < data.Length; i += 2)
            {
                sum += (ushort)((data[i] << 8) | data[i + 1]);
            }

            while ((sum >> 16) != 0)
            {
                sum = (sum & 0xFFFF) + (sum >> 16);
            }

            return (ushort)~sum;
        }
    }
}