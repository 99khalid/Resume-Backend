
using Domain.Enums;
using Domain.Models;
using Infrastructure.UnitOfWork;
using Infrastrucure.Context;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Features.ResumeFeatures.Command.Delete
{
    public class DeleteResumeCommandHandler : IRequestHandler<DeleteResumeCommand, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        private AppDBContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ResponseDTO _responseDto;

        public DeleteResumeCommandHandler(IUnitOfWork unitOfWork, AppDBContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _responseDto = new ResponseDTO();
        }

        public async Task<ResponseDTO> Handle(DeleteResumeCommand request, CancellationToken cancellationToken)
        {
            var userName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
            var resume = _unitOfWork.Repository<Resume>().GetAllAsNoTracking(r => r.UserId == userId.Id
            && (r.Id == request.ResumeId)
            ).FirstOrDefault();

            if (string.IsNullOrEmpty(userName))
            {
                return new ResponseDTO { Result = false, Message = "Invalid token: UserId not found" };
            }
            resume.State = State.Deleted;
            _dbContext.Resumes.Update(resume);
            await _dbContext.SaveChangesAsync();
            _responseDto.Message = "Resume Deleted!";
            return _responseDto;
        }
    }
}
