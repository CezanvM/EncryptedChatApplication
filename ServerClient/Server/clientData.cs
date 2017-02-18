
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerNSP;
using System;
using ServerData;

namespace Server
{
    class clientData
    {
        public Socket ClientSocket;
        public Thread ClientThread;
        public string id;

        public clientData(Socket clientSocket)
        {
            this.ClientSocket = clientSocket;
            id = Guid.NewGuid().ToString();
            ClientThread = new Thread(Server1.Data_IN);
            ClientThread.Start(ClientSocket);
            SendregistartionPacket();
        }

        public void SendregistartionPacket()
        {
            Packet p = new Packet(PacketType.ServerAck, "server");
            p.Gdata.Add(id);
            ClientSocket.Send(p.toByte());
        }

        public void Disconect()
        {
            ClientThread.Abort();
        }
    }
}
