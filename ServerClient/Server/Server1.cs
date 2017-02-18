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
        static List<Packet> PlayerPosPacketList = new List<Packet>();
        static List<Packet> BombPosPacketList = new List<Packet>();

        public delegate void ChangedEventHandler();

        private static event ChangedEventHandler eventHandler;

        static void Main(String[] args)
        {
            Console.WriteLine("starting server on " + Packet.getIp4Adress());
            listernerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clients = new List<Server.clientData>();


            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Packet.getIp4Adress()), 1337);

            listernerSocket.Bind(ip);

            Thread listenThread = new Thread(ListenThread);
            listenThread.Start();

        } // main thread




        static void ListenThread()
        {
            while (listening)
            {
                listernerSocket.Listen(10);
                clients.Add(new clientData(listernerSocket.Accept()));
                eventHandler.Invoke();
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

            Socket clientSocket = (Socket) cSocket;

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
                    checkConnections();
                    //Console.WriteLine(e.Message);
                }
                    


                if (readBytes > 0)
                    {

                        Packet packet = new Packet(Buffer);
                        DataManger(packet);

                    
                }
            }
        }

        //client data thhread recieves data from clients 



        static public void DataManger(Packet p)
        {
            switch (p.PacketType)
            {
                case PacketType.Login:
                    //check account from database/ json;
                    break;

                case PacketType.Message:
                   
                    break;

                case PacketType.Registration:
                    // write to json;
                    break;
            }

        }


        public static void checkConnections()
        {
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].ClientSocket.Connected == false)
                {
                    Console.WriteLine("client disconected" + clients[i].id);
                    clients.Remove(clients[i]);
                    
                }
            }
        }

        // data manager 
       


    }
 
}