using Domain.Models;
using Infrastructure.UnitOfWork;
using Infrastrucure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using UploadHelper.Helpers;

namespace Application.Features.ResumeFeatures.Command.Post
{
    public class PostResumeCommandHandler : IRequestHandler<PostResumeCommand, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private AppDBContext _dbContext;
        public ResponseDTO _responseDto;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hosting ;

        public PostResumeCommandHandler(IWebHostEnvironment hosting, IUnitOfWork unitOfWork, AppDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _responseDto = new ResponseDTO();
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _hosting = hosting;
        }

        public async Task<ResponseDTO> Handle(PostResumeCommand request, CancellationToken cancellationToken)
        {

            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId= _dbContext.Users.FirstOrDefault(u=>u.UserName==userName);
            if (string.IsNullOrEmpty(userName))
            {
                return new ResponseDTO { Result = false, Message = "Invalid token: UserId not found" };
            }
            var cvDto = request;
            var attachmentDto = new Attachment();
            if (request.Attachment!= null)
            {
                var mainImage = await Upload.UploadFiles(request.Attachment.File, _hosting, request.Attachment.File.Name);
                var attachment = new Attachment
                {
                    FileName = request.Attachment.File.Name,
                    FilePath = mainImage,
                };
                attachmentDto = attachment;
            }
          
           var cv = new Resume
            {
                UserId = userId.Id,
                FullName = cvDto.FullName,
                Email = cvDto.Email,
                PhoneNumber = cvDto.PhoneNumber,
                Address = cvDto.Address,
                Summary = cvDto.Summary,
               EducationHistory = cvDto.Educations?.Select(e => new Education
               {
                   InstitutionName = e.InstitutionName,
                   Degree = e.Degree,
                   StartDate = e.StartDate,
                   EndDate = e.EndDate
               }).ToList(),
               WorkExperience = cvDto.Experiences?.Select(w => new Experience
               {
                   CompanyName = w.CompanyName,
                   JobTitle = w.JobTitle,
                   StartDate = w.StartDate,
                   EndDate = w.EndDate,
                   Description = w.Description
               }).ToList(),
               Skills = cvDto.Skills?.Select(s => new Skill
               {
                   SkillName = s.SkillName
               }).ToList(),
               Attachments = cvDto.Attachment != null ? attachmentDto : null
           
           };
            _dbContext.Resumes.Add(cv);
              await _dbContext.SaveChangesAsync();
            //_unitOfWork.Commit();

            _responseDto.Message = "Resume Saved!";
            return _responseDto;
        }
    }
}
