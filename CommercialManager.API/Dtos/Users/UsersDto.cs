using System.ComponentModel.DataAnnotations;

namespace CommercialManager.API.Dtos.Users
{
    public class UsersDto
    {
        public Guid Id { get; set; }

        public string DNI { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
    }
}
