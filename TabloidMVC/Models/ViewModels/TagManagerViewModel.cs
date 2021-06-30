using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class TagManagerViewModel
    {
        public List<Tag> AllTags { get; set; }
        public List<Tag> AddedTags { get; set; }
        public List<Tag> NotAddedTags { get; set; }
    }
}
