using System;
using System.Collections.Generic;
using System.Net.Sockets;
using ServerData;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.Windows;
using Server;

namespace ServerNSP
{
    public class Server1
    {
        private static List<clientData> clients;
        static Socket listernerSocket;
        private static bool listening = true;
        private static List<Packet> PacketQue = new List<Packet>();

        public delegate void ChangedEventHandler();
        static void Main(String[] args)
        {
            Console.WriteLine("starting server on " + Packet.getIp4Adress());
            listernerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clients = new List<Server.clientData>();


            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Packet.getIp4Adress()), 1337);

            listernerSocket.Bind(ip);

            Thread listenThread = new Thread(ListenThread);
            listenThread.Start();

             Thread ManagerThread = new Thread(DataManger);
             ManagerThread.Start();

        } // main thread




        static void ListenThread()
        {
            while (listening)
            {
                listernerSocket.Listen(10);
                clients.Add(new clientData(listernerSocket.Accept()));
                Console.WriteLine("client connected");
                foreach (var client in clients)
                {
                    Console.WriteLine("connected id is: " + client.id);
                }
            }
        }

        // listerner listends for clients trying trying to connect



        public static void Data_IN(object cSocket)
        {

            Socket clientSocket = (Socket)cSocket;

            byte[] Buffer = new byte[0];
            int readBytes = 0;


            for (;;)
            {
                if (clientSocket.Connected != true)
                {
                    break;
                }

                Buffer = new byte[clientSocket.SendBufferSize];
                try
                {
                    readBytes = clientSocket.Receive(Buffer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    checkConnections();

                }



                if (readBytes > 0)
                {

                    Packet packet = new Packet(Buffer);
                    PacketQue.Add(packet);
                    //DataManger(packet);
                   


                }
            }
        }
 



        private static void DataManger()
        {
            A:
            if (PacketQue.Count > 0)
            {
                for(int i = PacketQue.Count; i > 0; i--)
                {
                    Packet p = PacketQue[i-1];
                    switch (p.PacketType)
                    {
                        case PacketType.Login:
                            //check account from database/ json;
                            break;

                        case PacketType.Message:
                            Console.WriteLine("message is: " + p.Gdata[0] + "from...." + p.senderID);
                            break;

                        case PacketType.Registration:
                            // write to json;
                            break;
                    }
                    PacketQue.RemoveAt(i-1);
                   
                }
                goto A;
            }
            else
            {       
                Thread.Sleep(500);
                goto A;
            }
            
            
        }


        public static void checkConnections()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].ClientSocket.Connected == false)
                {
                    clients[i].Disconect();
                    Console.WriteLine("client disconected" + clients[i].id);
                    clients.Remove(clients[i]);

                }
            }
        }
    }

}