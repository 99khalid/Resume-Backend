
using Domain.Enums;
using Domain.Models;
using Infrastructure.UnitOfWork;
using Infrastrucure.Context;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.ResumeFeatures.Query.GetAll
{
    public class GetAllResumesQueryHandler : IRequestHandler<GetAllResumesQuery, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private AppDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResponseDTO _responseDto;

        public GetAllResumesQueryHandler(IUnitOfWork unitOfWork, AppDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _responseDto = new ResponseDTO();
        }

        public async Task<ResponseDTO> Handle(GetAllResumesQuery request, CancellationToken cancellationToken)
        {
            
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            var resumes = _unitOfWork.Repository<Resume>().GetAllAsNoTracking(r => r.UserId == userId.Id &&r.State==State.NotDeleted).ToList();

            if (string.IsNullOrEmpty(userName))
            {
                return new ResponseDTO { Result = false, Message = "Invalid token: UserId not found" };
            }
            if (!resumes.Any())
            {
                return new ResponseDTO { Result = false, Message = "No Resumes Found!" };
            }
            var resumeDto = resumes.Select(r => new GetResumesDto
            {
                ResumeId=r.Id,
                FullName=r.FullName,
                Address=r.Address,
                Email=r.Email,
                PhoneNumber=r.PhoneNumber,
                Summary = r.Summary,
                Educations= r.EducationHistory?.Select(e=> new EducationDto
                {
                    Degree=e.Degree,
                    EndDate=e.EndDate,
                    InstitutionName=e.InstitutionName,
                    StartDate = e.StartDate

                }).ToList() ?? null,
                Experiences=r.WorkExperience?.Select(w=> new ExperienceDto
                {
                    StartDate=w.StartDate,
                    EndDate =w.EndDate,
                    CompanyName=w.CompanyName,
                    Description=w.Description,
                    JobTitle = w.JobTitle,
                }).ToList() ?? null,
                Skills=r.Skills?.Select(s=> new SkillDto
                {
                    SkillName=s.SkillName                    
                }).ToList()?? null,
                Attachment=r.Attachments != null ? new GetAttachmentDto
                {
                    Id=r.Attachments.Id,
                    FileName = r.Attachments.FileName,
                    FilePath = r.Attachments.FilePath
                } : null
            }).ToList();
            return new ResponseDTO { Message = "Success!", Result = resumeDto };
        }
    }
}
