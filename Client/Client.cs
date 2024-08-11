using ChatCommon;
using ChatNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Client<T>
    {
        //readonly IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
        readonly IMessageSource<T> messageSource;
        string name;
        T t;
        User fromUser;
        public Client(string name, IMessageSource<T> messageSource, T t)
        {
            this.messageSource = messageSource;
            this.name = name;
            fromUser = new User(name);
            this.t = t;
        }
        public async Task ClientReceveAsync()
        {
            await Register();
            try
            {
                while (true)
                {
                    var (answer, endpoint) = await messageSource.ReceiveAsync();
                    if (answer != null)
                    {
                        Console.WriteLine(answer);
                        await ConfirmAsync(answer);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task ConfirmAsync(Message message)
        {
            message.Command = Command.Confirmation;
            await SendMessage(message);
        }
        private async Task Register()
        {
            Message message = new Message();
            message.SenderName = name;
            await SendMessage(message);
        }
        public async Task ClientSendAsync()
        {
            while (true)
            {
                Message message = new Message();
                Console.WriteLine("Введите имя получателя: ");
                string toName = Console.ReadLine()!;
                message.RecipientName = toName;
                Console.WriteLine("Введите сообщение: ");
                message.SenderName = name;
                message.Text = Console.ReadLine()!;
                message.TimeMessage = DateTime.Now;
                message.Command = Command.Message;
                await SendMessage(message);
            }
        }
        private async Task SendMessage(Message message)
        {
            try
            {
                await messageSource.SendAsync(message, t);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
