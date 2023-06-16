namespace RentCarsAPI.Models.User
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string HashPassword { get; set; }
    }
}
