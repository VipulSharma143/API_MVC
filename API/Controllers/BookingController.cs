using API.Models;
using API.Models.DTO;
using API.Repository.IRepository;
using API_MVC.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepo _bookingRepository;
        private readonly IMapper _mapper;
        public BookingController(IBookingRepo bookingRepository, IMapper mapper)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBookings()
        {
            var BookingDTO = _bookingRepository.GetBookings().Select(_mapper.Map<Booking, BookingDTO>);
            return Ok(BookingDTO);
        }

        [HttpGet("{bookingId:int}", Name = "GetBooking")]
        public IActionResult GetBooking(int bookingId)
        {
            var booking = _bookingRepository.GetBooking(bookingId);
            if (booking == null) return NotFound();
            var BookingDTO = _mapper.Map<BookingDTO>(booking);
            return Ok(BookingDTO);
        }

        [HttpPost]
        public IActionResult CreateBooking([FromBody] BookingDTO BookingDTO)
        {
            if (BookingDTO == null) return BadRequest();

            // Check for model state
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Mapping DTO to Entity
            var booking = _mapper.Map<Booking>(BookingDTO);

            // Creating the booking
            if (!_bookingRepository.CreateBooking(booking))
            {
                // If booking creation fails
                ModelState.AddModelError("", $"Something went wrong");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            // If booking is created successfully
            return CreatedAtRoute("GetBooking", new { bookingId = booking.Id }, booking);
        }
        [HttpPut]
        public IActionResult UpdateBooking(BookingDTO BookingDTO)
        {
            if (BookingDTO == null) return BadRequest();
            if (!ModelState.IsValid) return BadRequest();
            var booking = _mapper.Map<BookingDTO, Booking>(BookingDTO);
            if (!_bookingRepository.UpdateeBooking(booking))
            {
                ModelState.AddModelError("", $"Something  wrong update data");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
        [HttpDelete("{bookingId:int}")]
        public IActionResult DeleteBooking(int bookingId)
        {
            if (!_bookingRepository.BookingExists(bookingId))
                return NotFound();
            var booking = _bookingRepository.GetBooking(bookingId);
            if (booking == null) return NotFound();
            if (!_bookingRepository.DeleteBooking(booking))
            {
                ModelState.AddModelError("", $"Something went wrong");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }
    }
}