using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface IFeedbackService
    {
        Task AddFeedbackAsync(Feedback feedback);
        Task<List<Feedback>> GetFeedbacksByFeedbackIdAsync(int id);
        Task<List<Feedback>> ViewAllFeedbacksAsync();
    }
}
