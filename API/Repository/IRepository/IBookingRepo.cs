using API.Models;
using API_MVC.Models;

namespace API.Repository.IRepository
{
    public interface IBookingRepo
    {
        ICollection<Booking> GetBookings();
        Booking GetBooking(int bookingId);
        bool BookingExists(int bookingId);
        bool BookingExists(string bookingName);
        bool CreateBooking(Booking booking);
        bool DeleteBooking(Booking booking);
        bool UpdateeBooking(Booking booking);
        bool Save();
    }
}
