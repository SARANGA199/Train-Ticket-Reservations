using EAD_Web_Services.Models.UserModel;

namespace EAD_Web_Services.Services.UserService
{
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
