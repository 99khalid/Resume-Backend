using Domain.Enums;

namespace Domain.Models
{
    public class BaseEntity<TKey>
    {
        public TKey Id { set; get; }
        public State State { set; get; }
        public Guid CreatedBy { set; get; }
        public DateTime CreatedOn { set; get; }
        public Guid? UpdatedBy { set; get; }
        public DateTime? UpdatedOn { set; get; }
    }
}
