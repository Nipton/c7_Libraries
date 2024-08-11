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
    public class NetMQMessageSourceServer : IMessageSource
    {
        ResponseSocket socket;
        public NetMQMessageSourceServer(int port)
        {
            socket = new ResponseSocket();
            socket.Bind($"tcp://127.0.0.1:{port}");
        }
        public async Task<(Message?, IPEndPoint)> ReceiveAsync()
        {
            var data = socket.ReceiveFrameString();
            Message? message = Message.FromJson(data);
            IPEndPoint iPEndPoint = null;
            var tuple = (message, iPEndPoint);
            return tuple;
        }

        public async Task SendAsync(Message message, IPEndPoint remoteEndPoint)
        {
            socket.Connect(remoteEndPoint.ToString());
            var data = message.ToJson();
            socket.SendFrame(data);
        }
    }
}
