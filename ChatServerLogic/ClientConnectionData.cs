using ChatMessages;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatServerLogic
{
    class ClientConnectionData
    {
        public TcpClient Connection;
        public string UserName;
        public ChatRoom ConnectedRoom;
        public Task Reader;
    }
}
