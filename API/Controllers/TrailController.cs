using API.Models;
using API.Models.DTO;
using API.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/trail")]
    [ApiController]
    public class TrailController : Controller
    {
        private readonly ITrail _trail;
        private readonly IMapper _mapper;
        public TrailController(ITrail trail, IMapper mapper)
        {
            _trail = trail;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetTrails()
        {
            var TrailDTOList=_trail.GetTrails().Select(_mapper.Map<Trail, TrailDTO>);
            return Ok(TrailDTOList);
        }
        [HttpGet("{TrailId:int}", Name = "GetTrail")]
        public IActionResult GetTrail(int TrailId)
        {

            var trail = _trail.GetTrail(TrailId);
            if (trail == null) return NotFound();
            var trailSTO = _mapper.Map<Trail, TrailDTO>(trail);
            return Ok(trailSTO);
        }
        [HttpPost]
        public IActionResult CreateTrail([FromBody] TrailDTO TrailDTO)
        {
            if (TrailDTO == null) return BadRequest();
            if (_trail.TrailExists(TrailDTO.Name))
            {
                ModelState.AddModelError("", $"Trail In Use{TrailDTO.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return NotFound();
            var trail = _mapper.Map<Trail>(TrailDTO);
            if (!_trail.CreateTrail(trail))
            {
                ModelState.AddModelError("", $"Kuch To Gadbad Hai{TrailDTO.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            // return Ok(trail);
            return CreatedAtRoute("GetTrail", new { trailid = trail.Id }, trail);
        }
        [HttpPut]
        public IActionResult UpdateTrail([FromBody] TrailDTO TrailDTO)
        {
            if (TrailDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var trail = _mapper.Map<TrailDTO, Trail>(TrailDTO);
            if (!_trail.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"Somethong Went Wrong While Update Data:{trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();//204
        }
        [HttpDelete("{trailId:int}")]
        public IActionResult DeleteTrail(int trailId)
        {
            if (!_trail.TrailExists(trailId))
                return NotFound();
            var trail = _trail.GetTrail(trailId);
            if (trail == null) return NotFound();
            if (!_trail.UpdateTrail(trail))
            {
                ModelState.AddModelError("", $"Somethong Went Wrong While Delete Data:{trail.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
