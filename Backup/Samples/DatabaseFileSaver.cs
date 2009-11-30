using System;
using System.Collections.Generic;
using System.Text;

namespace JumploaderWrapper.Samples
{
  public class DatabaseFileSaver : IFileSaver
  {
    private DatabaseDataAccess _dataAccess;
    private string _connectionString;

    public string ConnectionString
    {
      get { return _connectionString; }
      set
      {
        _connectionString = value;
        _dataAccess = new DatabaseDataAccess(_connectionString);
      }
    }

    #region IFileSaver Members

    public bool SaveFile(string fileName, byte[] data)
    {
      if (_dataAccess.DoesFileExist(fileName))
        return false;

      _dataAccess.AddFile(fileName, data);

      return true;
    }

    #endregion

    public void UploadHandler_UploadComplete(object sender, UploadEventArgs args)
    {
      _dataAccess.AddFileHistory(args.TotalInUpload, args.TotalSuccessful, args.TotalFailed,
        args.IPAddress);
    }

  }
}
