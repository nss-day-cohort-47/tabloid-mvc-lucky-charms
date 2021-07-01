using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class PostAllCategoryViewModel
    {
        public Post Post { get; set; }

        public List<Category> AllCategories { get; set; }
    }
}
