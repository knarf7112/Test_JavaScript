using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//
using System.Diagnostics;

namespace Test_JavaScript.App_Code
{
    public class TestRun
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsRun { get; set; }

        static void Main(string[] args)
        {
            

            Debug.WriteLine("Run...");
        }
    }
}