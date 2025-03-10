namespace Domain.Models
{
    public class Education : BaseEntity<Guid>
    {
        public string InstitutionName { get; set; }
        public string Degree { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid ResumeId { get; set; }
        public virtual Resume Resume { get; set; }
    }
}
