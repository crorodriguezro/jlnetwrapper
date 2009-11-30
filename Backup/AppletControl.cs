using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace JumploaderWrapper
{
  public enum AppletAlign
  {
    Left, Right, Middle
  }

  [DefaultProperty("Name")]
  [ToolboxData("<{0}:AppletControl runat=\"server\"></{0}:AppletControl>")]
  public class AppletControl : WebControl
  {
    private const string VARIBALE_VALUE = "{0}=\"{1}\" ";

    protected const string DISPLAY_CATEGORY = "Display";
    protected const string APPLET_CATEGORY = "Applet";

    private string _appletArchive;
    private string _name;
    private string _code;
    private string _hspace;
    private string _vspace;
    private AppletAlign _align;
    private bool _mayScript;
    private bool _useArchive;

    private Dictionary<string, string> _variables;

    [Category(AppletControl.DISPLAY_CATEGORY)]
    [DefaultValue("Applet")]
    public string Name
    {
      get { return _name; }
      set { _name = value; }
    }

    [Category(AppletControl.APPLET_CATEGORY)]
    [DefaultValue("")]
    public string Archive
    {
      get { return _appletArchive; }
      set
      {
        _appletArchive = value;
        if (!string.IsNullOrEmpty(_appletArchive))
          _useArchive = true;
      }
    }

    [Category(AppletControl.DISPLAY_CATEGORY)]
    [DefaultValue("Middle")]
    public AppletAlign Align
    {
      get { return _align; }
      set { _align = value; }
    }

    [Category(AppletControl.APPLET_CATEGORY)]
    [DefaultValue("com.some.applet")]
    public string Code
    {
      get { return _code; }
      set { _code = value; }
    }

    [Category(AppletControl.APPLET_CATEGORY)]
    [DefaultValue(false)]
    public bool MayScript
    {
      get { return _mayScript; }
      set { _mayScript = value; }
    }

    [Category(AppletControl.APPLET_CATEGORY)]
    [DefaultValue("0")]
    public string HSpace
    {
      get { return _hspace; }
      set { _hspace = value; }
    }

    [Category(AppletControl.APPLET_CATEGORY)]
    [DefaultValue("0")]
    public string VSpace
    {
      get { return _vspace; }
      set { _vspace = value; }
    }


    public Dictionary<string, string> AppletParams
    {
      get { return _variables; }
      set { _variables = value; }
    }


    public AppletControl()
    {
      _variables = new Dictionary<string, string>();
    }

    private void WriteParamValue(HtmlTextWriter writer, string key, string value)
    {
      writer.Write(string.Format("<param " + VARIBALE_VALUE, "name", key));
      writer.Write(string.Format(VARIBALE_VALUE + "/>", "value", value));
    }

    protected override void RenderContents(HtmlTextWriter writer)
    {
      if (_code.Contains("~"))
        _code = ResolveClientUrl(_code);
      if (_appletArchive.Contains("~"))
        _appletArchive = ResolveClientUrl(_appletArchive);

      string closingTag = "";

      if (Page.Request.Browser.Win32)
      {
        if ((Page.Request.Browser.Browser == "IE") || (Page.Request.Browser.Browser == "MSIE"))
        {
          writer.Write("<object classid=\"clsid:8AD9C840-044E-11D1-B3E9-00805F499D93\" ");
          writer.Write(VARIBALE_VALUE, "width", Width.ToString());
          writer.Write(VARIBALE_VALUE, "height", Height.ToString());
          writer.Write(VARIBALE_VALUE + ">", "codebase",
            "http://java.sun.com/update/1.5.0/jinstall-1_5-windows-i586.cab#version=1,4,1");
          writer.Write(">");
        }
        else
        {
          writer.Write("<object type=\"application/x-java-applet;version=1.4.1\" ");
          writer.Write(VARIBALE_VALUE, "width", Width.ToString());
          writer.Write(VARIBALE_VALUE + ">", "height", Height.ToString());
        }

        if (_useArchive)
          WriteParamValue(writer, "archive", _appletArchive);

        WriteParamValue(writer, "code", _code);
        WriteParamValue(writer, "name", _name);
        WriteParamValue(writer, "align", _align.ToString());

        if (!string.IsNullOrEmpty(_hspace))
          WriteParamValue(writer, "hspace", _hspace);
        if (!string.IsNullOrEmpty(_vspace))
          WriteParamValue(writer, "vspace", _vspace);

        if (_mayScript)
          WriteParamValue(writer, "mayscript", "yes");

        closingTag = "</object>";
      }
      else
      {
        writer.Write("<applet ");
        if (_useArchive)
          writer.Write(VARIBALE_VALUE, "archive", _appletArchive);

        writer.Write(VARIBALE_VALUE, "code", _code);
        writer.Write(VARIBALE_VALUE, "name", _name);

        if (_mayScript)
          writer.Write(VARIBALE_VALUE, "mayscript", "yes");

        if (!string.IsNullOrEmpty(_hspace))
          writer.Write(VARIBALE_VALUE, "hspace", _hspace);
        if (!string.IsNullOrEmpty(_vspace))
          writer.Write(VARIBALE_VALUE, "vspace", _vspace);

        writer.Write(VARIBALE_VALUE, "align", _align.ToString());

        writer.Write(VARIBALE_VALUE, "width", Width.ToString());
        writer.Write(VARIBALE_VALUE + ">", "height", Height.ToString());

        closingTag = "</applet>";
      }

      foreach (string key in _variables.Keys)
        WriteParamValue(writer, key, _variables[key]);

      writer.Write(closingTag);

      base.RenderContents(writer);
    }
  }
}
