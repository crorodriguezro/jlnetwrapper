using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JumploaderWrapper.Samples
{
  public class FileSystemFileSaver : IFileSaver
  {
    private const string DEFAULT_EXTENSION = "({0})";

    private string _baseDirectory;

    public string BaseDirectory
    {
      get { return _baseDirectory; }
      set { _baseDirectory = value; }
    }

    #region IFileSaver Members

    public bool SaveFile(string fileName, byte[] data)
    {
      fileName = _baseDirectory + fileName;

      string uniqueName = GetUniqueName(fileName);

      using (FileStream f = new FileStream(uniqueName, FileMode.CreateNew))
      {
        f.Write(data, 0, data.Length);
      }

      return true;
    }

    public string GetUniqueName(string fileName)
    {
      string originalName = fileName.Substring(0, fileName.LastIndexOf("."));
      string originalExt = fileName.Substring(fileName.LastIndexOf(".")+1);

      string returnName = fileName;
      int increment = 1;
      while (File.Exists(returnName))
      {
        returnName = originalName + string.Format(DEFAULT_EXTENSION, increment++) + originalExt;
      }

      return returnName;
    }

    #endregion
  }
}
