using ChatCommon;
using ChatDb;
using ChatNetwork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    internal class Server<T>
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();        
        IMessageSource<T> messageSource;
        IRepository repository;
        Dictionary<string, T> clients;
        public Server(IMessageSource<T> messageSource)
        {
            this.messageSource = messageSource;
            clients = new Dictionary<string, T>();
            repository = new Repository();
        }
        public Task Start()
        {
            return Task.Run(async () => await RunServerAsync(cancellationTokenSource.Token), cancellationTokenSource.Token);
        }
        protected async Task RunServerAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Сервер запущен.");
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var (message, remoteEndPoint) = await messageSource.ReceiveAsync();
                    if (message != null)
                    {
                        await HandleMessageAsync(message, remoteEndPoint);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        protected async Task HandleMessageAsync(Message message, T remoteEndPoint)
        {
            Console.WriteLine(message);
            if (message.Command == Command.Login)
            {
                await LoginAsync(message.SenderName, remoteEndPoint);
            }
            else if (message.Command == Command.Message)
            {
                T newiPEndPoint;
                bool reuslt =  await repository.SendMessageAsync(message); // можно потом добавить ответ сервера, если пользователь не найден.
                if (reuslt && clients.TryGetValue(message.RecipientName!, out newiPEndPoint!))
                {
                    await messageSource.SendAsync(message, newiPEndPoint);
                }
            }
            else if (message.Command == Command.Confirmation)
            {
                await repository.ConfirmMessageReceiptAsync(message);
            }
        }
        protected async Task LoginAsync(string senderName, T iPEndPoint)
        {
            User user = new User(senderName);
            clients[user.Name] = iPEndPoint;
            bool result = await repository.CreateUser(senderName);
            if (!result)
                await repository.CheckUnreadMessagesAsync(user.Name);
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
            Console.WriteLine("Сервер остановлен.");
        }
    }
}
