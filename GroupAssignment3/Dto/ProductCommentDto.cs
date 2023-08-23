namespace GroupAssignment3.Dto
{
    public class CommentImage
    {
        public string ImagePath { get; set; }
    }

    public class ProductCommentDto
    {
        public string ProductId { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; }

        public List<CommentImage> images { get; set; }

    }
}
