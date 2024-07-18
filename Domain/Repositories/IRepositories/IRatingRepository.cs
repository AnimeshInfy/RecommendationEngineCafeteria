using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.IRepositories
{
    public interface IRatingRepository
    {
        public Task<string> CalcAvgRatingAsync();
    }
}
