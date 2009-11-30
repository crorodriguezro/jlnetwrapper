using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Web;
using System.IO;
using System.Net;

namespace JumploaderWrapper
{
  public class ResourceHandler : IHttpHandler
  {
    private const string DEFAULT_LOCATION = "JumploaderWrapper.Resources.";
    internal const string RESOURCE_NAME = "Resource";
    internal const string RESOURCE_TYPE = "Type";

    private HttpRequest _request;
    private HttpResponse _response;

    #region IHttpHandler Members

    public bool IsReusable
    {
      get { return false; }
    }

    public void ProcessRequest(HttpContext context)
    {
      _request = context.Request;
      _response = context.Response;

      string name = _request.Params[RESOURCE_NAME];
      string type = _request.Params[RESOURCE_TYPE];
      if ((name == null) || (type == null))
      {
        if (context.Cache["Name"] != null)
        {
          name = context.Cache["Name"].ToString();
          type = context.Cache["Type"].ToString();
        }
        else
        {
          throw new ArgumentException("To use this handler you must specify the resource to retrieve");
        }
      }

      context.Cache["Name"] = name;
      context.Cache["Type"] = type;


      byte[] data = LoadItem(name);

      _response.Clear();

      switch (type)
      {
        case "applet":
          WriteApplet(data, name);
          break;
        case "jpg":
        case "gif":
        case "png":
          WriteImage(data, type, name);
          break;
        default:
          throw new ArgumentException("Invalid Resource Type Specified.");
      }

      return;
    }

    protected void WriteApplet(byte[] binary, string name)
    {
      //_response.ContentType = "application/octet-stream";
		_response.ContentType = "application/java-archive";
      _response.AddHeader("ContentLength", binary.Length.ToString());
      _response.BinaryWrite(binary);
      _response.Flush();
    }

    protected void WriteImage(byte[] data, string type, string name)
    {
      _response.ContentType = "image/" + type;

      _response.BinaryWrite(data);

      _response.Flush();
    }

    protected byte[] LoadItem(string name)
    {
      byte[] returnData = null;
      using (Stream item = Assembly.GetExecutingAssembly().GetManifestResourceStream(DEFAULT_LOCATION + name))
      {
        returnData = new byte[item.Length];
        item.Read(returnData, 0, (int)item.Length);
      }

      return returnData;
    }

    #endregion
  }
}
