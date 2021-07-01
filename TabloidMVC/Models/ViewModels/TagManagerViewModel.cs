using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class TagManagerViewModel
    {
        [DisplayName("Tags")]
        public List<Tag> AllTags { get; set; }
        public List<Tag> AddedTags { get; set; }
        public List<Tag> NotAddedTags { get; set; }
        public Post Post { get; set; }
    }
}
