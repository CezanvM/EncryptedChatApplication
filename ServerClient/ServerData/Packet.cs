using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;

namespace ServerData
{
    [Serializable]
    public class Packet
    {
        public List<String> Gdata;
        public int packetInt;
        public bool packetBool;
        public String senderID;
        public PacketType PacketType;


        public Packet(PacketType type, String senderID)
        {
            Gdata = new List<string>();
            this.senderID = senderID;
            this.PacketType = type;
        }

        public Packet(byte[] packetBytes)
        {

            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream(packetBytes);

            try
            {
                Packet p = (Packet)bf.Deserialize(ms);
                ms.Close();
                this.Gdata = p.Gdata;
                this.packetInt = p.packetInt;
                this.packetBool = p.packetBool;
                this.senderID = p.senderID;
                this.PacketType = p.PacketType;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }



        public byte[] toByte()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, this);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }



        public static string getIp4Adress()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress ip in ips)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }
    }

    public enum PacketType
    {
        Login,
        Registration,
        Message,
        ServerAck

    }
}
