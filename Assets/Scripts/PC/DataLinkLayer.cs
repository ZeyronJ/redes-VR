using System;
using System.Text;

namespace NSU
{
    public class DataLinkLayer
    {
        public static byte[] Encapsulate(NetworkPacket packet)
        {
            // Obtener las direcciones de origen y destino
            byte[] sourceAddress = Encoding.UTF8.GetBytes(packet.SourceAddress.PadRight(15)); // Ajusta la longitud a 15 y rellena con espacios si es necesario
            byte[] destinationAddress = Encoding.UTF8.GetBytes(packet.DestinationAddress.PadRight(15)); // Ajusta la longitud a 15 y rellena con espacios si es necesario

            // Combina las direcciones y datos para la trama / frame
            byte[] frame = new byte[sourceAddress.Length + destinationAddress.Length + packet.Data.Length];
            Buffer.BlockCopy(sourceAddress, 0, frame, 0, sourceAddress.Length);
            Buffer.BlockCopy(destinationAddress, 0, frame, sourceAddress.Length, destinationAddress.Length);
            Buffer.BlockCopy(packet.Data, 0, frame, sourceAddress.Length + destinationAddress.Length, packet.Data.Length);

            return frame;
        }

        public static NetworkPacket Decapsulate(byte[] frame)
        {
            // extraer las direcciones de origen y destino
            byte[] sourceAddress = new byte[15];
            byte[] destinationAddress = new byte[15];

            Buffer.BlockCopy(frame, 0, sourceAddress, 0, sourceAddress.Length);
            Buffer.BlockCopy(frame, sourceAddress.Length, destinationAddress, 0, destinationAddress.Length);

            // Eliminar espacios en blanco al final de las direcciones
            string source = Encoding.UTF8.GetString(sourceAddress).Trim();
            string destination = Encoding.UTF8.GetString(destinationAddress).Trim();

            // extraer el paquete de la trama / frame
            int dataStartIndex = sourceAddress.Length + destinationAddress.Length;
            int dataLength = frame.Length - dataStartIndex;

            byte[] packetData = new byte[dataLength];
            Buffer.BlockCopy(frame, dataStartIndex, packetData, 0, dataLength);

            // Construir y devolver el paquete
            return new NetworkPacket
            {
                SourceAddress = source,
                DestinationAddress = destination,
                Data = packetData
            };
        }
    }
}
