using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSocket
{
    class MySocketServer
    {
        Socket socket = SocketHelper.MySocket();

        public MySocketServer()
        {
            Task task = new Task(() => Listen());
            task.Start();
            ShowServerEndPoint();
            Console.WriteLine("Press enter to close server.");
            Console.ReadLine();
            if (socket != null)
            {
                socket.Close();
            }
        }

        private void Listen()
        {            
            while (true)
            {
                if (socket == null)
                {
                    break;
                }
                try
                {
                    socket.Listen(10);
                    Socket client = socket.Accept();
                    MyClient c = new MyClient(client);
                    SocketHelper.connections.Add(client);
                    Console.Out.WriteLine($"Kontakt med : {client.RemoteEndPoint} etablerad");
                    Console.Out.WriteLine($"Antal klienter online: {SocketHelper.connections.Count}");
                    byte[] toBytes = Encoding.ASCII.GetBytes("Welcome to server, Jespers dator.");
                    client.Send(toBytes);
                }
                catch (SocketException e)
                {
                    Console.WriteLine($"{e.Message}");
                }
                catch (FormatException e)
                {
                    Console.WriteLine($"{e.Message}");
                }                
            }
        }

        private void ShowServerEndPoint()
        {
            Console.WriteLine("Lyssnar på......");
            try
            {
                Console.WriteLine($"Adressen: {((IPEndPoint)socket.LocalEndPoint).Address}");
                Console.Out.WriteLine($"Port: {((IPEndPoint)socket.LocalEndPoint).Port}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); ;
            }
            
            
        }
    }
}
