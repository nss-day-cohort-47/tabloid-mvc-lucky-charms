using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class PostTagRepository : BaseRepository, IPostTagRepository
    {
        public PostTagRepository(IConfiguration config) : base(config) { }
        public List<PostTag> GetPostTagsByPostId(int postId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT pt.TagId, t.Name, pt.Id, pt.PostId
                        FROM PostTag pt
                            LEFT JOIN Tag t ON pt.TagId = t.Id
                        WHERE pt.postId = @postId
                    ";

                    cmd.Parameters.AddWithValue("@postId", postId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<PostTag> postTags = new List<PostTag>();
                    while (reader.Read())
                    {
                        PostTag postTag = new PostTag()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            TagId = reader.GetInt32(reader.GetOrdinal("TagId")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId"))
                        };

                        postTags.Add(postTag);
                    }

                    reader.Close();

                    return postTags;
                }
            }
        }
    }
}
