using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace imovi_web_app_backend.Models
{
    public class FavoriteMovie
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
    }
}
