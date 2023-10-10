//   Sri Lanka Institute of Information Technology
//   Year  :  4th Year 2nd Semester
//   Module Code  :  SE4040
//   Module  :  Enterprise Application Development
//   Student Id Number  :  IT20236014
//   Name  :  Ravindu Yasith T.K.

using EAD_Web_Services.DatabaseConfiguration;
using EAD_Web_Services.Models.TrainModel;
using EAD_Web_Services.Models.UserModel;
using MongoDB.Driver;
using System.Security.Cryptography;
using System.Text;

namespace EAD_Web_Services.Services.UserService
{
    /// <summary>
    /// Service class for managing User objects.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;

        /// <summary>
        /// Initializes a new instance of the UserService class.
        /// </summary>
        /// <param name="settings">The database settings.</param>
        /// <param name="mongoClient">The MongoDB client.</param>
        public UserService( IDatabaseSettings settings , IMongoClient mongoClient) 
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<User>(settings.UsersCollectionName);

        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="userReq">The User object to create.</param>
        /// <returns>A message indicating the result of the operation.</returns>
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

        /// <summary>
        /// Delete a user by NIC
        /// </summary>
        /// <param name="nic">The NIC of the user to delete</param>
        public void Delete(string nic)
        {
            _users.DeleteOne(user => user.Nic == nic);
        }

        /// <summary>
        ///Get a List of all users
        /// </summary>
        /// <returns>A list of User Objects</returns>
        public List<User> Get()
        {
            return _users.Find(user => true).ToList();
        }

        /// <summary>
        /// Get a user by NIC.
        /// </summary>
        /// <param name="id">The NIC of the user to retrieve.</param>
        /// <returns>The User object if found; otherwise, returns null.</returns>
        public User Get(string id)
        {
            return _users.Find(user => user.Nic == id).FirstOrDefault();
        }

        /// <summary>
        /// Update a user by NIC.
        /// </summary>
        /// <param name="id">The NIC of the user to update.</param>
        /// <param name="user">The User object with updated information.</param>
        public void Update(string id, User user)
        {
            user.Password = EncryptPassword(user.Password);

            _users.ReplaceOne(user => user.Nic == id, user);
        }

        /// <summary>
        /// Update the status (active/deactive) of a user
        /// </summary>
        /// <param name="nic">The NIC of the user to update</param>
        /// <returns>A message indicating the updated results</returns>
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

        /// <summary>
        /// Authenticate a user by NIC and password
        /// </summary>
        /// <param name="nic">The NIC of the user</param>
        /// <param name="password">The user's password</param>
        /// <returns>A message indicating the authentication results</returns>
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

        /// <summary>
        /// Verify a plain text password against a stored hashed password.
        /// </summary>
        /// <param name="plainTextPassword">The plain text password to verify.</param>
        /// <param name="storeddPassword">The stored hashed password to compare against.</param>
        /// <returns>True if the password is verified, otherwise false.</returns>
        private static bool VerifyPassword(string plainTextPassword, string storeddPassword)
        {
            
            string hashedPassword = EncryptPassword(plainTextPassword);
            return hashedPassword.Equals(storeddPassword);
        }

        /// <summary>
        /// Encrypt a password using SHA256 hashing.
        /// </summary>
        /// <param name="password">The password to encrypt.</param>
        /// <returns>The hashed password.</returns>
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
