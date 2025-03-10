using Domain.Enums;
using Domain.Models;
using Infrastructure.ResponseDTOs;
using Infrastructure.UnitOfWork;
using Infrastrucure.Context;
using MediatR;

namespace Application.Features.UserFeatures.Command
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public ResponseDTO _responseDto;

        public RegisterUserHandler( IUnitOfWork unitOfWork)
        {
            _responseDto = new ResponseDTO();
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existedUser= _unitOfWork.Repository<User>().GetAllAsNoTracking(u => u.UserName == request.UserName).FirstOrDefault();
            if (existedUser!=null)
            {
                _responseDto.Message = "User already exists";
                return _responseDto;
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                PasswordHash = passwordHash,
                UserRole = Role.User,                
                CreatedBy = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow
            };
            await  _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.Commit();
            _responseDto.Message = "Signed Up Successfully!";
            return _responseDto;

        }
    }
}
