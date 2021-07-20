using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imovi_web_app_backend.Models
{
    public class UserProfileColors
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public virtual User User { get; set; }
    }
}
