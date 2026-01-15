using MovieBooking.Web.ApiContracts.Languages;
using MovieBooking.Web.ApiContracts.Movies;
using MovieBooking.Web.ApiContracts.Screens;
using MovieBooking.Web.ApiContracts.Theatres;

namespace MovieBooking.Web.ViewModels
{
    public class AddShowTimeViewModel
    {
        public Guid MovieId { get; set; }
        public Guid TheatreId { get; set; }
        public Guid ScreenId { get; set; }
        public Guid LanguageId { get; set; }
        public DateOnly ShowDate { get; set; }
        public decimal BasePrice { get; set; }

        public List<MovieResponse> Movies { get; set; } = new();
        public List<TheatreResponse> Theatres { get; set; } = new();
        public List<ScreenResponse> Screens { get; set; } = new();
        public List<LanguageResponse> Languages { get; set; } = new();
    }

}
