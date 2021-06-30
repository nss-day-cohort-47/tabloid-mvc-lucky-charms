using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAllTags();
        List<Tag> GetTagsByPostId(int id);
        Tag GetTagById(int id);
        void Add(Tag tag);
        void DeleteTag(int id);
        void EditTag(Tag tag);
    }
}