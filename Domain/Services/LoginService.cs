using Domain.Repositories;
using Domain.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class LoginService : ILoginService
    {
        private readonly UserRepository _userRepository;    
        public LoginService(UserRepository userRepository) 
        {
            _userRepository = userRepository;   
        }   
        public string AuthorizeUserAndFetchRole(int id, string name)
        {
            //FIGURE OUT HOW YOU WILL FETCH ID and NAME
            var user_role = _userRepository.GetUserRole(id, name);
            if (user_role == "Admin")
            {

            }
            else if (user_role == "Employee")
            {
                 
            }
            else if (user_role == "Chef")
            {

            }
            else
            {

            }
            return user_role;

        }


        public void ShowMenuByRole()
        {
            
        }
    }
}
