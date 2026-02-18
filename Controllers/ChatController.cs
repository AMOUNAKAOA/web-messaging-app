using MessageApp.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace MessageApp.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("messages")]
        public async Task<ActionResult> GetMessages()
        {
            var messages = await _chatService.GetAllMessagesAsync();
            return Ok(messages);
        }

        [HttpGet("users")]
        public async Task<ActionResult> GetUsers()
        {
            var users = await _chatService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("user-count")]
        public async Task<ActionResult> GetUserCount()
        {
            var count = await _chatService.GetUserCountAsync();
            return Ok(new { count });
        }
    }
}