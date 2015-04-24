#Example uploader

# Example uploading example #

This example code is the code included in the test site (source code). This example enables the compression settings and uploads a zip file and unzips the file when the file is uploaded.


```
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace DP
{
	public class DPFileSaver : JumploaderWrapper.IFileSaver
	{
		private const string DEFAULT_EXTENSION = "({0})";

		private string _baseDirectory;

		public string BaseDirectory
		{
			get { return _baseDirectory; }
			set { _baseDirectory = value; }
		}

		#region IFileSaver Members

		public bool SaveFile(string fileName, byte[] data, System.Web.HttpRequest request)
		{

			string OriginalFilePath = _baseDirectory + fileName;
			string OriginalFileName = Path.GetFileNameWithoutExtension(OriginalFilePath);
			string OriginalFileExt = Path.GetExtension(OriginalFilePath);
			string NewFilePath = _baseDirectory + request["fileId"] + OriginalFileExt;
			string NewFileName = Path.GetFileNameWithoutExtension(NewFilePath);
			string NewFileExt = Path.GetExtension(OriginalFilePath.Replace(".zip", ""));

			string TmpFilePath = _baseDirectory + Path.GetFileNameWithoutExtension(NewFileName) + ".tmp";
			string TmpIndexName = _baseDirectory + Path.GetFileNameWithoutExtension(NewFileName) + ".idx";

			string index;

			if (File.Exists(TmpIndexName))
			{
				TextReader tReader = new StreamReader(TmpIndexName);
				index = tReader.ReadToEnd();
				tReader.Close();

				if (index == request["partitionIndex"])
				{
					return false;
				}
			}

			// Write partition data to temp file
			BinaryWriter bwtmp = new BinaryWriter(File.Open(TmpFilePath, FileMode.Create));
			bwtmp.Write(data);
			bwtmp.Close();

			// Check to make sure all partition data was written successfully
			Byte[] b = File.ReadAllBytes(TmpFilePath);
			if (int.Parse(request["partitionIndex"]) != int.Parse(request["partitionCount"]) - 1)
			{
				if (b.Length != 1099)
				{
					return false;
				}
			}

			// Write temp bytes to new file
			BinaryWriter bw = new BinaryWriter(File.Open(NewFilePath, FileMode.Append));
			bw.Write(b);
			bw.Close();

			// Save current index to index file
			TextWriter tWriter = new StreamWriter(TmpIndexName);
			tWriter.Write(request["partitionIndex"]);
			tWriter.Close();

			// If file is completed then unzip and cleanup temp files
			if (int.Parse(request["partitionIndex"]) == int.Parse(request["partitionCount"])-1)
			{
				File.Delete(TmpIndexName);
				File.Delete(TmpFilePath);

				FastZip fz = new FastZip();
				fz.ExtractZip(NewFilePath, _baseDirectory, "");
				File.Move(_baseDirectory + OriginalFileName, _baseDirectory + NewFileName + NewFileExt);

				File.Delete(NewFilePath);
			}

			

			return true;
		}

		public string GetUniqueName(string fileName)
		{
			string originalName = fileName.Substring(0, fileName.LastIndexOf("."));
			string originalExt = fileName.Substring(fileName.LastIndexOf(".") + 1);

			string returnName = fileName;
			int increment = 1;
			while (File.Exists(returnName))
			{
				returnName = originalName + string.Format(DEFAULT_EXTENSION, increment++) + "." + originalExt;
			}

			return returnName;
		}

		
		#endregion
	}
}
```

There is more to this example which you will find in the source code, but this is the meat and potatos of the server part of the uploading. The gist of the code is that it gets the partitions that are sent over from the java applet and writes the binary blocks to a temporary file. If the file has completed uploading then it will unzip the file and clean up the temporary files. If the file has not completed uploading you can press the resume button and it will attempt to continue uploading.