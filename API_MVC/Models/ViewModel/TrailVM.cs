using Microsoft.AspNetCore.Mvc.Rendering;

namespace API_MVC.Models.ViewModel
{
    public class TrailVM
    {
        public Trail Trail { get; set; }
        public IEnumerable<SelectListItem>NationalParkList { get; set; }    
    }
}
