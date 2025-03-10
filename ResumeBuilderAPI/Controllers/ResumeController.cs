using Application.DTOs;
using Application.Features.ResumeFeatures.Command.Delete;
using Application.Features.ResumeFeatures.Command.Post;
using Application.Features.ResumeFeatures.Command.Put;
using Application.Features.ResumeFeatures.Query.GetAll;
using Application.Features.ResumeFeatures.Query.GetById;
using Infrastructure.ResponseDTOs;

namespace ResumeBuilderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResumeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ResponseDTO _reponseDto;
        public ResumeController(IMediator mediator)
        {
            _mediator = mediator;
            _reponseDto = new ResponseDTO();
        }
        [HttpPost("PostNewResume")]
        public async Task<ResponseDTO> PostNewResume([FromForm] PostResumeCommand command) => await _mediator.Send(command);

        [HttpGet("GetAllResumes")]
        public async Task<ResponseDTO> GetAllResumes() => await _mediator.Send(new GetAllResumesQuery());
        [HttpGet("GetResumeById/{Id}")]
        public async Task<ResponseDTO> GetResumeById(Guid Id) => await _mediator.Send(new GetResumeByIdQuery { ResumeId=Id});

        [HttpPut("PutResume")]
        public async Task<ResponseDTO> PutResume([FromForm] PutResumeCommand command) => await _mediator.Send(command);

        [HttpDelete("DeleteResume/{Id}")]
        public async Task<ResponseDTO> DeleteResume(Guid Id) => await _mediator.Send(new DeleteResumeCommand { ResumeId=Id});
    }
}
