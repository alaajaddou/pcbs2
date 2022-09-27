using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PECS2022
{
    [ContentProperty("SourceImage")]
    public class PlatformImageExtension : IMarkupExtension<string>
    {
        public string SourceImage { get; set; }
        public string ProvideValue(IServiceProvider serviceProvider)
        {
            if (SourceImage == null)
                return null;
            string imagePath=string.Empty;

            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                case Device.iOS:
                    imagePath = SourceImage;
                    break;
                case Device.UWP:
                    imagePath = "Images/" + SourceImage;
                    break;               
            }
            return imagePath;
        }
        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return ProvideValue(serviceProvider);
        }
    }
}

