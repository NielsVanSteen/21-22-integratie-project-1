using Microsoft.AspNetCore.SignalR;

namespace UI.MVC.Models.Hub;

public class DocreviewHub : Microsoft.AspNetCore.SignalR.Hub
{
    public Task JoinGroup(string group)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, group);
    }

    public Task LeaveGroup(string group)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
    }
    
}