//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20236014
//   Name  :  Ravindu Yasith T.K.

using EAD_Web_Services.Models.UserModel;

namespace EAD_Web_Services.Services.UserService
{
    /// <summary>
    /// Interface for the User service.
    /// </summary>
    public interface IUserService
    {
        List<User> Get();
        User Get(string nic);
        string Create(User user);
        void Update(string nic, User user);
        void Delete(string nic);
        string Login(string nic, string password);
        string UpdateStatus(string nic);
        List<User> GetbyRole(string role);

    }
}
