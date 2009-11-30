using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace test
{
/// <summary>
/// Summary description for upload
/// </summary>
public class upload : IHttpHandler
{
	private static JumploaderWrapper.IFileSaver _fileSaver = null;
	private static event JumploaderWrapper.FileEventHandler _fileSavedEvent;
	private static event JumploaderWrapper.UploadEventHandler _uploadedEvent;

	public static event JumploaderWrapper.UploadEventHandler UploadComplete
	{
		add { _uploadedEvent += value; }
		remove { _uploadedEvent -= value; }
	}

	public static event JumploaderWrapper.FileEventHandler FileSaved
	{
		add { _fileSavedEvent += value; }
		remove { _fileSavedEvent -= value; }
	}

	public static JumploaderWrapper.IFileSaver FileSaver
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

	public bool IsReusable
	{
		get { return false; }
	}

	public upload()
	{
		//
		// TODO: Add constructor logic here
		//
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
            _fileSavedEvent(this, new JumploaderWrapper.FileSavedEventArgs(file.FileName, file.ContentLength, fileTemp));

          successful += 1;
        }
        catch
        {
          failed += 1;
        }
      }

      if (_uploadedEvent != null)
        _uploadedEvent(this, new JumploaderWrapper.UploadEventArgs(ipAddress, total, successful, failed));
    }
}
}
