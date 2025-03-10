namespace Domain.Models
{
    public class Skill : BaseEntity<Guid>
    {
        public string SkillName { get; set; }
        public Guid ResumeId { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
