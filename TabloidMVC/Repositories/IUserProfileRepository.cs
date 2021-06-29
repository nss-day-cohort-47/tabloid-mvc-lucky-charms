﻿using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        UserProfile GetByEmail(string email);
        List<UserProfile> GetAllUsers();
        UserProfile GetUserProfileById(int id);
        void DeactivateUser(int id);
    }
}