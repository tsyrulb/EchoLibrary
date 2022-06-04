using Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EchoAPI.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/contacts/")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ContactService _service;
        private string? username;
        private readonly IHubContext<ChatHub> _myHubContext;

        public ContactController(ContactService context, IHubContext<ChatHub> ch)
        {
            _service = context;
             username = null;
            _myHubContext = ch;
        }

        // GET: api/<ContactController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if (username == null)
                GetUserId();
            return Ok(await _service.GetContacts(username));
        }

        // GET api/<ContactController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (username == null)
                GetUserId();
            var contact = await _service.GetContact(id, username);
            if (contact == null)
                return NotFound();
            return Ok(contact);
        }

        // POST api/<ContactController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JsonObject contact)
        {
            if (username == null)
                GetUserId();
            int code = await _service.AddContact(contact, username);
            if (code == 400)
                return BadRequest();
            if (code == 404)
                return NotFound();
            signal(username);
            return Created("~api/contacts/", contact);
        }

        // PUT api/<ContactController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] JsonObject contact)
        {
            if (username == null)
                GetUserId();
            int code = await _service.ChangeContact(id, username, contact);
            if (code == 404)
                return NotFound();
            return new NoContentResult(); 
        }

        // DELETE api/<ContactController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (username == null)
                GetUserId();
            int code = await _service.DeleteContact(id, username);
            if (code == 404)
                return NotFound();
            if (code == 400)
                return BadRequest();
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
            await _myHubContext.Clients.Groups(groupName).SendAsync("ReceiveMessage");

        }

    }
}
