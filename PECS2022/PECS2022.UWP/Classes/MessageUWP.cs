using PECS2022.Interfaces;
using PECS2022.UWP.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

[assembly: Xamarin.Forms.Dependency(typeof(MessageUWP))]

namespace PECS2022.UWP.Classes
{
    public class MessageUWP : IMessage
    {
        ToastNotification toast;
        ToastNotifier ToastNotifier;
        private DispatcherTimer timer = new DispatcherTimer();
        public void LongAlert(string message)
        {

            if (toast != null)
            {
                ToastNotifier.Hide(toast);
             



            }

            ToastNotifier = ToastNotificationManager.CreateToastNotifier();
         
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(string.Empty));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(message));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            

            timer.Interval = new TimeSpan(0, 0, 4);
            timer.Start();
            timer.Tick -= timer_Tick;
            timer.Tick += timer_Tick;


            toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
        }

        private void timer_Tick(object sender, object e)
        {
            if (toast != null && ToastNotifier !=null)
                ToastNotifier.Equals(toast);

            toast = null;

            timer.Stop();

            timer.Tick -= timer_Tick;
        }

        public void ShortAlert(string message)
        {
            if (toast != null)
            {
                ToastNotifier.Hide(toast);




            }
            ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode(string.Empty));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode(message));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();
            timer.Tick -= timer_Tick;
            timer.Tick += timer_Tick;
            toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(2);
            ToastNotifier.Show(toast);
        }



    }
}
