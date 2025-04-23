using System.ComponentModel.DataAnnotations;

namespace CommercialManager.API.Dtos.Products
{
    public class ProductsCreateDto
    {
        [Display(Name = "Nombre del producto")]
        [Required(ErrorMessage = "El campo {0} es requeido")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un minimo de {2} y una maximo de {1} caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Descripcion del producto")]
        [Required(ErrorMessage = "El campo {0} es requeido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener un minimo de {2} y una maximo de {1} caracteres.")]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public decimal? Discount { get; set; }
        public Guid CategoryId { get; set; }
    }
}
 