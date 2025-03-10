namespace Domain.Models
{
    public class Attachment : BaseEntity<Guid>
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public Guid ResumeId { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
