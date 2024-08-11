using ChatDb;
using ChatNetwork;
using NetMQ;
using System.Net;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IMessageSource<NetMQFrame> messageSource = new NetMQMessageSourceServer(5000);
            Server<NetMQFrame> server = new Server<NetMQFrame>(messageSource);
            server.Start();
            Console.ReadLine();
            server.Stop();
            Console.ReadKey();
        }
    }
}
