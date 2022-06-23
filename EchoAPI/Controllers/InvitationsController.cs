
using Domain;
using Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Services;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EchoAPI.Controllers
{
    [Route("api/invitations/")]
    [ApiController]
    public class InvitationsController : ControllerBase
    {
        private ContactService _sevice;
        private MariaDbContext _context;
        private readonly IHubContext<ChatHub> _myHubContext;
        private readonly INotificationService _notificationService;

        public InvitationsController(ContactService s, INotificationService notificationService, MariaDbContext contextData, IHubContext<ChatHub> ch)
        {
            _notificationService = notificationService;
            _sevice = s;
            _context = contextData;
            _myHubContext = ch;
        }

        // POST api/<Inventations>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Invitation invt)
        {
            JsonObject json = new JsonObject();
            json.Add("id", invt.from);
            json.Add("name", invt.from);
            json.Add("server", invt.server);
            int code = await _sevice.AddContact(json, invt.to);

            NotificationModel notification = new NotificationModel();
            string username = invt.to.ToString();
            notification.DeviceId = _context.UserDB.FirstOrDefault(x => x.Username == username).Token;
            notification.Body = "type:invitation," + "server:"+invt.server+",from:"+invt.from;
            notification.Title = "Invitation from " + invt.from;
            var result = await _notificationService.SendNotification(notification);

            if (code == 404)
                return NotFound();
            if (code == 400)
                return BadRequest();
            signal(invt.to);
            return Created("~api/invintations/", invt);
        }

        private async void signal(string groupName)
        {
            await _myHubContext.Clients.Groups(groupName).SendAsync("ReceiveMessage");

        }
    }
}