using OrderGIS.Enums;
using OrderGIS.Interfaces;
using OrderGIS.Models;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace OrderGIS.OrderBusinessLogic
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConectionFactory _conectionFactory;

        public OrderRepository(IDbConectionFactory conectionFactory)
        {
            _conectionFactory = conectionFactory;
        }

        public async Task<int> CreateOrder(Order order)
        {
            using var connection = _conectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@CustomerId", order.CustomerId);
            parameters.Add("@GISAssetId", order.GisAssetId);
            parameters.Add("@Status", order.Status);
            parameters.Add("@CreatedDate", DateTime.UtcNow);
            parameters.Add("@UpdatedDate", null);

            await connection.QueryFirstOrDefaultAsync<Order>(
                "sp_CreateOrder",
                parameters,
                commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("NewOrderId");
        }

        public async Task<Order?> GetOrderById(int orderId)
        {
            try
            {
                using var connection = _conectionFactory.CreateConnection();

                //var response = (await connection.Query(
                //    "sp_GetOrderById",
                //    new { OrderId = orderId },
                //    commandType: CommandType.StoredProcedure
                //    )).SingleOrDefault();

                var response = (await connection.QueryAsync(
                    "sp_GetOrderById",
                    new { OrderId = orderId },
                    commandType: CommandType.StoredProcedure
                )).SingleOrDefault();

                if (response != null)
                {
                    Order order = new Order()
                    {
                        OrderId = response.OrderId,
                        CustomerId = response.CustomerId,
                        GisAssetId = response.GisAssetId,
                        Status = response.Status,
                        CreatedDate = response.CreatedDate,
                        UpdatedDate = response.UpdatedDate
                    };

                    return order;
                }

                return null;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteOrderById(int OrderId)
        {
            using var conn = _conectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", OrderId);

            await conn.ExecuteAsync(
                "sp_DeleteOrderById",
                parameters, 
                commandType: CommandType.StoredProcedure);
        }

        public async Task UpdateOrderStatus(int OrderId, OrderStatus status)
        {
            using var conn = _conectionFactory.CreateConnection();
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", OrderId);
            parameters.Add("@status", status);

            await conn.ExecuteAsync(
                "sp_UpdateOrderStatus",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }
}
