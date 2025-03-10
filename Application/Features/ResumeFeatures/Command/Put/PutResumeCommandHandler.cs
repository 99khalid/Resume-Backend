
using Application.DTOs;
using Domain.Enums;
using Domain.Models;
using Infrastructure.UnitOfWork;
using Infrastrucure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UploadHelper.Helpers;

namespace Application.Features.ResumeFeatures.Command.Put
{
    public class PutResumeCommandHandler : IRequestHandler<PutResumeCommand, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private AppDBContext _dbContext;
        public ResponseDTO _responseDto;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hosting ;

        public PutResumeCommandHandler(IWebHostEnvironment hosting, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork, AppDBContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _responseDto = new ResponseDTO();
            _hosting = hosting;
        }

        public async Task<ResponseDTO> Handle(PutResumeCommand request, CancellationToken cancellationToken)
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            var resume = _unitOfWork.Repository<Resume>().GetAllAsTracking(r => r.UserId == userId.Id
            &&(r.Id==request.ResumeId)
            ).FirstOrDefault();

            if (string.IsNullOrEmpty(userName))
            {
                return new ResponseDTO { Result = false, Message = "Invalid token: UserId not found" };
            }
            var attachmentDto = new Attachment();
            if (request.Attachment != null)
            {
                var mainImage = await Upload.UploadImagess(request.Attachment.File, _hosting, request.Attachment.File.Name);
                var attachment = new Attachment
                {
                    FileName = request.Attachment.File.Name,
                    FilePath = mainImage,
                };
                attachmentDto = attachment;
            }
            resume.FullName = request.FullName?? resume.FullName;
            resume.Email = request.Email ?? resume.Email;
            resume.PhoneNumber = request.PhoneNumber ?? resume.PhoneNumber;
            resume.Address = request.Address ?? resume.Address;
            resume.Summary = request.Summary ?? resume.Summary;
            if (request.Educations != null)
            {
                //resume.EducationHistory.Clear();
                foreach(var education in resume.EducationHistory)
                {
                    education.State = State.Deleted;
                }
                resume.EducationHistory = request.Educations.Select(e => new Education
                {
                    InstitutionName = e.InstitutionName,
                    Degree = e.Degree,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate
                }).ToList();
            }
            if (request.Experiences != null)
            {
                //resume.WorkExperience.Clear();
                foreach(var e in resume.WorkExperience)
                {
                    e.State = State.Deleted;
                }
                resume.WorkExperience = request.Experiences.Select(w => new Experience
                {
                    CompanyName = w.CompanyName,
                    JobTitle = w.JobTitle,
                    StartDate = w.StartDate,
                    EndDate = w.EndDate,
                    Description = w.Description
                }).ToList();
            }
            if (request.Skills != null)
            {
                foreach (var skill in resume.Skills)
                {
                    skill.State = State.Deleted;
                }
                resume.Skills = request.Skills.Select(s => new Skill
                {
                    SkillName = s.SkillName
                }).ToList();
            }

            // تحديث Attachments
            if (request.Attachment != null)
            {
                if (resume.Attachments == null)
                {
                    resume.Attachments = new Attachment();
                }
                resume.Attachments.FileName = attachmentDto.FileName;
                resume.Attachments.FilePath = attachmentDto.FilePath;
            }

            _dbContext.Resumes.Update(resume);
            await _dbContext.SaveChangesAsync();
            _responseDto.Message = "Resume Saved!";
            return _responseDto;
        }
    }
}
