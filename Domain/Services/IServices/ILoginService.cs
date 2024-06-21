using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.IServices
{
    public interface ILoginService
    {
        public string AuthorizeUserAndFetchRole(int id, string name);
        public void ShowMenuByRole();
    }
}
