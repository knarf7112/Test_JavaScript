using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
//
using System.Xml;
using System.Xml.Linq;
using System.Dynamic;

namespace LogTail.Config
{
    public class XmlFileReader : DynamicObject
    {
        private IDictionary<string, string> CmdManager;

        public XmlFileReader()
        {
            CmdManager = new Dictionary<string, string>();
            
        }


    }
}
