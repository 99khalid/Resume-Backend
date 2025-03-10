using Domain.Enums;
using Infrastructure.ResponseDTOs;
using MediatR;

namespace Application.Features.UserFeatures.Command
{
    public class RegisterUserCommand : IRequest<ResponseDTO>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
