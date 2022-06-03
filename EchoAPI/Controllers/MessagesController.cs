using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Services;

using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EchoAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/contacts/{contactid}/messages/")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private MessageService _service;
        private string? username;
        private readonly IHubContext<ChatHub> _myHubContext;

        public MessagesController(MessageService service, IHubContext<ChatHub> ch)
        {
            _service = service;
            username = null;
            _myHubContext = ch;
        }

        // GET: api/<MessagesController>
        [HttpGet]
        public IActionResult Get(string contactid)
        {
            if (username == null)
                GetUserId();
            
            return Ok(_service.GetMessages(username, contactid));
        }

        [HttpPost]
        public IActionResult Post(string contactid, [FromBody] JsonObject message)
        {
            if (username == null)
                GetUserId();
            message.Add("sent", true);
            int code = _service.AddMessage(username, contactid, message);
            if (code == 400)
                return BadRequest();
            if (code == 404)
                return NotFound();
            signal(username);
            return Created("~api/contacts/{contactid}/messages", message);
        }
        // GET api/<MessagesController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string contactid, long id)
        {
            if (username == null)
                GetUserId();
            var m = _service.GetMessage(id, username, contactid);
            if (m == null)
                return NotFound();
            return Ok(m);
        }

        // PUT api/<MessagesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string contactid, long id, [FromBody] JsonObject content)
        {
            if (username == null)
                GetUserId();
            int code = _service.ChangeMessage(contactid, id, username, content);
            if (code == 400)
                return BadRequest();
            if (code == 404)
                return NotFound();
            return new NoContentResult();
        }


        // DELETE api/<MessagesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string contactid, long id)
        {
            if (username == null)
                GetUserId();
            int code = _service.DeleteMessage(id, username, contactid);
            if (code == 404)
                return NotFound();
            return new NoContentResult();
        }

        private async void GetUserId()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            username = User.Claims.FirstOrDefault(c => c.Type.Equals("UserId", StringComparison.InvariantCultureIgnoreCase)).Value;

        }

        private async void signal(string groupName)
        {
            //await _myHubContext.Clients.All.SendAsync("ReceiveMessage");
            await _myHubContext.Clients.Groups(groupName).SendAsync("ReceiveMessage");
        }
    }
}
