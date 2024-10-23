namespace TeaPost.DTOs.Comment
{
    public class CreateCommentDTO
    {
        public int PostId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
