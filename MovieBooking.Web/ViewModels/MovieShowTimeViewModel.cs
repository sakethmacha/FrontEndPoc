using MovieBooking.Web.ApiContracts.ShowTimes;

public class MovieShowTimeViewModel
{
    public string MovieTitle { get; set; } = string.Empty;
    public List<ShowTimeResponse> ShowTimes { get; set; } = new();
}
