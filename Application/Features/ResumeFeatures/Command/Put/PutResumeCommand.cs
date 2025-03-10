﻿namespace Application.Features.ResumeFeatures.Command.Put
{
    public class PutResumeCommand : IRequest<ResponseDTO>
    {
        public Guid ResumeId { get; set; }
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
