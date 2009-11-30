CREATE DATABASE TestFileUpload
GO

USE TestFileUpload
GO


CREATE TABLE FileHistory
(
  UploadedCount INT NOT NULL,
  DateAdded DATETIME NOT NULL DEFAULT(GETDATE()),
  Successful INT NOT NULL,
  Failed INT NOT NULL,
  IPAddress NVARCHAR(30) NOT NULL
)
GO


CREATE TABLE SaveFileInfo
(
  FileName NVARCHAR(256) NOT NULL PRIMARY KEY,
  FileData VARBINARY(MAX) NOT NULL,
  DateAdded DATETIME NOT NULL DEFAULT(GETDATE())
)
GO



CREATE PROCEDURE AddFileHistory @Count INT, @Successful INT, @Failed INT, @IP NVARCHAR(30)
AS
  INSERT INTO FileHistory (UploadedCount, Successful, Failed, IPAddress)
    VALUES (@Count, @Successful, @Failed, @IP)
GO


CREATE PROCEDURE DoesFileExist @FileName NVARCHAR(512), @Exists BIT OUTPUT
AS
  SELECT @Exists = 1 FROM SaveFileInfo WHERE FileName = @FileName
GO


CREATE PROCEDURE AddFile @FileName NVARCHAR(512), @FileData VARBINARY(MAX)
AS
  INSERT INTO SaveFileInfo (FileName, FileData) VALUES (@FileName, @FileData)
GO
  