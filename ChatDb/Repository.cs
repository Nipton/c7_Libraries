using ChatCommon;
using ChatDb.EntityModels;
using ChatDb.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatDb
{
    public class Repository : IRepository
    {
        public async Task<bool> CreateUser(string name)
        {
            using (ChatContext chatContext = new ChatContext())
            {
                try
                {
                    if (!chatContext.Users.Any(x => x.Name == name))
                    {
                        UserEntity user = new UserEntity() { Name = name };
                        await chatContext.Users.AddAsync(user);
                        await chatContext.SaveChangesAsync();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    
                }
                return false;
            }
        }
        public async Task ConfirmMessageReceiptAsync(Message message)
        {
            using (ChatContext chatContext = new ChatContext())
            {
                try
                {
                    var msg = chatContext.Messages.FirstOrDefault(x => x.Id == message.Id);
                    if (msg != null)
                    {
                        msg.ReceivedStatus = true;
                        await chatContext.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        public async Task CheckUnreadMessagesAsync(string name)
        {
            using (ChatContext chatContext = new ChatContext())
            {
                try
                {
                    UserEntity? user = await chatContext.Users.SingleOrDefaultAsync(u => u.Name == name);
                    if (user == null)
                        return;
                    var unreadMessages = await chatContext.Messages.Where(m => m.RecipientId == user.Id && !m.ReceivedStatus).ToListAsync();

                    foreach (var message in unreadMessages)
                    {
                        Message mes = new Message() { Command = Command.Message, Id = message.Id, ReceivedStatus = message.ReceivedStatus, Text = message.Text, RecipientName = message.Recipient?.Name, SenderName = message.Sender?.Name!, TimeMessage = message.TimeMessage };
                        await SendMessageAsync(mes);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        public async Task<bool> SendMessageAsync(Message message)
        {
            using (ChatContext chatContext = new ChatContext())
            {
                bool statusSend = false;
                if (message.RecipientName == null || message.SenderName == null)
                    return statusSend;
                var toUser = chatContext.Users.FirstOrDefault(x => x.Name == message.RecipientName);
                var fromUser = chatContext.Users.FirstOrDefault(x => x.Name == message.SenderName);
                if (toUser != null && fromUser != null)
                {
                    MessageEntity newMessage; 
                    try
                    {
                        if (chatContext.Messages.Any(x => x.Id == message.Id))
                        {
                            newMessage = chatContext.Messages.Find(message.Id)!;
                        }
                        else
                        {
                            newMessage = new MessageEntity { Id = message.Id, SenderId = fromUser.Id, RecipientId = toUser.Id, Text = message.Text, TimeMessage = message.TimeMessage };
                            chatContext.Messages.Add(newMessage);
                        }
                        await chatContext.SaveChangesAsync();
                        message.Id = newMessage.Id;
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    return true;
                }
                else
                    return statusSend;
            }
        }
    }
}
