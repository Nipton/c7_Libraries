using ChatDb.EntityModels;
using ChatDb.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatDb
{
    public class ChatContext : DbContext
    {
        public DbSet<UserEntity> Users {  get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseMySql("server=localhost;user=root;password=password;database=NewChatdb;", new MySqlServerVersion(new Version(8, 0, 36)));
        }
    }
}
