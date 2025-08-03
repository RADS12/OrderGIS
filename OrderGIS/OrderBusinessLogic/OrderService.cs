using OrderGIS.Enums;
using OrderGIS.Interfaces;
using OrderGIS.Models;

namespace OrderGIS.OrderBusinessLogic
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(5);
        private readonly object _locker = new object();

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<int> CreateOrder(Order order)
        {
            await _semaphoreSlim.WaitAsync();

            try
            {
                lock (_locker)
                {

                };

                order.CreatedDate = DateTime.UtcNow;
                var result = await _orderRepository.CreateOrder(order);

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { 
                _semaphoreSlim.Release();
            }
        }

        public async Task<Order> GetOrder(int OrderId)
        {
            await _semaphoreSlim.WaitAsync();

            try
            {
                var response = await _orderRepository.GetOrderById(OrderId);
                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _semaphoreSlim?.Release();
            }
        }

        public async Task DeleteOrder(int orderId)
        {
            await _orderRepository.DeleteOrderById(orderId);
        }

        public async Task UpdateOrder(int OrderId, OrderStatus orderStatus)
        {
            await _orderRepository.UpdateOrderStatus(OrderId, orderStatus);
        }
    }
}
