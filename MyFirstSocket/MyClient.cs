using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstSocket
{
    public class MyClient
    {
        Task task;
        Socket client;
        private string endMessage = "Good bye";
        public MyClient(Socket client)
        {
            this.client = client;
            task = new Task(() => Listen());
            task.Start();
        }
        public void Listen()
        {
            while (true)
            {
                byte[] bytes = new byte[2048];

                int recBytes = client.Receive(bytes); //Om det inte finns något stannar den här.
               
                if (recBytes == 0)//Detta menas att socketen på andra sidan är stängd.
                {
                    break;
                }
                
                string response = Encoding.UTF8.GetString(bytes, 0, recBytes);
                if (response == "e") //Valt string värde avslutar loopen och sedan avslutar connection.
                {                   
                    break;
                }

                var message = bytes.Take(recBytes).ToArray();
                
                SocketHelper.BroadCast(client, message);

                //Console.WriteLine("Skickar tid");
                //var tid = DateTime.Now.ToShortDateString();
                //var array = Encoding.ASCII.GetBytes(tid);

                Console.Write(response); //Skickar svaret som man skrev
                //client.Send(array);//Skickar datumet för idag.
            }
            Console.Write(endMessage);
            client.Send(Encoding.UTF8.GetBytes(endMessage));
            client.Close();//Stänger client
            SocketHelper.connections.Remove(client);//Ta bort client från connection list när man försvinner.
        }
    }
}
