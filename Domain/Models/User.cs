using Domain.Enums;

namespace Domain.Models
{
    public class User : BaseEntity<Guid>
    {
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public Role UserRole { get; set; }
        public virtual ICollection<Resume> Resumes { get; set; } = new HashSet<Resume>();

    }
}
