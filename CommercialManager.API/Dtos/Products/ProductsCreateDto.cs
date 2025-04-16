namespace CommercialManager.API.Dtos.Products
{
    public class ProductsCreateDto
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public int Stock { get; set; }
        public decimal? Discount { get; set; }

        public Guid CategoryId { get; set; }
    }
}
