using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }

        public List<UserProfile> GetAllUsers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
                                            U.Id,
                                            U.FirstName,
                                            U.LastName,
                                            U.DisplayName,
                                            U.UserTypeId,
                                            ut.Name
                                        FROM UserProfile U
                                        LEFT JOIN UserType ut ON U.UserTypeId = ut.Id
                                        ORDER BY DisplayName
                                        ";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<UserProfile> userProfiles = new List<UserProfile>();

                    while (reader.Read())
                    {
                        UserProfile userProfile = new UserProfile
                        {
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            Id = reader.GetInt32(reader.GetOrdinal("Id"))
                        };
                        userProfiles.Add(userProfile);
                    }
                    reader.Close();
                    return userProfiles;
                }
            }
        }
        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }

        public UserProfile GetUserProfileById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
                                            u.Id,
                                            u.DisplayName, 
                                            u.FirstName, 
                                            u.LastName, 
                                            u.Email, 
                                            u.CreateDateTime, 
                                            u.ImageLocation, 
                                            u.UserTypeId,
                                            ut.[Name] AS UserTypeName
                                        FROM UserProfile u
                                        LEFT JOIN UserType ut on u.UserTypeId = ut.Id
                                        WHERE u.id = @id
                                        ";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        UserProfile userProfile = new UserProfile()
                        {
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };

                        if (reader.IsDBNull(reader.GetOrdinal("ImageLocation")) == false)
                        {
                            userProfile.ImageLocation = reader.GetString(reader.GetOrdinal("ImageLocation"));
                        };
                        reader.Close();

                        return userProfile;
                    }
                    reader.Close();
                    return null;
                }
            }
        }

        public void DeactivateUser(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM UserProfile WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
