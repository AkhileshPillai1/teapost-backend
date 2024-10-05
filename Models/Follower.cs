using System.ComponentModel.DataAnnotations.Schema;

namespace TeaPost.Models
{
    public class Followers
    {
        public int Follower {  get; set; }
        public int Followed { get; set; }
        public DateTime FollowedAt { get; set; }
    }
}
