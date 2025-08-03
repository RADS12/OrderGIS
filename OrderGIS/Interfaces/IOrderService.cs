using OrderGIS.Enums;
using OrderGIS.Models;

namespace OrderGIS.Interfaces
{
    public interface IOrderService
    {
        Task<int> CreateOrder(Order order);

        Task<Order> GetOrder(int OrderId);

        Task UpdateOrder(int OrderId, OrderStatus orderStatus);

        Task DeleteOrder(int orderId);
    }
}
