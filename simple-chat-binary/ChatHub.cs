using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using MsgPack.Serialization;

namespace signalr_aspnetcore_binary
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync() => await Clients.All.InvokeAsync("Send", $"{Context.ConnectionId} joined");

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            await Clients.All.InvokeAsync("Send", $"{Context.ConnectionId} left");
        }

        public Task Send(string message) => Clients.All.InvokeAsync("Send", $"{Context.ConnectionId}: {message}");

        public Task SendToSomeone(string receiverId, string message) => Clients.Client(receiverId).InvokeAsync("Send", $"{Context.ConnectionId}@{receiverId}: {message}");

        public Task SendToGroup(string groupName, string message) => Clients.Group(groupName).InvokeAsync("Send", $"{Context.ConnectionId}@{groupName}: {message}");

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).InvokeAsync("Send", $"{Context.ConnectionId} joined {groupName}");
        }

        public async Task LeaveGroup(string groupName)
        {
            await Groups.RemoveAsync(Context.ConnectionId, groupName);

            await Clients.Group(groupName).InvokeAsync("Send", $"{Context.ConnectionId} left {groupName}");
        }

        public Task Echo(string message)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync("Send", $"{Context.ConnectionId}: {message}");
        }
    }
}