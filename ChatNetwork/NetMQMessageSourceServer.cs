using ChatCommon;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatNetwork
{
    public class NetMQMessageSourceServer : IMessageSource<NetMQFrame>
    {
        RouterSocket socket;
        public NetMQMessageSourceServer(int port)
        {
            socket = new RouterSocket();
            socket.Bind($"tcp://127.0.0.1:{port}");
        }
        public async Task<(Message?, NetMQFrame)> ReceiveAsync()
        {
            var data = socket.ReceiveMultipartMessage();
            NetMQFrame clientId = data.First;
            Message? message = Message.FromJson(data.Last.ConvertToString());
            var tuple = (message, clientId);
            return tuple;
        }

        public async Task SendAsync(Message message, NetMQFrame remoteEndPoint)
        {
            var data = message.ToJson();
            var responseMessage = new NetMQMessage();
            responseMessage.Append(remoteEndPoint);
            responseMessage.Append(data);
            socket.TrySendMultipartMessage(responseMessage);
        }
    }
}
