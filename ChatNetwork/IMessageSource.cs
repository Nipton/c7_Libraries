using ChatCommon;
using System.Net;

namespace ChatNetwork
{
    public interface IMessageSource<T>
    {
        Task SendAsync(Message message, T remoteEndPoint);
        Task<(Message?, T)> ReceiveAsync();
    }
}
