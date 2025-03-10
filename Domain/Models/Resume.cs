namespace Domain.Models
{
    public class Resume : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Summary { get; set; }
        public virtual ICollection<Education> EducationHistory { get; set; } = new HashSet<Education>();
        public virtual ICollection<Experience> WorkExperience { get; set; } = new HashSet<Experience>();
        public virtual ICollection<Skill> Skills { get; set; } = new HashSet<Skill>();
        public virtual Attachment Attachments { get; set; }
    }
}
