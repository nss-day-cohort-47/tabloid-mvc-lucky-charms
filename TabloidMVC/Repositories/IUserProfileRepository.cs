using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        List<UserProfile> GetAllUsers();
        List<UserProfile> GetAllUnathenticatedUsers();
        UserProfile GetUserProfileById(int id);
        void DeactivateUser(int id);
        void ReactivateUser(int id);
        UserProfile GetById(int id);
        void AddUserProfile(UserProfile user);
        void EditUserType(UserProfile userProfile);
        int CheckNumOfAdmins();
    }
}