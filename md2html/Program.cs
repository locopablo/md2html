﻿using MarkdownSharp;
using System;

namespace md2html
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Configuration configuration = new Configuration();
            CommandLineArguments.Configure(configuration, args);
        }
    }
}
