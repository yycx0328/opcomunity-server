using Utility.DataAccess;

class LogDbCommon
{
    public static string LOG_CONNECTION_STRING
    {
        get
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["opc_log"].ConnectionString;
        }
    }

    public static SqlAccessCommand CreateSqlCommand()
    {
        return new SqlAccessCommand(LOG_CONNECTION_STRING);
    }
}