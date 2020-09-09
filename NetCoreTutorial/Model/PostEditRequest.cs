namespace NetCoreTutorial.Model
{
    public class PostEditRequest
    {
        public long? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string[] Tags { get; set; }
    }
}