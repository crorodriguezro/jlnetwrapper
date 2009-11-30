<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="JumploaderWrapper" Namespace="JumploaderWrapper" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    
      <cc1:JumploaderControl CompressionMode="zipOnAdd" UploadURL="~/JumploaderUpload.asx" PartitionLength="1099" MetaDescriptorURL="meta.xml" UseMetaData="true" runat="server" EnableDuplicateFiles="false" AutoRetryCount="5" EnableImageEditor="true" >
      </cc1:JumploaderControl>
      
    </div>
    </form>
</body>
</html>
