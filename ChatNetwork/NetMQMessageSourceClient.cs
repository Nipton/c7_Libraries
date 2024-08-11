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
    public class NetMQMessageSourceClient : IMessageSource<NetMQFrame>
    {
        DealerSocket socket;
       
         public NetMQMessageSourceClient(int port)
        {
            socket = new DealerSocket();
            socket.Connect($"tcp://127.0.0.1:{port}");
        }

        public async Task<(Message?, NetMQFrame)> ReceiveAsync()
        {
            NetMQFrame netMQ = null;
            var data = socket.ReceiveFrameString();
            Message? message = Message.FromJson(data);
            //IPEndPoint iPEndPoint = null;
            var tuple = (message, netMQ);
            return tuple;
        }

        public async Task SendAsync(Message message, NetMQFrame remoteEndPoint)
        {
            var data = message.ToJson();
            socket.SendFrame(data);
        }
    }
}
