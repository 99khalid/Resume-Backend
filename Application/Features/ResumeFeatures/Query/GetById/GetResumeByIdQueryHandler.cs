
using Domain.Enums;
using Domain.Models;
using Infrastructure.UnitOfWork;
using Infrastrucure.Context;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.ResumeFeatures.Query.GetById
{
    public class GetResumeByIdQueryHandler : IRequestHandler<GetResumeByIdQuery, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private AppDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResponseDTO _responseDto;

        public GetResumeByIdQueryHandler(IUnitOfWork unitOfWork, AppDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _responseDto = new ResponseDTO();
        }

        public async Task<ResponseDTO> Handle(GetResumeByIdQuery request, CancellationToken cancellationToken)
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            var resume = _unitOfWork.Repository<Resume>().GetAllAsTracking(r => r.UserId == userId.Id && r.Id==request.ResumeId && r.State==State.NotDeleted).FirstOrDefault();

            if (string.IsNullOrEmpty(userName))
            {
                return new ResponseDTO { Result = false, Message = "Invalid token: UserId not found" };
            }
            if (resume == null)
            {
                return new ResponseDTO { Result = false, Message = "No Resumes Found!" };
            }
            var resumeDto = new GetResumesDto
            {
                ResumeId = resume.Id,
                FullName = resume.FullName,
                Address = resume.Address,
                Email = resume.Email,
                PhoneNumber = resume.PhoneNumber,
                Summary = resume.Summary,
                Educations = resume.EducationHistory?.Select(e => new EducationDto
                {
                    Degree = e.Degree,
                    EndDate = e.EndDate,
                    InstitutionName = e.InstitutionName,
                    StartDate = e.StartDate

                }).ToList() ?? null,
                Experiences = resume.WorkExperience?.Select(w => new ExperienceDto
                {
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    CompanyName = w.CompanyName,
                    Description = w.Description,
                    JobTitle = w.JobTitle,
                }).ToList() ?? null,
                Skills = resume.Skills?.Select(s => new SkillDto
                {
                    SkillName = s.SkillName
                }).ToList() ?? null,
                Attachment = resume.Attachments != null ? new GetAttachmentDto
                {
                    Id = resume.Attachments.Id,
                    FileName = resume.Attachments.FileName,
                    FilePath = resume.Attachments.FilePath
                } : null
            };
            return new ResponseDTO { Message = "Success!", Result = resumeDto };
        }
    }
}
