using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace JumploaderWrapper
{
  public delegate void FileEventHandler(object sender, FileSavedEventArgs args);
  public delegate void UploadEventHandler(object sender, UploadEventArgs args);

  public class UploadHandler : IHttpHandler
  {
    private static IFileSaver _fileSaver = null;
    private static event FileEventHandler _fileSavedEvent;
    private static event UploadEventHandler _uploadedEvent;

    public static event UploadEventHandler UploadComplete
    {
      add { _uploadedEvent += value; }
      remove { _uploadedEvent -= value; }
    }

    public static event FileEventHandler FileSaved
    {
      add { _fileSavedEvent += value; }
      remove { _fileSavedEvent -= value; }
    }

    public static IFileSaver FileSaver
    {
      get { return _fileSaver; }
      set { _fileSaver = value; }
    }

    private HttpRequest _request;
    private HttpResponse _response;

    public HttpRequest Request
    {
      get { return _request; }
    }

    public HttpResponse Response
    {
      get { return _response; }
    }

    #region IHttpHandler Members

    public bool IsReusable
    {
      get { return false; }
    }

    public void ProcessRequest(HttpContext context)
    {
      _request = context.Request;
      _response = context.Response;

      if (context.Request.Files.Count <= 0)
      {
        _response.ContentType = "text/plain";
        _response.Write("NO FILE!");
        
        return;
      }

      SaveFiles();

      _response.ContentType = "text/plain";
      _response.Write("Upload OK");
    }

    protected void SaveFiles()
    {
      int failed = 0;
      int successful = 0;
      int total = _request.Files.Count;
      string ipAddress = _request.UserHostAddress;

      for (int i = 0; i < _request.Files.Count; ++i)
      {
        HttpPostedFile file = _request.Files[i];

        try
        {
          byte[] fileTemp = new byte[file.InputStream.Length];
          file.InputStream.Read(fileTemp, 0, (int)file.InputStream.Length);

          if (FileSaver != null)
            FileSaver.SaveFile(file.FileName, fileTemp, Request);

          if (_fileSavedEvent != null)
            _fileSavedEvent(this, new FileSavedEventArgs(file.FileName, file.ContentLength, fileTemp));

          successful += 1;
        }
        catch
        {
          failed += 1;
        }
      }

      if (_uploadedEvent != null)
        _uploadedEvent(this, new UploadEventArgs(ipAddress, total, successful, failed));
    }

    #endregion
  }
}
