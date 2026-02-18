// SignalR connection
let connection = null;
let currentUsername = null;
let messageCount = 0;

// Initialize connection when page loads
document.addEventListener('DOMContentLoaded', function () {
    initializeSignalR();
});

function initializeSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5000/chatHub", {
            withCredentials: false
        })
        .withAutomaticReconnect()
        .build();

    // Connection event handlers
    connection.on("ReceiveMessage", (data) => {
        displayMessage(data.username, data.content, data.timestamp, currentUsername === data.username);
    });

    connection.on("UserJoined", (username) => {
        if (username !== currentUsername) {
            displaySystemMessage(`${username} joined the chat`);
        }
    });

    connection.on("UserCountUpdated", (count) => {
        document.getElementById('userCount').textContent = count;
    });

    connection.on("ChatHistory", (messages) => {
        const messagesList = document.getElementById('messagesList');
        messagesList.innerHTML = '';
        messageCount = 0;
        
        messages.forEach(msg => {
            displayMessage(msg.username, msg.content, msg.timestamp, currentUsername === msg.username);
        });
    });

    connection.on("Error", (error) => {
        showError(error);
    });

    connection.on("UserListUpdated", (users) => {
        updateUserList(users);
    });

    connection.onreconnected((connectionId) => {
        console.log("Reconnected to server");
        showNotification("Reconnected to server");
    });

    connection.onclose((error) => {
        if (error) {
            console.error('Connection closed with error:', error);
            showError("Connection lost. Please refresh the page.");
        }
    });

    // Start connection
    connection.start()
        .catch(err => {
            console.error("Failed to connect:", err);
            showError("Failed to connect to server. Make sure the server is running on http://localhost:5000");
        });
}

function joinChat() {
    const usernameInput = document.getElementById('usernameInput').value.trim();
    const loginError = document.getElementById('loginError');

    // Validation
    if (!usernameInput) {
        loginError.textContent = "Username cannot be empty";
        return;
    }

    if (usernameInput.length > 50) {
        loginError.textContent = "Username must be less than 50 characters";
        return;
    }

    if (!/^[a-zA-Z0-9_-]+$/.test(usernameInput)) {
        loginError.textContent = "Username can only contain letters, numbers, underscore, and hyphen";
        return;
    }

    currentUsername = usernameInput;
    loginError.textContent = "";

    // Hide login section, show chat section
    document.getElementById('loginSection').classList.add('hidden');
    document.getElementById('chatSection').classList.remove('hidden');
    document.getElementById('currentUser').textContent = `You: ${currentUsername}`;

    // Clear input
    document.getElementById('usernameInput').value = '';

    // Send join message via SignalR
    if (connection && connection.state === signalR.HubConnectionState.Connected) {
        connection.invoke("JoinChat", currentUsername)
            .catch(err => console.error("Error joining chat:", err));

        // Request chat history
        connection.invoke("GetChatHistory")
            .catch(err => console.error("Error getting chat history:", err));
    }

    // Focus on message input
    document.getElementById('messageInput').focus();
}

function sendMessage() {
    const messageInput = document.getElementById('messageInput');
    const message = messageInput.value.trim();

    if (!message) {
        return;
    }

    if (!currentUsername) {
        showError("You must join the chat first");
        return;
    }

    if (connection && connection.state === signalR.HubConnectionState.Connected) {
        connection.invoke("SendMessage", message)
            .catch(err => {
                console.error("Error sending message:", err);
                showError("Failed to send message");
            });
    } else {
        showError("Connection is not established");
    }

    messageInput.value = '';
    messageInput.focus();
}

function displayMessage(username, content, timestamp, isOwn = false) {
    const messagesList = document.getElementById('messagesList');
    messageCount++;

    const messageDiv = document.createElement('div');
    messageDiv.className = `message ${isOwn ? 'own' : ''}`;
    messageDiv.innerHTML = `
        <div class="message-bubble">
            <div class="message-header">${escapeHtml(username)}</div>
            <div class="message-content">${escapeHtml(content)}</div>
            <div class="message-time">${timestamp}</div>
        </div>
    `;

    messagesList.appendChild(messageDiv);
    messagesList.scrollTop = messagesList.scrollHeight;

    // Limit messages displayed (keep last 100)
    if (messageCount > 100) {
        messagesList.removeChild(messagesList.firstChild);
        messageCount--;
    }
}

function displaySystemMessage(message) {
    const messagesList = document.getElementById('messagesList');
    const systemDiv = document.createElement('div');
    systemDiv.className = 'system-message';
    systemDiv.textContent = message;

    messagesList.appendChild(systemDiv);
    messagesList.scrollTop = messagesList.scrollHeight;
}

function updateUserList(users) {
    const userListItems = document.getElementById('userListItems');
    userListItems.innerHTML = '';

    users.forEach(user => {
        const li = document.createElement('li');
        li.textContent = user;
        if (user === currentUsername) {
            li.textContent += ' (you)';
            li.style.fontWeight = 'bold';
        }
        userListItems.appendChild(li);
    });
}

function leaveChat() {
    currentUsername = null;
    messageCount = 0;

    // Show login section, hide chat section
    document.getElementById('loginSection').classList.remove('hidden');
    document.getElementById('chatSection').classList.add('hidden');
    document.getElementById('currentUser').textContent = '';
    document.getElementById('messagesList').innerHTML = '';
    document.getElementById('usernameInput').value = '';
    document.getElementById('usernameInput').focus();

    // Disconnect from server
    if (connection) {
        connection.stop()
            .then(() => {
                console.log("Disconnected from server");
                // Reconnect for next join
                initializeSignalR();
            })
            .catch(err => console.error("Error disconnecting:", err));
    }
}

function showError(message) {
    const errorAlert = document.getElementById('errorAlert');
    errorAlert.textContent = message;
    errorAlert.classList.remove('hidden');

    setTimeout(() => {
        errorAlert.classList.add('hidden');
    }, 5000);
}

function showNotification(message) {
    console.log(message);
}

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}