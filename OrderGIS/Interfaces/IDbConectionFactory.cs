using System.Data;

namespace OrderGIS.Interfaces
{
    public interface IDbConectionFactory
    {
        IDbConnection CreateConnection();
    }
}
