using GroupAssignment3.Data;
using GroupAssignment3.Dto;
using GroupAssignment3.Exception;

namespace GroupAssignment3.Services
{
    public class ProductService
    {

        private readonly DatabaseContext context;

        public ProductService(DatabaseContext context)
        {
            this.context = context;
        }

        public ProductEntity CreateProduct(CreateProductDto productDto)
        {
            ProductEntity product = new ProductEntity();
            product.productId = Guid.NewGuid().ToString();
            product.pricing = productDto.pricing;
            product.image = productDto.image;
            product.description = productDto.description;
            product.shippingCost = productDto.shippingCost;
            context.Add(product);
            context.SaveChanges();
            return product;
        }

        public ProductEntity GetProductById(string idProduct)
        {
            ProductEntity filteredProduct = context.Product.Where(p => p.productId == idProduct).FirstOrDefault();
            if (filteredProduct == null)
            {
                throw new HttpException(404, "Product ID '" + idProduct + "' does not exists");
            }
            return filteredProduct;
        }

        public List<ProductEntity> GetAllProducts()
        {
            return context.Product.ToList();
        }

    }
}
