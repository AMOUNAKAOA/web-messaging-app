using MessageApp.Backend.Models;
using MessageApp.Backend.Services;
using Microsoft.AspNetCore.SignalR;

namespace MessageApp.Backend.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatService;
        private static HashSet<string> _connectedUsers = new HashSet<string>();

        public ChatHub(ChatService chatService)
        {
            _chatService = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.All.SendAsync("UserCountUpdated", _connectedUsers.Count);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var username = Context.User?.Identity?.Name ?? 
                          Context.Items["username"]?.ToString();
            
            if (!string.IsNullOrEmpty(username))
            {
                _connectedUsers.Remove(username);
            }

            await Clients.All.SendAsync("UserCountUpdated", _connectedUsers.Count);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task JoinChat(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                await Clients.Caller.SendAsync("Error", "Username cannot be empty");
                return;
            }

            Context.Items["username"] = username;
            _connectedUsers.Add(username);

            // Register user in database
            await _chatService.RegisterUserAsync(username);

            // Notify all clients
            await Clients.All.SendAsync("UserJoined", username);
            await Clients.All.SendAsync("UserCountUpdated", _connectedUsers.Count);
            await Clients.Caller.SendAsync("UserListUpdated", _connectedUsers.ToList());
        }

        public async Task SendMessage(string message)
        {
            var username = Context.Items["username"]?.ToString();

            if (string.IsNullOrEmpty(username))
            {
                await Clients.Caller.SendAsync("Error", "You must join the chat first");
                return;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                await Clients.Caller.SendAsync("Error", "Message cannot be empty");
                return;
            }

            // Save to database
            var savedMessage = await _chatService.AddMessageAsync(message, username);

            // Broadcast to all clients
            await Clients.All.SendAsync("ReceiveMessage", new
            {
                username = savedMessage.Username,
                content = savedMessage.Content,
                timestamp = savedMessage.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

        public async Task GetChatHistory()
        {
            var messages = await _chatService.GetAllMessagesAsync();
            var chatHistory = messages.Select(m => new
            {
                username = m.Username,
                content = m.Content,
                timestamp = m.Timestamp.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            await Clients.Caller.SendAsync("ChatHistory", chatHistory);
        }
    }
}