using OrderGIS.Enums;
using OrderGIS.Models;

namespace OrderGIS.Interfaces
{
    public interface IOrderRepository
    {
        public Task<int> CreateOrder(Order order);
        public Task<Order> GetOrderById(int OrderId);
        public Task UpdateOrderStatus(int OrderId, OrderStatus OrderStatus);
        public Task DeleteOrderById(int OrderId);
    }
}
