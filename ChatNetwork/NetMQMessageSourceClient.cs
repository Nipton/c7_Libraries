using ChatCommon;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatNetwork
{
    public class NetMQMessageSourceClient : IMessageSource
    {
        RequestSocket socket;
       
         public NetMQMessageSourceClient(int port)
        {
            socket = new RequestSocket();
            socket.Connect($"tcp://127.0.0.1:{port}");
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
            var data = message.ToJson();
            socket.SendFrame(data);
        }
    }
}
