using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository(IConfiguration config) : base(config) { }

        public List<Comment> GetCommentsByPost(int postId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
						Select	c.Id,
								c.PostId,
								c.UserProfileId,
								c.Subject,
								c.Content,
								c.CreateDateTime,
								p.Title AS PostTitle,
								p.Content AS PostContent,
								p.ImageLocation AS PostImageLocation,
								p.CreateDateTime AS PostCreateDateTime,
								p.PublishDateTime AS PostPublishDateTime,
								p.IsApproved AS PostIsApproved,
                                p.CategoryId AS PostCategoryId,
								up.DisplayName AS UserDisplayName,
								up.FirstName AS UserFirstName,
								up.LastName AS UserLastName,
								up.Email AS UserEmail,
								up.CreateDateTime AS UserCreateDateTime,
								up.ImageLocation AS UserImageLocation,
								up.UserTypeId
						FROM Comment c
						LEFT JOIN Post p ON p.Id = c.PostId
						LEFT JOIN UserProfile up ON p.UserProfileId = up.Id
						WHERE c.PostId = @postId
					";
                    cmd.Parameters.AddWithValue("@postId", postId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    while (reader.Read())
                    {
                        Comment comment = new Comment()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            Post = new Post()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("PostId")),
                                Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                                Content = reader.GetString(reader.GetOrdinal("PostContent")),
                                ImageLocation = reader.IsDBNull(reader.GetOrdinal("PostImageLocation"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("PostImageLocation")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("PostCreateDateTime")),
                                PublishDateTime = reader.IsDBNull(reader.GetOrdinal("PostPublishDateTime"))
                                ? null
                                : reader.GetDateTime(reader.GetOrdinal("PostPublishDateTime")),
                                IsApproved = reader.GetBoolean(reader.GetOrdinal("PostIsApproved"))
                            },
                            UserProfile = new UserProfile()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                                DisplayName = reader.GetString(reader.GetOrdinal("UserDisplayName")),
                                FirstName = reader.GetString(reader.GetOrdinal("UserFirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("UserLastName")),
                                Email = reader.GetString(reader.GetOrdinal("UserEmail")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("UserCreateDateTime")),
                                ImageLocation = reader.IsDBNull(reader.GetOrdinal("UserImageLocation"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("UserImageLocation")),
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId"))
                            }
                        };
                        comments.Add(comment);
                    }
                    reader.Close();

                    return comments;
                }
            }
        }

        public Comment GetCommentById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
						Select	c.Id,
								c.PostId,
								c.UserProfileId,
								c.Subject,
								c.Content,
								c.CreateDateTime,
								p.Title AS PostTitle,
								p.Content AS PostContent,
								p.ImageLocation AS PostImageLocation,
								p.CreateDateTime AS PostCreateDateTime,
								p.PublishDateTime AS PostPublishDateTime,
								p.IsApproved AS PostIsApproved,
                                p.CategoryId AS PostCategoryId,
								up.DisplayName AS UserDisplayName,
								up.FirstName AS UserFirstName,
								up.LastName AS UserLastName,
								up.Email AS UserEmail,
								up.CreateDateTime AS UserCreateDateTime,
								up.ImageLocation AS UserImageLocation,
								up.UserTypeId
						FROM Comment c
						LEFT JOIN Post p ON p.Id = c.PostId
						LEFT JOIN UserProfile up ON p.UserProfileId = up.Id
						WHERE c.Id = @id
					";
                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Comment comment = new Comment()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            Post = new Post()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("PostId")),
                                Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                                Content = reader.GetString(reader.GetOrdinal("PostContent")),
                                ImageLocation = reader.IsDBNull(reader.GetOrdinal("PostImageLocation"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("PostImageLocation")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("PostCreateDateTime")),
                                PublishDateTime = reader.IsDBNull(reader.GetOrdinal("PostPublishDateTime"))
                                ? null
                                : reader.GetDateTime(reader.GetOrdinal("PostPublishDateTime")),
                                IsApproved = reader.GetBoolean(reader.GetOrdinal("PostIsApproved"))
                            },
                            UserProfile = new UserProfile()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                                DisplayName = reader.GetString(reader.GetOrdinal("UserDisplayName")),
                                FirstName = reader.GetString(reader.GetOrdinal("UserFirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("UserLastName")),
                                Email = reader.GetString(reader.GetOrdinal("UserEmail")),
                                CreateDateTime = reader.GetDateTime(reader.GetOrdinal("UserCreateDateTime")),
                                ImageLocation = reader.IsDBNull(reader.GetOrdinal("UserImageLocation"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("UserImageLocation")),
                                UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId"))
                            }
                        };
                        reader.Close();
                        return comment;
                    }
                    reader.Close();
                    return null;
                }
            }
        }

        public void AddComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Comment (PostId, UserProfileId, Subject, Content, CreateDateTime)
                        OUTPUT INSERTED.ID
                        VALUES (@postId, @userProfileId, @subject, @content, @createDateTime)
                    ";

                    cmd.Parameters.AddWithValue("@postId", comment.PostId);
                    cmd.Parameters.AddWithValue("@userProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", DateTime.Now);

                    int id = (int)cmd.ExecuteScalar();

                    comment.Id = id;
                }
            }
        }

        public void UpdateComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Comment
                            SET 
                                Subject = @subject, 
                                Content = @content                            
                            WHERE Id = @id
                    ";
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@id", comment.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteComment(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Comment
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
