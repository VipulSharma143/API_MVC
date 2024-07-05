using API.Models;
using API.Models.DTO;
using API.Repository;
using API.Repository.IRepository;
using API_MVC.Models;
using AutoMapper;

namespace API.DTOMapping
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<NationalParkDTO, NationalPark>().ReverseMap();
            CreateMap<Trail, TrailDTO>().ReverseMap();
            CreateMap<Booking, BookingDTO>().ReverseMap();
        }
    }
}
//DB----Model---Repository--DTO---Controller(Client)
//Vice Versa 