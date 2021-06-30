using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TabloidMVC.Models.ViewModels
{
    public class ProtectAdminViewModel
    {
        public UserProfile userProfile { get; set; }

        public bool CanDeactivate = true;
        public bool CanDemote = true;
    }
}
