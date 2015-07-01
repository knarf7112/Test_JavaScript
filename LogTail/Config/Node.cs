using System;
using System.Collections.Generic;
using System.Xml;
namespace LogTail.Config
{
    /// <summary>
    /// 存放Xml轉換的資料結構(結構還不完整,目前沒用)
    /// </summary>
    [Serializable]
    public class Node
    {
        public string NodeName { get; set; }

        public XmlNodeType Nodetype { get; set; }

        public string NodeValue { get; set; }

        public Node NextNode { set; get; }

        private IDictionary<string, string> _AttributeList = new Dictionary<string, string>();

        private IList<Node> _ChildNodeList = new List<Node>();
        //get Child Node
        public Node this[int index]
        {
            get
            {
                return this._ChildNodeList[index];
            }
            set
            {
                if (this._ChildNodeList[index - 1] == null)
                {
                    throw new Exception("此index(" + index + ")前一位元素為null,無法插入當前位置");
                }
                if (this._ChildNodeList[index] == null)
                {
                    //因為IList無法用list[index] = value的方式
                    this._ChildNodeList.Add(value);
                }
                else
                {
                    this._ChildNodeList[index] = value;
                }
            }
        }
        //get Attribute
        public string this[string key]{
            get
            {
                if (this._AttributeList.ContainsKey(key))
                    return this._AttributeList[key];
                else
                    return null;
            }
            set
            {
                //this._AttributeList.Add(key, value);
                this._AttributeList[key] = value;
            }
        }

        public IDictionary<string, string> GetAttributeDic
        {
            get
            {
                return this._AttributeList;
            }
        }

        public IList<Node> GetChildList
        {
            get
            {
                return this._ChildNodeList;
            }
        }

        public int AttributeLength
        {
            get
            {
                return this._AttributeList.Count;
            }
        }

        public int ChildLength
        {
            get
            {
                return this._ChildNodeList.Count;
            }
        }
    }
}
