namespace MovieBooking.Web.ApiContracts.Admin
{
    public class CreateAdminDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool IsActive { get; set; }
    }
}
