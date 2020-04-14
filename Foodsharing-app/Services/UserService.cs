using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DevOne.Security.Cryptography.BCrypt;
using Foodsharing_app.Models;
using MongoDB.Driver;

namespace Foodsharing_app.Services
{
    public class UserService : ICrudService<User>
    {
        private readonly IMongoCollection<User> _items;

        private static readonly Random Random = new Random();

        public UserService(IFoodSharingDatabaseSettings settings, DatabaseService databaseService)
        {
            _items = databaseService.Database.GetCollection<User>(settings.UsersCollectionName);
        }
        
        public bool ActivateUser(string id, string activationToken)
        {
            var user = Get(id);
            var tokenCorrect = user.ActivationToken == activationToken;
            if (tokenCorrect)
            {
                user.Active = true;
                Update(id, user);
            }

            return tokenCorrect;
        }

        public void ChangePassword(User user, string newPlainTextPassword)
        {
            user.HashedPassword = HashPassword(newPlainTextPassword);
            Update(user.Id, user);
        }

        public static bool PasswordIsCorrect(User user, string password) =>
            BCryptHelper.CheckPassword(password, user.HashedPassword);

        private static string HashPassword(string password) =>
            BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt());

        private static string GenerateActivationToken(User user)
        {
            using var sha256 = SHA256.Create();
            var stringToHash = $"{user.Name}{user.Email}{DateTime.Now}{GenerateRandomString(30)}";
            var bytesToHash = Encoding.UTF8.GetBytes(stringToHash);
            return Convert.ToBase64String(sha256.ComputeHash(bytesToHash));
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }

        public User CreateUser(User user, string password)
        {
            user.HashedPassword = HashPassword(password);
            user.Active = false;
            user.RegisteredSince = DateTime.Now;
            user.ActivationToken = GenerateActivationToken(user);
            return Create(user);
        }

        public List<User> Get() =>
            _items.Find(item => true).ToList();

        public User Get(string id) =>
            _items.Find(item => item.Id == id).FirstOrDefault();

        public User GetByUsername(string name) =>
            _items.Find(item => item.Name == name).FirstOrDefault();

        public User Create(User user)
        {
            _items.InsertOne(user);
            return user;
        }

        public void Update(string id, User user) =>
            _items.ReplaceOne(item => item.Id == id, user);

        public void Remove(User user) =>
            _items.DeleteOne(item => item.Id == user.Id);

        public void Remove(string id) =>
            _items.DeleteOne(item => item.Id == id);
    }
}