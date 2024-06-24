using Domain.DataAccess;
using Domain.Models;
using Domain.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly CafeteriaDbContext _context;

        public FeedbackRepository(CafeteriaDbContext context)
        {
            _context = context;
        }

        public async Task AddFeedbackAsync(Feedback feedback)
        {
            await _context.Feedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Feedback>> GetFeedbacksByFeedbackIdAsync(int Id)
        {
            return await _context.Feedbacks.Where(f => f.Id == Id).ToListAsync();
        }

        public async Task<List<Feedback>> ViewAllFeedbacks()
        {
            return await _context.Feedbacks.ToListAsync();
        }

    }
}
