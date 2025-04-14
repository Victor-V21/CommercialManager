using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommercialManager.API.Dtos.Categories
{
    public class CategoryCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
