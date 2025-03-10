using Domain.Enums;

namespace Infrastructure.ResponseDTOs
{
    public abstract class ParentResponseDTO
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
        public int PageSize { get; set; }
        public StatusEnum StatusEnum { get; set; }

        public string Message { get; set; }

    }
}
