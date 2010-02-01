using System;
using System.Text;
using System.IO;

namespace md2html
{
    public class PostProcessor
    {
        public static string Process(string html, Configuration configuration)
        {
            StringBuilder result = new StringBuilder();
            result.Append("<html>");
            result.Append("<head>");
            if (configuration.Title == string.Empty)
                configuration.Title = Path.GetFileName(configuration.InputFileName);

            result.Append("<title>" + configuration.Title + "</title>");
            result.Append("</head>");
            result.Append("<body>");
            result.Append(html);
            result.Append("</body>");
            result.Append("</html>");
            return result.ToString();
        }
    }
}
