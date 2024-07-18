using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Repositories.IRepositories;
using Domain.Services.IServices;

namespace Domain.Services
{
    public class RatingService : IRatingServce
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public Task<string> CalcAvgRatingAsync()
        {
            return _ratingRepository.CalcAvgRatingAsync();
        }
    }
}
