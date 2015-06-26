using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
//
using System.Xml.Linq;

namespace Test_XmlToDynamic
{
    class Program
    {
        static void Main(string[] args)
        {
            //------------------------------------------------------
            //黑暗執行緒的簡易動態範例
            //http://blog.darkthread.net/post-2011-06-10-expandoobject.aspx
            //-----------------------------------
            //失敗的測試: 本來想用類似JS語言寫法
            //dynamic foo = new { };
            //foo.Qoo = "Yes";
            //Console.WriteLine(foo.Qoo);
            //------------------------------------------------------
            dynamic boo = new ExpandoObject();//神奇的動態動到爽的物件
            boo.Qoo = "knock";//動態物件的屬性並給予字串值

            //查MSDN: ExpandoObject可轉集合類的IDynamicMetaObjectProvider, IDictionary<string, Object>, ICollection<KeyValuePair<string, Object>>, IEnumerable<KeyValuePair<string, Object>>, IEnumerable, INotifyPropertyChanged
            IDictionary<string, object> booList = boo as IDictionary<string, object>;//ExpandoObject可轉型成IDictionary<stirng, object>
            
            Console.WriteLine("Qoo:" + booList["Qoo"] + ":" + boo.Qoo);
            booList["Show"] = (Action<string>)((string s) => { Console.WriteLine("這是動態執行的方法:" + s); });//定義動態方法
            boo.Show("helle this is test");//動態...
            ((Action<string>)booList["Show"]).Invoke("Run...");//靜態...
            //-------------------------------------------------------
            //***********************************************************************
            //read from text
            //var xDoc = XDocument.Parse(txt);

            //read from url
            //var request = WebRequest.Create(@"http://...") as HttpWebRequest;
            //request.Credentials = CredentialCache.DefaultNetworkCredentials;
            //var xDoc = XDocument.Load(request.GetResponse().GetResponseStream());

            //read from file
            var xDoc = XDocument.Load(new StreamReader("contacts.xml"));

            dynamic root = new ExpandoObject();

            XmlToDynamic.Parse(root, xDoc.Elements().First());

            Console.WriteLine(root.contacts.contact.Count + "\t:" + root.contacts.contact.Count.GetType());
            Console.WriteLine(root.contacts.contact[0].Qoo + "\t:" + root.contacts.contact[0].Qoo.GetType());
            Console.WriteLine(root.contacts.contact[0].firstName + " :" + root.contacts.contact[0].firstName.GetType());
            Console.WriteLine(root.contacts.contact[0].id + "\t:" + root.contacts.contact[0].id.GetType());
            Console.ReadKey();
        }
    }
}
