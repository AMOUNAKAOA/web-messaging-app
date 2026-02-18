using MessageApp.Backend.Data;
using MessageApp.Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageApp.Backend.Services
{
    public class ChatService
    {
        private readonly ChatDbContext _context;

        public ChatService(ChatDbContext context)
        {
            _context = context;
        }

        // Get all messages
        public async Task<List<Message>> GetAllMessagesAsync()
        {
            return await _context.Messages
                .OrderBy(m => m.Timestamp)
                .ToListAsync();
        }

        // Add a new message
        public async Task<Message> AddMessageAsync(string content, string username)
        {
            var message = new Message
            {
                Content = content,
                Username = username,
                Timestamp = DateTime.UtcNow
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }

        // Register or get user
        public async Task<User> RegisterUserAsync(string username)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                user = new User { Username = username };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            return user;
        }

        // Get user count
        public async Task<int> GetUserCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        // Get all users
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}