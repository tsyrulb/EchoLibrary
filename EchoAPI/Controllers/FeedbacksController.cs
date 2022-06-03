using Microsoft.AspNetCore.Mvc;
using Repository;
using Domain;
using Services;

namespace EchoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedbacksController : ControllerBase
    {
        private IFeedbackService feedbackService;

        public FeedbacksController(FeedbackContext context)
        {
            feedbackService = new FeedbackService(context);
        }

        [HttpGet]
        public IEnumerable<Feedback> Index()
        {
            Console.WriteLine("Hello from API");
            return feedbackService.GetFeedbacks();
        }

        [HttpPost("Create")]
        public void Create(string id, string content, int score)

        {
            feedbackService.Add(id, content, score);
        }

        [HttpGet("Details/{id}")]
        public Feedback? Details(string id)
        {
            return feedbackService.get(id);
        }

        [HttpPost("Edit/{id}")]
        public void Edit(string id, string content, int score)
        {
            feedbackService.Edit(id, content, score);
        }

        [HttpDelete("Delete/{id}")]
        public void Delete(string id)
        {
            feedbackService?.Delete(id);
        }

    }
}










