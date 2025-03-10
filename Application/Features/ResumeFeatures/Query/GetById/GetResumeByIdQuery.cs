namespace Application.Features.ResumeFeatures.Query.GetById
{
    public class GetResumeByIdQuery : IRequest<ResponseDTO>
    {
        public Guid ResumeId { get; set; }
    }
}
