using ChatCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChatDb
{
    public interface IRepository
    {
        Task<bool> CreateUser(string name);
        Task ConfirmMessageReceiptAsync(Message message);
        Task<bool> SendMessageAsync(Message message);
        Task CheckUnreadMessagesAsync(string name);
    }
}
