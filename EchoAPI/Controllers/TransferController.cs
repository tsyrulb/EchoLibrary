
using Domain;
using Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Services;
using System.Text.Json.Nodes;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EchoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private MessageService _sevice;
        private readonly IHubContext<ChatHub> _myHubContext;
        private readonly INotificationService _notificationService;
        MariaDbContext _context;



        public TransferController(MessageService s, IHubContext<ChatHub> ch, INotificationService notificationService, MariaDbContext context)
        {
            _sevice = s;
            _myHubContext = ch;
            _notificationService = notificationService;
            _context = context;

        }

        // POST api/<TransferController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Transfer value)
        {
            JsonObject json = new JsonObject();
            json.Add("content", value.content);
            json.Add("sent", false);
            int code = await _sevice.AddMessage(value.to, value.from, json);
            NotificationModel notification = new NotificationModel();
            string username = value.to.ToString();
            notification.DeviceId = _context.UserDB.FirstOrDefault(x => x.Username == username).Token;
            notification.Body = value.content;
            notification.Title = "Message from " + value.from;
            var result = await _notificationService.SendNotification(notification);

            if (code == 404)
                return NotFound();
            if (code == 400)
                return BadRequest();
            signal(value.to);
            return Created("~api/transfer/", value);
        }

        private async void signal(string groupName)
        {
            //await _myHubContext.Clients.All.SendAsync("ReceiveMessage");
             await _myHubContext.Clients.Groups(groupName).SendAsync("ReceiveMessage");
        }
       
    }
}
