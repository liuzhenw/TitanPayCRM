using System.Threading.Tasks;
using Astra;

namespace Crm.Products;

public interface IProductRepository:IAstraBasicRepository<Product,string>
{
    Task UpdateStatisticAsync();
}