using MovieBooking.Web.ApiContracts.Movies;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Interfaces
{
    public interface IMovieMvcService
    {
        Task<List<MovieResponse>> GetMoviesAsync();
        Task<MovieResponse> GetMovieByIdAsync(Guid movieId);
        Task AddMovieAsync(AddMovieViewModel vm);
        Task UpdateMovieAsync(Guid movieId, AddMovieViewModel vm);
        Task DeleteMovieAsync(Guid movieId);
    }
}