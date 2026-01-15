using MovieBooking.Web.ApiContracts.Languages;
using MovieBooking.Web.ApiContracts.Movies;
using MovieBooking.Web.ApiContracts.Theatres;

namespace MovieBooking.Web.ViewModels
{
    public class AddShowTimeBulkViewModel
    {
        public Guid MovieId { get; set; }
        public Guid LanguageId { get; set; }
        //public DateOnly ShowDate {  get; set; }
        // for dropdowns
        public List<MovieResponse> Movies { get; set; } = new();
        public List<LanguageResponse> Languages { get; set; } = new();
        public List<TheatreResponse> Theatres { get; set; } = new();

        public List<ShowTimeItemVm> ShowTimes { get; set; } = new();
    }

}
