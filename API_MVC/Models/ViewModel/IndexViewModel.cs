namespace API_MVC.Models.ViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<NationalPark> NationalParkList { get; set;}
        public IEnumerable<Trail> TrailList { get; set;}
    }
}
