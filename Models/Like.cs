using Microsoft.EntityFrameworkCore;

namespace TeaPost.Models
{
    [Keyless]
    public class Like
    {
        public int PostId { get; set; }
        public int UserId {  get; set; }
    }
}
