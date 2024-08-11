using ChatCommon;
using System.Net;

namespace ChatNetwork
{
    public interface IMessageSource
    {
        Task SendAsync(Message message, IPEndPoint remoteEndPoint);
        Task<(Message?, IPEndPoint)> ReceiveAsync();

    }
}
