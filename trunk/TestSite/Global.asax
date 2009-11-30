<%@ Application Language="C#" %>

<script RunAt="server">

	void Application_Start(object sender, EventArgs e)
	{
    //
    // Below is the example that uses the Database
    //
    //JumploaderWrapper.Samples.DatabaseFileSaver databaseSaver = new JumploaderWrapper.Samples.DatabaseFileSaver();
    //databaseSaver.ConnectionString = "Data Source=(local);Initial Catalog=TestFileUpload;Integrated Security=True";

    //JumploaderWrapper.UploadHandler.UploadComplete += databaseSaver.UploadHandler_UploadComplete;
    //JumploaderWrapper.UploadHandler.FileSaver = databaseSaver;
    
    
    //
    // Below is the example that uses the filesystem.
    //

	//JumploaderWrapper.Samples.FileSystemFileSaver saver = new JumploaderWrapper.Samples.FileSystemFileSaver();
    //saver.BaseDirectory = this.Context.Request.PhysicalApplicationPath + "FileSave\\";
    //JumploaderWrapper.UploadHandler.FileSaver = saver;

		Datapro.DPFileSaver saver = new Datapro.DPFileSaver();
		saver.BaseDirectory = this.Context.Request.PhysicalApplicationPath + "FileSave\\";
		JumploaderWrapper.UploadHandler.FileSaver = saver;
	}

  void Application_End(object sender, EventArgs e)
  {
    //  Code that runs on application shutdown

  }

  void Application_Error(object sender, EventArgs e)
  {
    // Code that runs when an unhandled error occurs

  }

  void Session_Start(object sender, EventArgs e)
  {
    // Code that runs when a new session is started

  }

  void Session_End(object sender, EventArgs e)
  {
    // Code that runs when a session ends. 
    // Note: The Session_End event is raised only when the sessionstate mode
    // is set to InProc in the Web.config file. If session mode is set to StateServer 
    // or SQLServer, the event is not raised.

  }
       
</script>

