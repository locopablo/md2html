using MarkdownSharp;
using System;
namespace md2html
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Markdown md = new Markdown();
            Console.Out.WriteLine(md.Version);
        }
    }
}
