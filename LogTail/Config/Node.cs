using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogTail.Config
{
    /// <summary>
    /// 存放Xml轉換的資料結構(結構還不完整,目前沒用)
    /// </summary>
    [Serializable]
    public class Node
    {
        public string NodeName { get; set; }

        public string NodeValue { get; set; }

        public Node NextNode { set; get; }

        public IDictionary<string, string> AttributeList = new Dictionary<string, string>();

        private IList<Node> SameNameNodeList = new List<Node>();
        public Node this[int index]
        {
            get
            {
                return this.SameNameNodeList[index];
            }
            set
            {
                if (this.SameNameNodeList[index - 1] == null)
                {
                    throw new Exception("此index(" + index + ")前一位元素為null,無法插入當前位置");
                }
                if (this.SameNameNodeList[index] == null)
                {
                    //因為IList無法用list[index] = value的方式
                    this.SameNameNodeList.Add(value);
                }
                else
                {
                    this.SameNameNodeList[index] = value;
                }
            }
        }
        //get Attribute
        public string this[int index,string key]{
            get
            {
                if (this.SameNameNodeList[index] != null && this.SameNameNodeList[index].AttributeList.ContainsKey(key))
                    return this.SameNameNodeList[index].AttributeList[key];
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                this.SameNameNodeList[index].AttributeList[key] = value;
            }
        }

        public int Length
        {
            get
            {
                return this.SameNameNodeList.Count;
            }
        }
    }
}
