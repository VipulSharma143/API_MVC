using API_MVC.Models;
using API_MVC;
using Microsoft.AspNetCore.Mvc;
using API_MVC.Repository.IRepository;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Stripe;

namespace NP_APP.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepo _bookingRepository;
        private readonly ITrailParkRepository _trailParkRepository;

        public BookingController(IBookingRepo bookingRepository, ITrailParkRepository trailParkRepository)
        {
            _bookingRepository = bookingRepository;
            _trailParkRepository = trailParkRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdateCount(int ?bookingId, int numberOfAdults, int numberOfChildren)
        {
            Bookings booking = new Bookings();
            // Fetch the booking from the database
            var bookings = _bookingRepository.GetAsync(SD.BookingAPIPath, bookingId.GetValueOrDefault());

            if (booking == null)
            {
                return NotFound(); // Or handle the case where booking is not found
            }
            
            // Update the number of adults and children
            booking.NumberOfAdults = numberOfAdults;
            booking.NumberOfChildren = numberOfChildren;

            // Recalculate the total count based on new values
            booking.Count = (booking.NumberOfAdults * booking.AdultPrice) + (booking.NumberOfChildren * booking.ChildPrice);

            // Save changes to the database
            _bookingRepository.GetAsync(SD.BookingAPIPath, bookingId.GetValueOrDefault());

            // You might want to return something indicating success, or redirect somewhere
            return RedirectToAction("Index"); // Redirect to index or any other action
        }

        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookingList = await _bookingRepository.GetAllAsync(SD.BookingAPIPath);
            return Json(new { data = bookingList });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _bookingRepository.DeleteAsync(SD.BookingAPIPath, id);
            if (status)
                return Json(new { success = true, mseeage = "Deleted successfully" });
            return Json(new { success = true, mseeage = "Deleted wrong" });

        }
        #endregion
        public async Task<IActionResult> Upsert(int? id, Bookings bookings)
        {
            Bookings booking = new Bookings(); // Create a new Booking object
            if (id == null)
            {
                // This is for adding a new booking
                booking.Date = DateTime.Today; // Set the date field to the current date
                return View(booking);
            }

            // This is for updating an existing booking
            booking = await _bookingRepository.GetAsync(SD.BookingAPIPath, id.GetValueOrDefault());
            if (booking == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                // Assume payment validation logic here

                if (bookings.Id == 0)
                {
                    // This is for adding a new booking
                    TempData["BookingData"] = bookings; // Store booking data in TempData
                    return RedirectToAction(nameof(ProcessPayment)); // Redirect to payment processing
                }
                else
                {
                    // This is for updating an existing booking
                    await _bookingRepository.UpdateAsync(SD.BookingAPIPath, bookings);
                    return RedirectToAction(nameof(Index)); // Redirect to booking list
                }
            }
            return View(bookings);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Bookings booking)
        {
            if (ModelState.IsValid)
            {
                // Get all bookings
                var allBookings = await _bookingRepository.GetAllAsync(SD.BookingAPIPath);

                // Group bookings by date and count the bookings for each date
                var bookingsGroupedByDate = allBookings.GroupBy(b => b.Date.Date);

                // Check if any date has reached the maximum booking limit
                foreach (var group in bookingsGroupedByDate)
                {
                    if (group.Count() >= 2 && group.Key == booking.Date.Date)
                    {
                        // Maximum booking limit reached for the selected date
                        ModelState.AddModelError("", "If you continue with this date, please note that only two bookings are available for this date. If you wish to proceed with the booking, kindly select another date.");
                        return View(booking);
                    }
                    else if (group.Count() > 2)
                    {
                        // Maximum booking limit reached for one or more dates
                        ModelState.AddModelError("", "Maximum booking limit reached for one or more dates.");
                        return View(booking);
                    }
                }

                // This is for adding a new booking or updating an existing booking
                await _bookingRepository.CreateAsync(SD.BookingAPIPath, booking);
            }
            return RedirectToAction(nameof(PaymentSuccessful));
        }

        public async Task<bool> ProcessPayment(Bookings booking, string stripeToken)
        {
            // Your Stripe API keys
            StripeConfiguration.ApiKey = "sk_test_your_stripe_secret_key";

            // Create a PaymentIntent to charge the user
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(booking.Count * 100), // Stripe expects amount in cents
                Currency = "usd", // Change currency as per your requirement
                PaymentMethodTypes = new List<string> { "card" },
                Description = "Booking payment",
                ReceiptEmail = booking.Email, // Email of the customer
                PaymentMethod = stripeToken // Token obtained from Stripe.js
            };

            var service = new PaymentIntentService();
            try
            {
                var paymentIntent = await service.CreateAsync(options);
                if (paymentIntent.Status == "succeeded")
                {
                    // Payment successful
                    return true;
                }
                else
                {
                    // Payment failed
                    return false;
                }
            }
            catch (StripeException ex)
            {
                // Handle Stripe exceptions
                Console.WriteLine("Stripe Exception: " + ex.Message);
                return false;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PaymentSuccessful()
        {
            string accountSid = "";
            string authToken = "";

            TwilioClient.Init(accountSid, authToken);

            string fromPhoneNumber = "+";

            string toPhoneNumber = "";

            try
            {
                var message = MessageResource.Create(
                    body: "Your payment was successful! Thank you for your order.",
                    from: new PhoneNumber(fromPhoneNumber),
                    to: new PhoneNumber(toPhoneNumber)
                );

                Console.WriteLine("Message SID: " + message.Sid);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
            return View(ProcessPayment;
        }
    }
}
    