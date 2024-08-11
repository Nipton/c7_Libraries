using ChatDb;
using ChatNetwork;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IMessageSource messageSource = new NetMQMessageSourceServer(5000);
            Server server = new Server(messageSource);
            server.Start();
            Console.ReadLine();
            server.Stop();
            Console.ReadKey();
        }
    }
}
