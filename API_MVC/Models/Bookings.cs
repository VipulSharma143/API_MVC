﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_MVC.Models
{
    public class Bookings
    {
        public Bookings()
        {
            AdultPrice = 500;
            ChildPrice = 300;
            NumberOfAdults = 1;
            NumberOfChildren = 1;
            Count = (NumberOfAdults*AdultPrice) + (NumberOfChildren*ChildPrice);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Number Of Person")]
        public int NumberOfPersons { get; set; }
        [Display(Name = "Booking Status")]
        public bool BookingStatus { get; set; }
        [Display(Name = "Total")]
        public int Count { get; set; }
        [NotMapped]
        public int AdultPrice { get; set; }
        [NotMapped]
        public int ChildPrice { get; set; }
        public int NumberOfAdults {  get; set; }
        public int NumberOfChildren {  get; set; }
    }
}
