<Query Kind="Program" />

// this code is from Derek Finlen, many thanks

void Main()
{
      var xDoc = XDocument.Load(@"C:\temp\ISEEResults-2021-12-05-12-08-14-1.xml"); // loading source xml
      var xmls = xDoc.Root.Elements().ToArray(); // split into elements
      var file = File.CreateText(string.Format(@"C:\temp\ISEEResults-2021-12-05-12-08-14-1-{0}.xml", 0));
      for (int i = 0; i < xmls.Length; i++)
      {
            // write each element into different file
            if (i % 125 == 0)
            {
                  file.WriteLine("</Results>");
                  file.Flush();
                  file.Close();
                  file = File.CreateText(string.Format(@"C:\temp\ISEEResults-2021-12-05-12-08-14-1-{0}.xml", (i/125)));
                  file.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?><Results>");    
            }
            file.Write(xmls[i].ToString());
      }
      file.WriteLine("</Results>");
      file.Flush();
      file.Close();
}