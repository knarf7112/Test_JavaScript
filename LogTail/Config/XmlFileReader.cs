using System;
using System.Collections.Generic;
using System.Linq;
//
using System.Xml.Linq;
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
        //private XElement xEle;
        //private IEnumerable<XElement> list;
        /// <summary>
        /// Xml檔案讀取
        /// </summary>
        /// <param name="filePath">ex:"Config\DBConfig.xml"</param>
        public XmlFileReader(string filePath)
        {
            //CmdManager = new Dictionary<string, string>();
            //TODO..............................
            //讀取Xml設定檔的方式,還沒想到要怎寫比較靈活
            //var DOC = XDocument.Load("DBConfig.xml");
            string fullPath = AppDomain.CurrentDomain.RelativeSearchPath + filePath;
            try
            {
                this.xDoc = XDocument.Load(new StreamReader(fullPath));
                //this.xEle = this.xDoc.Root;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 暫時不用
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="node"></param>
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

        /// <summary>
        /// Get Node Value for the Node
        /// Condition is the node's Attribute value
        /// </summary>
        /// <param name="nodeName">Search's Node Name</param>
        /// <param name="attributeName">依據Node's Attribute Name</param>
        /// <param name="attributeValue">依據Node's Attribute Value</param>
        /// <returns>指定Node的Value</returns>
        public string GetNodeValue(string nodeName,string attributeName,string attributeValue)
        {
            XElement resultXele = this.xDoc.Descendants(nodeName).Where(
                (XElement x) =>{
                    if (x.Attribute(attributeName) != null)
                        return x.Attribute(attributeName).Value.Trim().ToUpper() == attributeValue.ToUpper();
                    else
                        return false;
                }).FirstOrDefault();//.Value.Trim();
            if (resultXele != null)
            {
                return resultXele.Value.Trim();
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// Get Attribute's Value for the Node
        /// Condition is the node's Attribute value
        /// </summary>
        /// <param name="nodeName">Search's Node Name</param>
        /// <param name="attributeName"></param>
        /// <param name="conditionAttributeName">條件式:Node AttributeName</param>
        /// <param name="conditionattributeValue">此Node AttributeName的Value</param>
        /// <returns>指定Node的某個Attribute Value</returns>
        public string GetNodeAttributeValue(string nodeName, string attributeName, string conditionAttributeName, string conditionattributeValue)
        {
            XElement resultXele = this.xDoc.Descendants(nodeName).Where(
                (XElement x) =>{
                    if (x.Attribute(conditionAttributeName) != null)
                        return x.Attribute(conditionAttributeName).Value.Trim().ToUpper() == conditionattributeValue.ToUpper();
                    else
                        return false;
                }).FirstOrDefault();//.Attribute(attributeName).Value.Trim();
            if (resultXele != null && resultXele.Attribute(attributeName) != null)
            {
                return resultXele.Attribute(attributeName).Value.Trim();
            }
            else
            {
                return null;
            }
        }
    }
}
