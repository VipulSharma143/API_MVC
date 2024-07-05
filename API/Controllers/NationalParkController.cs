using API.Data;
using API.Models;
using API.Models.DTO;
using API.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace API.Controllers
{
    [Route("api/nationalpark")]
    [ApiController]
    public class NationalParkController : Controller
    {
        private readonly INationalPark _NationalPark;
        private readonly IMapper _mapper;
        public NationalParkController(INationalPark nationalPark, IMapper mapper)
        {
            _NationalPark = nationalPark;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var nationalparkdto = _NationalPark.GetNationalParks().
                Select(_mapper.Map<NationalPark, NationalParkDTO>);
            return Ok(nationalparkdto);//status 200 means success
        }
        [HttpGet("{nationalParkId}" ,Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var NationalPark=_NationalPark.GetNationalPark(nationalParkId);
            if (NationalPark == null) return NotFound(); //404
            var NationalParkDTO = _mapper.Map<NationalParkDTO>(NationalPark);
            return Ok(NationalParkDTO);
        }
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody]NationalParkDTO nationalParkDTO)
        {
            if(nationalParkDTO==null) return BadRequest(); 
            if(_NationalPark.NationalParkExists(nationalParkDTO.Name))
            {
                ModelState.AddModelError("", "National Park In DB");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (!ModelState.IsValid) return BadRequest();
            var NationalPark=_mapper.Map<NationalParkDTO, NationalPark>(nationalParkDTO);
            if(!_NationalPark.CreateNationalPark(NationalPark))
            {
                ModelState.AddModelError("", $"Something Went Wrong While Saving Data: {NationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //return Ok();
            return CreatedAtRoute("GetNationalPark", new { nationalparkId = NationalPark.Id }, NationalPark);
        }
        [HttpPut]
        public IActionResult UpdateNationalPark([FromBody]NationalParkDTO nationalParkDTO)
        {
            if(nationalParkDTO ==null) return BadRequest();
            if(!ModelState.IsValid) return BadRequest();
            var nationalPark = _mapper.Map<NationalParkDTO, NationalPark>(nationalParkDTO);
            if(!_NationalPark.UpdateNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something Went Wrong While Updating Data : {nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent(); //204
        }
        [HttpDelete("{nationalparkId:int}")]
        public IActionResult DeleteNationalPark(int nationalparkId)
        {
            if (!_NationalPark.NationalParkExists(nationalparkId))
                return NotFound();
            var nationalPark=_NationalPark.GetNationalPark(nationalparkId);
            if(nationalPark == null) return NotFound();
            if (!_NationalPark.DeleteNationalPark(nationalPark))
            {
                ModelState.AddModelError("", $"Something Went Wrong While Deleting Data : {nationalPark.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}
