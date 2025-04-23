using System.ComponentModel.DataAnnotations;

namespace CommercialManager.API.Dtos.Users
{
    public class UsersCreateDto
    {

        [Required(ErrorMessage = "El campo {0} es requeido")]
        [StringLength(13, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un minimo de {2} y una maximo de {1} caracteres.")]
        public string DNI { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public int Age { get; set; }
    }
}
