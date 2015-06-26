using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//
using System.Dynamic;
using System.Xml.Linq;

namespace Test_XmlToDynamic
{
    /// <summary>
    /// 將寫入的Xml的Tag動態的定義到記憶體的
    /// </summary>
    public class XmlToDynamic
    {
        /// <summary>
        /// 將Xml檔案轉換成動態的物件
        /// </summary>
        /// <param name="parent">動態載入資料用的的ExpandoObject物件</param>
        /// <param name="node"></param>
        public static void Parse(dynamic parent, XElement node)
        {
            //若node內有子項目
            if (node.HasElements)
            {
                //抓取node內Tag的名稱超過一個的(即兩個以上同樣的node)
                if (node.Elements(node.Elements().First().Name.LocalName).Count() > 1)
                {
                    //list
                    var item = new ExpandoObject();//建立一個空
                    var list = new List<dynamic>();
                    foreach (var element in node.Elements())
                    {
                        Parse(list, element);
                    }

                    AddProperty(item, node.Elements().First().Name.LocalName, list);
                    AddProperty(parent, node.Name.ToString(), item);//這個要用到Microsoft.CSharp.Rumtime
                }
                else
                {
                    var item = new ExpandoObject();

                    foreach (var attribute in node.Attributes())
                    {
                        AddProperty(item, attribute.Name.ToString(), attribute.Value.Trim());
                    }

                    //element
                    foreach (var element in node.Elements())
                    {
                        Parse(item, element);
                    }
                    //第二層
                    AddProperty(parent, node.Name.ToString(), item);
                }
            }
            else
            {
                //表示為最內層的node,只剩value
                AddProperty(parent, node.Name.ToString(), node.Value.Trim());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">dynamic 型別的變數都會編譯為 object 型別的變數,dynamic會略過編譯時期型別檢查</param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        private static void AddProperty(dynamic parent, string name, object value)
        {
            //Runtime check this parametter若為List<dynamic>集合
            if (parent is List<dynamic>)
            {
                //轉成List<dynamic>集合並將object(object還是List<dynamic>集合)加入集合
                //表示還沒到坎套的最底部(leaf)結構
                (parent as List<dynamic>).Add(value);
            }
            else
            {
                //轉成IDictionary<String, object>集合並
                //表示為坎套的最底部(即只剩一個node跟node的value,沒有任何子node了)
                (parent as IDictionary<String, object>)[name] = value;
                //上面等同 =>  IDictionary<String, object> parentDic = parent as IDictionary<String, object>
                //            parentDic[name] = value;//增加name的值到key,value的值到value
            }
        }
    }
}
