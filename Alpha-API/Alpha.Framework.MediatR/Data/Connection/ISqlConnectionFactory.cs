using System.Data;

namespace Alpha.Framework.MediatR.Data.Connection
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}
