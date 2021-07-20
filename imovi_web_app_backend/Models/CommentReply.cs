using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace imovi_web_app_backend.Models {
    public class CommentReply {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public int Rating { get; set; }
        public virtual Comment Comment { get; set; }
    }
}
