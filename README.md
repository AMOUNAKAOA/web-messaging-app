# Web Messaging App 💬

A complete, production-ready web-based messaging application built with C# ASP.NET Core and modern web technologies. Features real-time chat in a big lobby with multiple users using WebSocket communication.

## Features ✨

- ✅ **Real-time messaging** with WebSockets (SignalR)
- ✅ **User authentication** (simple username-based)
- ✅ **Message persistence** with SQLite database
- ✅ **Chat history** - Previous messages loaded on connect
- ✅ **User count tracking** - See how many users are online
- ✅ **System notifications** - Get notified when users join
- ✅ **Responsive design** - Works perfectly on mobile and desktop
- ✅ **Error handling** - Comprehensive error messages
- ✅ **Auto-reconnection** - Automatic recovery on connection loss
- ✅ **Input validation** - Server and client-side validation
- ✅ **XSS protection** - HTML escaping for security

## Technology Stack 🛠️

### Backend
- **Framework**: ASP.NET Core 8
- **Real-time Communication**: SignalR
- **Database**: Entity Framework Core with SQLite
- **Architecture**: RESTful API + WebSocket Hub

### Frontend
- **HTML5** - Semantic markup
- **CSS3** - Modern styling with gradients and animations
- **Vanilla JavaScript** - No dependencies
- **SignalR JavaScript Client** - WebSocket communication

## Project Structure 📁

```
web-messaging-app/
├── Program.cs                    # Application entry point
├── appsettings.json             # Configuration
├── MessageApp.Backend.csproj     # Project file
├── Models/
│   ├── Message.cs               # Message entity
│   └── User.cs                  # User entity
├── Data/
│   └── ChatDbContext.cs          # Database context
├── Services/
│   └── ChatService.cs            # Business logic
├── Controllers/
│   └── ChatController.cs         # REST API endpoints
├── Hubs/
│   └── ChatHub.cs                # SignalR hub for real-time chat
├── Frontend/
│   ├── index.html                # Main HTML file
│   ├── css/
│   │   └── styles.css            # Styling
│   └── js/
│       └── app.js                # Client-side logic
└── README.md                     # This file
```

## Getting Started ��

### Prerequisites
- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js** (optional, for serving frontend)
- **Any modern web browser**

### Backend Setup

1. **Navigate to the backend directory**
   ```bash
   cd web-messaging-app
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```
   The server will start on `http://localhost:5000`

### Frontend Setup

1. **Serve the frontend files**
   
   **Option A - Using Python:**
   ```bash
   cd Frontend
   python -m http.server 3000
   ```
   
   **Option B - Using Node.js (http-server):**
   ```bash
   cd Frontend
   npx http-server -p 3000
   ```
   
   **Option C - Using VS Code Live Server:**
   - Install the "Live Server" extension
   - Right-click on `index.html` and select "Open with Live Server"

2. **Open your browser**
   - Navigate to `http://localhost:3000` (or the port shown by your server)

## Usage 💻

1. **Enter a username** and click "Join Chat"
2. **Type your message** in the input field
3. **Press Enter or click Send** to broadcast to all users
4. **See real-time updates** as other users join and send messages
5. **Click Leave** to exit the chat

## API Endpoints 🔌

### REST API
- `GET /api/chat/messages` - Get all messages
- `GET /api/chat/users` - Get all registered users
- `GET /api/chat/user-count` - Get current user count

### SignalR Events

**Server-to-Client:**
- `ReceiveMessage(username, content, timestamp)` - New message received
- `UserJoined(username)` - User joined the chat
- `UserCountUpdated(count)` - User count changed
- `ChatHistory(messages)` - Message history loaded
- `UserListUpdated(users)` - User list updated
- `Error(message)` - Error notification

**Client-to-Server:**
- `JoinChat(username)` - Join the chat lobby
- `SendMessage(message)` - Send a message
- `GetChatHistory()` - Request chat history

## Configuration ⚙️

### Update Backend URL
Edit `Frontend/js/app.js` and update the server URL:
```javascript
.withUrl("http://your-server:5000/chatHub")
```

### Update CORS Policy
Edit `Program.cs` to allow your frontend origin:
```csharp
policy.WithOrigins("http://localhost:3000")  // Your frontend URL
```

### Database
- SQLite database is automatically created in the backend directory
- File: `messaging_app.db`

## Features Explained 🎯

### Real-time Communication
Uses SignalR WebSockets for instant message delivery. If WebSocket isn't available, it automatically falls back to other transports.

### Message Persistence
All messages are saved to a SQLite database, so chat history is maintained even after server restarts.

### User Tracking
Connected users are tracked in memory, while registered users are stored in the database.

### Chat History
When a user joins, they receive all previous messages (up to 100 last messages) automatically.

## Security Features 🔒

- ✅ HTML escaping to prevent XSS attacks
- ✅ Input validation on both client and server
- ✅ CORS configuration
- ✅ Username constraints (alphanumeric, underscore, hyphen only)
- ✅ Message content validation

## Performance 📊

- Handles multiple concurrent connections
- Efficient database queries with Entity Framework
- Message limit (last 100) to manage memory usage
- Automatic connection recovery

## Troubleshooting 🐛

### "Failed to connect to server"
- Ensure the backend is running on `http://localhost:5000`
- Check that CORS policy includes your frontend URL

### Messages not appearing
- Check browser console for errors (F12)
- Verify SignalR connection status
- Ensure both frontend and backend are running

### Database locked error
- Stop the backend application
- Delete `messaging_app.db` if corrupted
- Restart the backend

## Future Enhancements 🚀

- Private messaging between users
- Message editing and deletion
- User profiles and avatars
- Message reactions (emojis)
- File sharing
- Message search
- Message categories/channels
- User authentication (JWT)
- Admin controls
- Message encryption

## Learning Resources 📚

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [SignalR Documentation](https://docs.microsoft.com/aspnet/core/signalr)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Modern JavaScript](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide)

## License 📄

MIT License - Feel free to use this project for learning and development!

## Contributing 🤝

Contributions are welcome! Feel free to:
- Report bugs
- Suggest features
- Submit pull requests
- Improve documentation

## Contact 📧

For questions or suggestions, reach out on GitHub!

---

**Built with ❤️ using ASP.NET Core and Modern Web Technologies**