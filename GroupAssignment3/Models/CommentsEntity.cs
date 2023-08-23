using System.ComponentModel.DataAnnotations;

public class CommentsEntity
{
    [Key]
    public string commentId { get; set; }
    public int rating { get; set; }
    public List<CommentImage> commentImages { get; set; } = new List<CommentImage>();
    public string? text { get; set; }
    // one to one
    public required UserEntity user { get; set; }
    // one to one
    public required ProductEntity product { get; set; }
}

public class CommentImage
{
    [Key]
    public string imageId { get; set; }
    public required string imagePath { get; set; }
}