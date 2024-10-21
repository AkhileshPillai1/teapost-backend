using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TeaPost.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public int AuthorId { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
