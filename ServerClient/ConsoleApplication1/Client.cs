using System;
using ServerData;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ClientNSP
{
    public class Client
    {
        static Random rnd = new Random();
        public  Socket master;
        public  string name = Environment.MachineName + (rnd.Next(0, 221)) + "";
        public  bool connecting = true;
        public  bool running = false;

        public static int clientNumber;
        public static bool connected = false;

        static void Main()
        {
            Client c1 = new Client();
            c1.init();
        }

        public  void init()
        {
            new Thread(run).Start();
        }

         void run()
        {
            while (connecting)
            {
                if (connecting)
                {
                A:
                    //Console.Clear();
                    //Console.WriteLine("enter ip adress:  ");
                    string ip = "192.168.1.100";
                    //Console.WriteLine(ip);

                    master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), 1337);

                    try
                    {
                        Console.Clear();
                        Console.WriteLine("connecting");
                        master.Connect(ipe);
                        Console.WriteLine("connected");
                        Thread t = new Thread(data_IN);
                        t.Start();
                        Console.WriteLine("thread started");
                        connecting = false;
                        running = true;
                        
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("could not connect to host");
                        Console.WriteLine("trying again");
                        Thread.Sleep(700);
                        goto A;
                    }
                }
            }
            Console.WriteLine("while closed");

            while (true)
            {
                sentMessage("message1");
                Thread.Sleep(500);
                sentMessage("message2");
                Thread.Sleep(200);
                sentMessage("message3");
                Thread.Sleep(333);
                sentMessage("message4");
                Thread.Sleep(250);
                sentMessage("message5");
                Thread.Sleep(1500);
                sentMessage("message6");
                Thread.Sleep(673);
                sentMessage("message7");
            }
        }

        void sendplayerPos(int x, int y)
        {
            //Packet p = new Packet(PacketType.PlayerPos, ID);
            //p.Gdata.Add(ID);
            //p.Gdata.Add(x + "");
            //p.Gdata.Add(y + "");
            //master.Send(p.toByte());
        }

        void sendBombPos(int x, int y)
        {
            // Packet p = new Packet(PacketType.BombPos, ID);
            //p.Gdata.Add(ID);
            //p.Gdata.Add(x + "");
            //p.Gdata.Add(y + "");
            //master.Send(p.toByte());
        }
         void sentMessage(string m)
        {
            Packet p = new Packet(PacketType.Message, "lala");
            p.Gdata.Add(m);
            master.Send(p.toByte());
        }

         void data_IN()
        {
            byte[] Buffer;
            int readBytes;

            while (running)
            {
                try
                {
                    Buffer = new byte[master.SendBufferSize];
                    readBytes = master.Receive(Buffer);

                    if (readBytes > 0)
                    {
                        dataManager(new Packet(Buffer));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    running = false;
                    Thread.Sleep(10);
                    connecting = true;
                }
            }
        }

        void dataManager(Packet p)
        {
            // switch case with packet switch
            switch(p.PacketType)
            {
                case PacketType.ServerAck:
                    Console.WriteLine("Server response:  " + p.Gdata[0]);
                break;
            }
        }

        public void writeToFile(List<string> context)
        {
        }

        public void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public T DeSerObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
                Console.WriteLine(ex);
            }

            return objectOut;
        }
    }
}