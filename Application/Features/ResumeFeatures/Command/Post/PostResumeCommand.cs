

namespace Application.Features.ResumeFeatures.Command.Post
{
    public class PostResumeCommand : IRequest<ResponseDTO>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Summary { get; set; }

        public List<EducationDto>? Educations { get; set; }
        public List<SkillDto>? Skills { get; set; }
        public AttachmentDto? Attachment { get; set; }
        public List<ExperienceDto>? Experiences { get; set; }
    }
}
