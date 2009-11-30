using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;

namespace JumploaderWrapper.Samples
{
  internal delegate void CommandExecuteContent();
  internal delegate void CommandExecuteReader<T>(List<T> list, SqlDataReader reader);

  public class DatabaseDataAccess
  {
    private const string ADD_FILE_HISTORY = "AddFileHistory";
    private const string DOES_FILE_EXIST = "DoesFileExist";
    private const string ADD_FILE = "AddFile";

    private static readonly string DEFAULT_CONNECTION;

    static DatabaseDataAccess()
    {
      DEFAULT_CONNECTION = ConfigurationManager.AppSettings["DBConnection"];
    }

    private SqlConnection _connection;

    public DatabaseDataAccess() 
      : this(DEFAULT_CONNECTION)
    {
    }

    public DatabaseDataAccess(string connection)
    {
      if (connection != null)
        _connection = new SqlConnection(connection);
    }

    #region Helper Functions

    internal int RunExecuteNonQuery(SqlCommand command)
    {
      return RunExecuteNonQuery(command, null);
    }

    internal int RunExecuteNonQuery(SqlCommand command, CommandExecuteContent functionYield)
    {
      int returnCode = -1;
      ConnectionState previousState = command.Connection.State;
      try
      {
        if (previousState != ConnectionState.Open)
          command.Connection.Open();

        returnCode = command.ExecuteNonQuery();

        if (functionYield != null)
          functionYield();
      }
      finally
      {
        if (previousState != ConnectionState.Open)
          command.Connection.Close();
      }

      return returnCode;
    }

    #endregion

    public void AddFileHistory(int count, int successful, int failed, string ip)
    {
      SqlCommand command = new SqlCommand(ADD_FILE_HISTORY, _connection);
      command.CommandType = CommandType.StoredProcedure;

      command.Parameters.Add("@Count", SqlDbType.Int).Value = count;
      command.Parameters.Add("@Successful", SqlDbType.Int).Value = successful;
      command.Parameters.Add("@Failed", SqlDbType.Int).Value = failed;
      command.Parameters.Add("@IP", SqlDbType.NVarChar, 30).Value = ip;

      RunExecuteNonQuery(command);
    }

    public bool DoesFileExist(string fileName)
    {
      SqlCommand command = new SqlCommand(DOES_FILE_EXIST, _connection);
      command.CommandType = CommandType.StoredProcedure;

      command.Parameters.Add("@FileName", SqlDbType.NVarChar, 256).Value = fileName;

      SqlParameter existParam = new SqlParameter("@Exists", SqlDbType.Bit);
      existParam.Direction = ParameterDirection.Output;
      command.Parameters.Add(existParam);

      bool returnValue = false;
      RunExecuteNonQuery(command, delegate()
      {
        if (existParam.Value != DBNull.Value)
          returnValue = Convert.ToBoolean(existParam.Value);
      });

      return returnValue;
    }

    public void AddFile(string fileName, byte[] data)
    {
      SqlCommand command = new SqlCommand(ADD_FILE, _connection);
      command.CommandType = CommandType.StoredProcedure;

      command.Parameters.Add("@FileName", SqlDbType.NVarChar, 256).Value = fileName;
      command.Parameters.Add("@FileData", SqlDbType.VarBinary, int.MaxValue).Value = data;

      RunExecuteNonQuery(command);
    }
  }
}
