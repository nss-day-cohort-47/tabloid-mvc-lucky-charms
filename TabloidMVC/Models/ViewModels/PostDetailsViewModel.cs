using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public List<Tag> Tags { get; set; }
        public bool ShowSubscribe { get; set; }
        public bool ShowUnsubscribe { get; set; }
        public bool CanInteract = false;
    }
}
