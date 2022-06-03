using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Domain;



namespace EchoAPI
{
    public class ChatHub : Hub
    {
        public void JoinGroup(string groupName)
        {
            this.Groups.AddToGroupAsync(this.Context.ConnectionId, groupName);
        }






    }
}
