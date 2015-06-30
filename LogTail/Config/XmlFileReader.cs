using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
//
using System.Xml;
using System.Xml.Linq;
using System.Dynamic;
using System.IO;

namespace LogTail.Config
{
    //邏輯有問題 暫時不寫了
    public class XmlFileReader
    {
        #region 未完成版
        //private IDictionary<string, string> CmdManager;

        ////任意tag的Parser(目前未寫完)
        //public static void Parse(object parent,XElement node)
        //{
        //    //如果node還有子集合
        //    if (node.HasElements)
        //    {
        //        #region 繼續遞迴掃描深度
        //        //如果有重複的TagName,加在同層
        //        if (node.Elements(node.Elements().First().Name.LocalName).Count() > 1)
        //        {
        //            var item = new Node();
        //            //var list = new List<Node>();
        //            //parentNode.SubNodeList = new List<Node>(node.Elements(node.Elements().First().Name.LocalName).Count());
        //            int i = 0;
        //            foreach (var element in node.Elements())
        //            {
        //                //list.Add(new Node());
        //                Parse(item[i++], element);
        //            }
        //            //AddProperty(item, node.Elements().First().Name.LocalName, list);
        //            item.NodeName = node.Elements().First().Name.LocalName;
        //            ((Node)parent).NextNode = item;
        //            //AddProperty(parent, node.Name.ToString(), item);
        //        }
        //        else
        //        {
        //            //只有單一個TagName
        //            var item = new Node();
        //            foreach (var attribute in node.Attributes())
        //            {
        //                AddProperty(item.AttributeList, attribute.Name.ToString(), attribute.Value.Trim());
        //            }

        //            foreach (var element in node.Elements())
        //            {
        //                Parse(item.NextNode, element);
        //            }
        //            AddProperty(parent, node.Name.ToString(), item);
        //        }
        //        #endregion
        //    }
        //    else
        //    {
        //        //最尾部的node
        //        if (node.HasAttributes)
        //        {
        //            var ele = parent as Node;
        //            //ele.AttributeList = new Dictionary<string, string>();
        //            foreach (XAttribute item in node.Attributes())
        //            {
        //                AddProperty(ele.AttributeList, item.Name.ToString(), item.Value.Trim());
                        
        //            }
        //        }
        //        AddProperty(parent, node.Name.LocalName, node.Value.Trim());
        //    }
        //}

        //private static void AddProperty(object obj, string name, object value)
        //{
        //    //subNode
        //    if (obj is IList<Node>)
        //    {
        //        var node = value as Node;
        //        (obj as IList<Node>).Add(node);
        //    }
        //        //attribute
        //    else if (obj is IDictionary<string, string>)
        //    {
        //        //(obj as IDictionary<string, object>).Add(name, value);
        //        (obj as IDictionary<string, string>)[name] = (string)value;
        //    }
        //    else
        //    {
        //        Node leaf = obj as Node;
        //        if (leaf != null)
        //        {
        //            leaf.NodeName = name;
        //            leaf.NodeValue = (string)value;
        //        }
        //    }
        //}
        #endregion
        //----------------------------------------------------------------------------
        //簡易設定
        private IDictionary<string, object> XmlDic = new Dictionary<string, object>();
        private XDocument xDoc;
        public XElement xEle;
        public XmlFileReader(string filePath)
        {
            //CmdManager = new Dictionary<string, string>();
            //TODO..............................
            //讀取Xml設定檔的方式,還沒想到要怎寫比較靈活
            //var DOC = XDocument.Load("DBConfig.xml");
            string path = AppDomain.CurrentDomain.BaseDirectory;
            this.xDoc = XDocument.Load(new StreamReader(path + filePath));
            this.xEle = this.xDoc.Root;
        }

        public void Parse(object parent,XElement node)
        {
            if (node.HasElements)
            {
                var result = parent as IDictionary<string, object>;
                if(node.Elements(node.Elements().First().Name.LocalName).Count() > 1)
                {
                    var list = new List<object>();
                    //var result = parent as IDictionary<string,object>;
                    result.Add(node.Name.LocalName, list);
                    int i = 0;
                    foreach (XElement ele in node.Elements())
                    {
                        //var n = new Dictionary<string, object>();
                        list.Add(new Dictionary<string, object>());
                        Parse(list[i++], ele);
                    }
                }
                else
                {
                    foreach(XElement n in node.Elements())
                    {
                        result.Add(n.Name.LocalName,new Dictionary<string, object>());
                        Parse(result[n.Name.LocalName], n);
                    }
                }
            }
            else
            {
                var result =  parent as IDictionary<string,object>;
                if (node.HasAttributes)
                {
                    var dic = new Dictionary<string, string>();
                    foreach (XAttribute attribute in node.Attributes())
                    {
                        dic.Add(attribute.Name.LocalName, attribute.Value.Trim());
                    }
                    result.Add("Attribute",dic);
                }
                result.Add(node.Name.LocalName,node.Value.Trim());
            }
        }

        //public void AddProperty(object )
        public string GetString(string key)
        {
            return (string)XmlDic[key];
        }

        public T GetObject<T>(string key)
        {
            return (T)XmlDic[key];
        }
    }
}
