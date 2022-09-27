using PECS2022;
using PECS2022.UWP.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly:ExportRenderer(typeof(CustomEditor) , typeof(CustomEditorRenderer))]
namespace PECS2022.UWP.Classes
{
   public  class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer()
        {

        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            Control.TextAlignment = Windows.UI.Xaml.TextAlignment.Start;
            Control.FlowDirection=Windows.UI.Xaml.FlowDirection.RightToLeft;
           // Control.FontSize = 100;

            Control.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;



        }
    }
    
}
