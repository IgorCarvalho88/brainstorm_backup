using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message, string groupName)
        {
            // Call the addNewMessageToPage method to update clients.
            //Clients.All.addNewMessageToPage(name, message, groupName);
            var data = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            Clients.Group(groupName).addNewMessageToPage(name, message, groupName, data);

        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
            Clients.Group(groupName).addChatMessage(Context.User.Identity.Name + " joined.");
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }
    }

    //public class ContosoChatHub : Hub
    //{
    //    public async Task JoinGroup(string groupName)
    //    {
    //        await Groups.Add(Context.ConnectionId, groupName);
    //        Clients.Group(groupName).addChatMessage(Context.User.Identity.Name + " joined.");
    //    }

    //    public Task LeaveGroup(string groupName)
    //    {
    //        return Groups.Remove(Context.ConnectionId, groupName);
    //    }
    //}
}