using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace MyFirstSocket
{
    public static class SocketHelper
    {
        public static List<Socket> connections = new List<Socket>();

        private static EndPoint MyEndPoint()
        {
            IPAddress address = IPAddress.Parse("172.20.201.17");//Kan även smälla vid parse.
            IPEndPoint endPoint = new IPEndPoint(address, 8080);
            return endPoint;
        }

        public static Socket MySocket()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Bind(MyEndPoint());//Om felaktig IpAdress skrivs in eller hämtas fel får man en socket exception
                return socket;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        
        public static void BroadCast(Socket current, byte[] message)
        {
            foreach (Socket client in connections)
            {
                if (client != current)
                {
                    client.Send(message);
                }
            }
        }

        private static EndPoint SelectAllEndpoints()
        {
            IPAddress address = Dns.GetHostAddresses(Dns.GetHostName())
                .First(a => a.AddressFamily == AddressFamily.InterNetwork);
            IPEndPoint endPoint = new IPEndPoint(address, 8080);
            return endPoint;
        }
    }
}
