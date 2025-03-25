namespace NSurePhysicsWebAPI.DTOs
{
    public class TopicWithAttachmentDto
    {
        public int TopicsId { get; set; }
        public string? TopicsName { get; set; }
        public int TopicAttachmentsId { get; set; }
        public string? TopicAttachmentFileName { get; set; }
    }
}
