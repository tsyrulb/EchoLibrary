using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Repository;

namespace Services
{
    public interface IFeedbackService
    {
        IEnumerable<Feedback> GetFeedbacks();
        void Edit(string id, string content, int score);
        void Delete(string id);
        void Add(string id, string content, int score);
        Feedback get(string id);
    }
}
