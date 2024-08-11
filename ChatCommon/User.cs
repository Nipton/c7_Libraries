using System.Net;
using System.Text.Json.Serialization;

namespace ChatCommon
{
    public class User
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = string.Empty;
        public User(string name) 
        {
            Name = name;
        }
    }
}
