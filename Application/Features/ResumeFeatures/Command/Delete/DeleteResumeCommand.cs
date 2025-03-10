namespace Application.Features.ResumeFeatures.Command.Delete
{
    public class DeleteResumeCommand:IRequest<ResponseDTO>
    {
        public Guid ResumeId { get; set; }
    }
}
