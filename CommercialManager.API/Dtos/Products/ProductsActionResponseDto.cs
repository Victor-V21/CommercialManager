namespace CommercialManager.API.Dtos.Products
{
    public class ProductsActionResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public decimal? Price { get; set; }
    }
}
