using OnlineShop.Models;

namespace OnlineShop.Interfaces
{
    public interface IOrderShippingService
    {
        Task<string> CreateOrderShippingAsync(ShippingOrderModel model);
    }
}
