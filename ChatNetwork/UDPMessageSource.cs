using ChatCommon;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatNetwork
{
    public class UDPMessageSource : IMessageSource<IPEndPoint>
    {
        readonly UdpClient udpClient;
        public UDPMessageSource(int port)
        {
            udpClient = new UdpClient(port);
        }
        public async Task<(Message?, IPEndPoint)> ReceiveAsync()
        {
            var buffer = await udpClient.ReceiveAsync();
            var data = Encoding.UTF8.GetString(buffer.Buffer);
            var tuple = (Message.FromJson(data), buffer.RemoteEndPoint);
            return tuple;
        }
        public async Task SendAsync(Message message, IPEndPoint remoteEndPoint)
        {
            var data = Encoding.UTF8.GetBytes(message.ToJson());
            await udpClient.SendAsync(data, remoteEndPoint);
        }
    }
}
