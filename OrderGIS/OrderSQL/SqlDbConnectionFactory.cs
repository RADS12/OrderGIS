using OrderGIS.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace OrderGIS.OrderSQL
{
    public class SqlDbConnectionFactory : IDbConectionFactory, IDisposable
    {
        private bool _disposed;
        private readonly string _connString;

        public SqlDbConnectionFactory(IConfiguration configuartion)
        {
            _connString = configuartion.GetConnectionString("GisOrderDbConnectionString");
        }

        public IDbConnection CreateConnection()
        {
            var conn = new SqlConnection(_connString);
            conn.Open();

            return conn;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
        }
    }
}