using MovieBooking.Web.ApiContracts.Languages;
using MovieBooking.Web.ViewModels;

namespace MovieBooking.Web.Interfaces
{
    public interface ILanguageMvcService
    {
        Task<List<LanguageResponse>> GetLanguagesAsync();
        Task<LanguageResponse> GetLanguageByIdAsync(Guid languageId);
        Task AddLanguageAsync(string name);
        Task UpdateLanguageAsync(Guid languageId, string name);
        Task DeleteLanguageAsync(Guid languageId);
    }
}