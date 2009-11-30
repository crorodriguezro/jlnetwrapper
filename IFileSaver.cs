using System;
using System.Collections.Generic;
using System.Text;

namespace JumploaderWrapper
{
  public interface IFileSaver
  {
    bool SaveFile(string fileName, byte[] data, System.Web.HttpRequest request);
  }
}
