using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIS_J.FISJ
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Upperwether : ContentPage
    {
        public Upperwether()
        {
            InitializeComponent();
        }
        [Obsolete]
        private void upperasia200_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa20_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa20_00.pdf"));
            }
        }
        [Obsolete]
        private void upperasia200hpa12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa20_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa20_12.pdf"));
            }
        }
        [Obsolete]
        private void upperasia250hpa_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa25_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa25_00.pdf"));
            }
        }
        [Obsolete]
        private void upperasia250hpa12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa25_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupa25_12.pdf"));
            }
        }
        [Obsolete]
        private void upperasia300hpa00_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupn30_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupn30_00.pdf"));
            }
        }
        [Obsolete]
        private void upperasia300hpa12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupn30_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupn30_12.pdf"));
            }
        }
        [Obsolete]
        private void upperasia500hpa00_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq35_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq35_00.pdf"));
            }
        }
        [Obsolete]
        private void upperasia500hpa12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq35_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq35_12.pdf"));
            }
        }
        [Obsolete]
        private void upperasia850hpa00_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq78_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq78_00.pdf"));
            }
        }
        [Obsolete]
        private void upperasia850hpa12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq78_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/aupq78_12.pdf"));
            }
        }
        [Obsolete]
        private void upperNorthhemi00_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/auxn50_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/auxn50_12.pdf"));
            }
        }
        [Obsolete]
        private void uppereast850hpa00_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axfe578_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axfe578_00.pdf"));
            }
        }
        [Obsolete]
        private void uppereast850hpa12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axfe578_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axfe578_12.pdf"));
            }
        }
        [Obsolete]
        private void asiaGround850_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/feas50_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/feas50_12.pdf"));
            }
        }
        [Obsolete]
        private void uppercut00_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axjp140_00.pdff"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axjp140_00.pdf"));
            }
        }
        [Obsolete]
        private void uppercut12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axjp140_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/axjp140_12.pdf"));
            }
        }
    }
}