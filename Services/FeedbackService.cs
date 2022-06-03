using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FeedbackService : IFeedbackService
    {
        FeedbackContext feedbackContext;

        public FeedbackService(FeedbackContext context)
        {
            feedbackContext = context;
        }

        public void Add(string id, string content, int score)
        {
            bool IsIDNameExist = feedbackContext.Feedback.Any
          (x => x.Id == id);

            if (IsIDNameExist)
            {
                return;
            }
            else
            {
                Feedback feedback = new Feedback();
                feedback.Id = id;
                feedback.Content = content;
                if (score < 1 && score > 5)
                {
                    return;
                }
                feedback.Score = score;
                feedback.Date = DateTime.Now;
                feedbackContext.Feedback.Add(feedback);
                feedbackContext.SaveChanges();
            }
            return;
        }

        public void Delete(string id)
        {
            var f = feedbackContext.Feedback.FirstOrDefault(m => m.Id == id);
            if (f != null)
            {
                feedbackContext.Feedback.Remove(f);

            }
            feedbackContext.SaveChanges();
            return;
        }

        public void Edit(string id, string content, int score)
        {
            var f = feedbackContext.Feedback.FirstOrDefault(m => m.Id == id);
            if (f != null)
            {
                if (f != null)
                {
                    f.Content = content;
                }
                if (score < 6 && score > 0)
                {
                    f.Score = score;
                }
                f.Date = DateTime.Now;
            }
            feedbackContext.SaveChanges();
            return;
        }

        public Feedback get(string id)
        {
            return feedbackContext.Feedback.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Feedback> GetFeedbacks()
        {
            return feedbackContext.Feedback;
        }
    }
}
