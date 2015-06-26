#region Imports
using System;
using System.Collections.Generic;
//
using System.Reflection;
using System.Text;
using System.Xml.Linq;
#endregion
namespace LogTail.Common
{
    /// <summary>
    /// 將子類別instance內容以xml方式列出 
    /// </summary>
    [Serializable()]
    public abstract class AbstractDO : IComparable
    {
        [NonSerialized]
        private Dictionary<string, int> SkipDic = null; 
        /// <summary>
        /// Empty constructor for future use 
        /// </summary>
        public AbstractDO()
        {
        }

        /// <summary>
        ///    指定property名稱,在列表時跳過不顯示
        /// </summary>
        /// <param name="fName">欲跳過不顯示的欄位名稱</param>
        public virtual void addSkipField(String fName)
        {
            if (this.SkipDic == null)
            {
                this.SkipDic = new Dictionary<string, int>();
            }
            this.SkipDic.Add(fName, 1);
        }

        /// <summary>
        ///   逐一比對各property value,若均相同則回傳 true
        /// </summary>
        /// <param name="obj">待比對物件</param>
        /// <returns>true:各屬性均相同</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            //判斷obj物件(輸入參數)是否為當前物件的實作
            if (!this.GetType().IsInstanceOfType(obj))
            {
                return false;
            }
            //取得當前物件的所有屬性資訊
            PropertyInfo[] pi = this.GetType().GetProperties();

            try
            {
                foreach (PropertyInfo p in pi)
                {
                    //若屬性為忽略名單則跳過
                    if (this.SkipDic != null && this.SkipDic.ContainsKey(p.Name))
                    {
                        continue;
                    }
                    //若屬性的值為null
                    if (p.GetValue(this, null) == null && p.GetValue(obj, null) == null)
                    {
                        continue;
                    }
                    //若屬型型別為byte陣列
                    if (p.PropertyType == typeof(byte[]))
                    {
                        byte[] thisVal = (byte[])p.GetValue(this, null);
                        byte[] objVal = (byte[])p.GetValue(obj, null);
                        //若長度不一樣,表示有問題
                        if (thisVal.Length != objVal.Length)
                        {
                            return false;
                        }
                        //逐一比對陣列內所有的值
                        for (int pt = 0; pt < thisVal.Length; pt++)
                        {
                            if (!thisVal[pt].Equals(objVal[pt]))
                            {
                                return false;
                            }
                        }
                    }
                        //Recursive check this object property of value
                    else if (!p.GetValue(this, null).Equals(p.GetValue(obj, null)))
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 取得物件 Hash Code, ref:
        /// 若程式有重新定義Operator==，則我們必須跟著重新定義Object.GetHashCode方法
        /// </summary>
        /// <returns>value of hash code</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            XElement xd = this.GetXElement();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            using (System.Xml.XmlTextWriter xmlwriter = new System.Xml.XmlTextWriter(sw))
            {
                //縮排
                xmlwriter.Indentation = 2;
                xmlwriter.Formatting = System.Xml.Formatting.Indented;
                xd.WriteTo(xmlwriter);
                xmlwriter.Close();//會推送所有緩衝區資料
            }
            // out put the formated xml
            string res = sw.ToString();
            sw.Close();
            return "\n" + res;
        }

        /// <summary>
        ///    傳出該 Dependent Object 之排序字串值
        /// </summary>
        /// <returns>Dependent Object 之排序字串</returns>
        public virtual string GetSortKey()
        {
            PropertyInfo p = (this.GetType().GetProperties())[0];
            return p.GetValue(this, null).ToString();
        }

        /// <summary>
        ///    根據排序字串比較該 Dependent Object 與傳入物件之大小
        /// </summary>
        /// <param name="obj">傳入物件,需為相同之 Dependent Object</param>
        /// <returns>int 傳回值 負值:本物件較小; 0:兩物件相同; 正值:本物件較大</returns>
        public int CompareTo(object obj)
        {
            if ((obj != null) && this.GetType().IsInstanceOfType(obj))
            {
                AbstractDO dst = (AbstractDO)obj;
                return this.GetSortKey().CompareTo(dst.GetSortKey());
            }
            else
            {
                throw new Exception(
                     "傳入物件型別錯誤,無法比較: " +
                     "parameter class " +
                     this.GetType().Name +
                     " is not the same of my class " +
                     obj.GetType().Name
                );
            }
        }

        /// <summary>
        /// 將物件property內容組成XmlElement
        /// </summary>
        /// <returns>XmlElement</returns>
        public XElement GetXElement()
        {
            XElement root = new XElement(this.GetType().Name, "");
            PropertyInfo[] pi = this.GetType().GetProperties();

            try
            {
                //迴圈寫入XML物建
                foreach (PropertyInfo p in pi)
                {
                    XElement xe = new XElement(p.Name, "");
                    object tmpObj = p.GetValue(this, null);
                    if (tmpObj != null)
                    {
                        //若屬性名稱包含在忽略用的字典檔
                        if ((this.SkipDic != null) && this.SkipDic.ContainsKey(p.Name))
                        {
                            xe.SetValue("Skipped");
                        }
                        //屬性為AbstractDO型別(表示可能還在遞迴)
                        else if (tmpObj is AbstractDO)
                        {
                            //xe.InnerText = "AbstractDO";
                            xe.Add(new XElement(((AbstractDO)tmpObj).GetXElement()));
                        }
                        //若屬性為陣列
                        else if(tmpObj == typeof(byte[]))
                        {
                            byte[] thisVal = (byte[])tmpObj;
                            StringBuilder sb = new StringBuilder("{");
                            for (int pt = 0; pt < thisVal.Length; pt++)
                            {
                                sb.Append(String.Format("0x{0:X2}", thisVal[pt]));
                                if (pt < thisVal.Length - 1)
                                {
                                    sb.Append(',');
                                }
                                else
                                {
                                    sb.Append('}');
                                }
                            }
                            xe.Value = sb.ToString();
                        }
                        else
                        {
                            xe.Value = tmpObj.ToString();
                        }
                    }
                    root.Add(xe);
                }
            }
            catch
            {
                return null;
            }
            return root;
        }
    }
}
