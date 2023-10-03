using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.TrainModel;
using EAD_Web_Services.Models.UserModel;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

namespace EAD_Web_Services.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService( IDatabaseSettings settings , IMongoClient mongoClient) 
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);

        }

        public string Create(User userReq)
        {
            var existingUser = _users.Find(user => user.Nic == userReq.Nic).FirstOrDefault();
            if (existingUser != null)
            {
                return "User NIC already exists.";
            }

            userReq.Password = EncryptPassword(userReq.Password);

            _users.InsertOne(userReq);
            return "User registered successfully.";
        }

        public void Delete(string nic)
        {
            _users.DeleteOne(user => user.Nic == nic);
        }

        public List<User> Get()
        {
            return _users.Find(user => true).ToList();
        }

        public User Get(string id)
        {
            return _users.Find(user => user.Nic == id).FirstOrDefault();
        }

        public void Update(string id, User user)
        {
            user.Password = EncryptPassword(user.Password);

            _users.ReplaceOne(user => user.Nic == id, user);
        }

        public string UpdateStatus(string nic)
        {
            var user = _users.Find(user => user.Nic == nic).FirstOrDefault();
            var status = !user.IsActive;
            _users.UpdateOne(user => user.Nic == nic, Builders<User>.Update.Set("IsActive", status));

            if (status)
            {
                return "Traveler account is activated";
            }
            else
            {
                return "Traveler account is deactivated";
            }
        }

        public string Login(string nic, string password)
        {
            var user = _users.Find(user => user.Nic == nic).FirstOrDefault();

            if (user == null)
                return "User not found";
            if (!user.IsActive)
                return "Account is deactivated";
            var isVerified = VerifyPassword(password, user.Password);

            if (isVerified)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        private static bool VerifyPassword(string plainTextPassword, string storeddPassword)
        {
            
            string hashedPassword = EncryptPassword(plainTextPassword);
            return hashedPassword.Equals(storeddPassword);
        }

        private static string EncryptPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            
            StringBuilder stringBuilder = new();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                stringBuilder.Append(hashBytes[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

    }
}
