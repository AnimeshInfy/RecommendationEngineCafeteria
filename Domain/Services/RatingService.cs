using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataAccess;
using Domain.Models;
using Domain.Repositories.IRepositories;
using Domain.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace Domain.Services
{
    public class RatingService : IRatingServce
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        Task IRatingServce.CalculateAverageRatingAsync()
        {
            return _ratingRepository.CalculateAverageRatingAsync();
        }
    }
}
