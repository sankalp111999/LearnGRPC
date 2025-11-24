using Grpc.Core;
using GRPCAPI.Protos;
using Grpc.Core;
using Google.Protobuf.WellKnownTypes;

namespace GRPCAPI.Services
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly static List<Product> _products = [];

        public override Task<Product?> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = _products.FirstOrDefault(p => p.Id == request.Id);
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product not found."));
            }
            else
            {
                return Task.FromResult<Product?>(product);
            }

        }

        public override Task<ProductListResponse> ListProduct(Empty request, ServerCallContext context)
        {
            var response = new ProductListResponse();
            response.Products.AddRange(_products);
            if (response.Products.Count == 0)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product not found."));
            }
            return Task.FromResult(response);
        }

        public override Task<Product> CreatProduct(CreatProductRequest request, ServerCallContext context)
        {
            var response = new Product
            {
                Id = _products.Count + 1,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };
            _products.Add(response);
            return Task.FromResult(response);
        }

        public override Task<Product> UpdateProduct(UpdateProductRequest request, ServerCallContext context)
        {
            var product = _products.FirstOrDefault(p => p.Id == request.Id);
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product not found."));
            }
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            return Task.FromResult(product);
        }

        public override Task<Empty> DeleteProduct(DeleteProductRequest request, ServerCallContext context)
        {
            var product = _products.FirstOrDefault(p => p.Id == request.Id);
            if (product == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Product not found."));
            }
            _products.Remove(product);
            return Task.FromResult(new Empty());
        }
    }
}
