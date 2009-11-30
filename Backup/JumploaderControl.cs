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
  public enum JumploaderLookAndFeel
  {
    System, CrossPlatform
  }

  [DefaultProperty("Name")]
  [ToolboxData("<{0}:JumploaderControl runat=\"server\"></{0}:JumploaderControl>")]
  public class JumploaderControl : AppletControl
  {
    private const string DEFAULT_ARCHIVE = "~/JumploaderResource.asx?Resource=jumploader_z.jar&Type=applet";
    //private const string DEFAULT_ARCHIVE = "~/jumploader_z.jar";
    private const string DEFAULT_CODE = "jmaster.jumploader.app.JumpLoaderApplet.class";

    private const string UPLOADER_CONFIG_CATEGORY = "UploaderConfig";
    private const string VIEW_CONFIG_CATEGORY = "ViewConfig";
    private const string APPLET_CONFIG_CATEGORY = "AppletConfig";

    // uc_
    private const string UPLOAD_URL_NAME = "uc_uploadURL";
    private const string MAX_FILE_LENGTH_NAME = "uc_maxFileLength";
    private const string DIRECTORY_ENABLED = "uc_directoryEnabled";
    private const string DUPLICATE_FILE_ENABLED = "uc_duplicateFileEnabled";
    private const string MAX_FILE_LIST = "uc_maxFiles";
    private const string UPLOAD_THREAD_COUNT = "uc_uploadThreadCount";

    // vc_
    private const string LOOK_AND_FEEL = "vc_lookAndFeel";
    private const string VIEW_FILE_LIST_VISIBLE = "vc_mainViewFileListViewVisible";
    private const string VIEW_FILE_TREE_VISIBLE = "vc_mainViewFileTreeViewVisible";
    private const string VIEW_LOGO_ENABLED = "vc_mainViewLogoEnabled";

    public JumploaderControl()
    {
      Archive = DEFAULT_ARCHIVE;
      Code = DEFAULT_CODE;

      Width = 600;
      Height = 400;
      MayScript = true;
      Align = AppletAlign.Middle;
    }


    [Category(JumploaderControl.VIEW_CONFIG_CATEGORY)]
    [DefaultValue(true)]
    public bool ViewLogo
    {
      get
      {
        if (!AppletParams.ContainsKey(VIEW_LOGO_ENABLED))
          return true;
        else
          return Convert.ToBoolean(AppletParams[VIEW_LOGO_ENABLED]);
      }
      set
      {
        AppletParams[VIEW_LOGO_ENABLED] = value.ToString();
      }
    }

    [Category(JumploaderControl.VIEW_CONFIG_CATEGORY)]
    [DefaultValue(true)]
    public bool FileTreeVisible
    {
      get
      {
        if (!AppletParams.ContainsKey(VIEW_FILE_TREE_VISIBLE))
          return true;
        else
          return Convert.ToBoolean(AppletParams[VIEW_FILE_TREE_VISIBLE]);
      }
      set
      {
        AppletParams[VIEW_FILE_TREE_VISIBLE] = value.ToString();
      }
    }


    [Category(JumploaderControl.VIEW_CONFIG_CATEGORY)]
    [DefaultValue(true)]
    public bool FileListVisible
    {
      get
      {
        if (!AppletParams.ContainsKey(VIEW_FILE_LIST_VISIBLE))
          return true;
        else
          return Convert.ToBoolean(AppletParams[VIEW_FILE_LIST_VISIBLE]);
      }
      set
      {
        AppletParams[VIEW_FILE_LIST_VISIBLE] = value.ToString();
      }
    }



    [Category(JumploaderControl.VIEW_CONFIG_CATEGORY)]
    public JumploaderLookAndFeel LookAndFeel
    {
      get 
      {
        if (!AppletParams.ContainsKey(LOOK_AND_FEEL))
        {
          return JumploaderLookAndFeel.CrossPlatform;
        }
        else
        {
          switch (AppletParams[LOOK_AND_FEEL])
          {
            case "system":
              return JumploaderLookAndFeel.System;
            case "crossPlatform":
              return JumploaderLookAndFeel.CrossPlatform;
            default:
              throw new ArgumentException("Invalid Value found in look and Feel");
          }
        }
      }
      set
      {
        switch (value)
        {
          case JumploaderLookAndFeel.System:
            AppletParams[LOOK_AND_FEEL] = "system";
            break;
          case JumploaderLookAndFeel.CrossPlatform:
            AppletParams[LOOK_AND_FEEL] = "crossSystem";
            break;
          default:
            throw new ArgumentException("If you got this you really screwed up, invalid value for look and feel.");
        }
      }
    }
         

    [Category(JumploaderControl.UPLOADER_CONFIG_CATEGORY)]
    [DefaultValue(1)]
    public int UploadThreadCount
    {
      get
      {
        if (!AppletParams.ContainsKey(UPLOAD_THREAD_COUNT))
          return 1;
        else
          return Convert.ToInt32(AppletParams[UPLOAD_THREAD_COUNT]);
      }
      set
      {
        AppletParams[UPLOAD_THREAD_COUNT] = value.ToString();
      }
    }

    [Category(JumploaderControl.UPLOADER_CONFIG_CATEGORY)]
    [DefaultValue(-1)]
    public int MaxFiles
    {
      get
      {
        if (!AppletParams.ContainsKey(MAX_FILE_LIST))
          return -1;
        else
          return Convert.ToInt32(AppletParams[MAX_FILE_LIST]);
      }
      set
      {
        AppletParams[MAX_FILE_LIST] = value.ToString();
      }
    }

    [Category(JumploaderControl.UPLOADER_CONFIG_CATEGORY)]
    [DefaultValue(false)]
    public bool DuplicateFileEnabled
    {
      get
      {
        if (!AppletParams.ContainsKey(DUPLICATE_FILE_ENABLED))
          return false;
        else
          return Convert.ToBoolean(AppletParams[DUPLICATE_FILE_ENABLED]);
      }
      set
      {
        AppletParams[DUPLICATE_FILE_ENABLED] = value.ToString();
      }
    }

    [Category(JumploaderControl.UPLOADER_CONFIG_CATEGORY)]
    [DefaultValue("")]
    public string UploadURL
    {
      get { return AppletParams[UPLOAD_URL_NAME]; }
      set { AppletParams[UPLOAD_URL_NAME] = value; }
    }

    [Category(JumploaderControl.UPLOADER_CONFIG_CATEGORY)]
    [DefaultValue(-1)]
    public int MaxFileLength
    {
      get
      {
        if (!AppletParams.ContainsKey(MAX_FILE_LENGTH_NAME))
          return -1;
        else
          return Convert.ToInt32(AppletParams[MAX_FILE_LENGTH_NAME]);
      }
      set
      {
        AppletParams[MAX_FILE_LENGTH_NAME] = value.ToString();
      }
    }

    [Category(JumploaderControl.UPLOADER_CONFIG_CATEGORY)]
    [DefaultValue(false)]
    public bool DirectoryEnabled
    {
      get
      {
        if (!AppletParams.ContainsKey(DIRECTORY_ENABLED))
          return false;
        else
          return Convert.ToBoolean(AppletParams[DIRECTORY_ENABLED]);
      }
      set
      {
        AppletParams[DIRECTORY_ENABLED] = value.ToString();
      }
    }
  }
}
