using ChatNetwork;
using NetMQ;
using System.Net;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.Write("Введите имя: ");
            string name = Console.ReadLine()!;
            Console.Write("Введите порт: ");
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            int port = int.Parse(Console.ReadLine()!);
            NetMQFrame netMQFrame = null;
            IMessageSource<NetMQFrame> messageSource = new NetMQMessageSourceClient(port);
            Client<NetMQFrame> client = new Client<NetMQFrame>(name, messageSource, netMQFrame);
            _ = Task.Run(client.ClientReceveAsync);
            await client.ClientSendAsync();
        }
    }
}
