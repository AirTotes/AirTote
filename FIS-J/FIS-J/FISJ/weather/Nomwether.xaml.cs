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
    public partial class Nomwether : ContentPage
    {
        public Nomwether()
        {
            InitializeComponent();

        }

        [Obsolete]
        private void asia250hpa_Clicked_1(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa252_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa252_00.pdf"));
            }

        }

        [Obsolete]
        private void asia300hpa_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa302_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa302_00.pdf"));
            }
        }

        [Obsolete]
        private void asia400hpa_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa402_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa402_00.pdf"));
            }
        }
        [Obsolete]
        private void asia500hpa_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa502_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa502_00.pdf"));
            }
        }
        [Obsolete]
        private void east850hpatem_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe5782_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe5782_00.pdf"));
            }
        }
        [Obsolete]
        private void Groundpressure_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe502_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe502_00.pdf"));
            }
        }
        [Obsolete]
        private void asia250hpa12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa252_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa252_12.pdf"));
            }
        }
        [Obsolete]
        private void asia300hpa12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa302_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa302_12.pdf"));
            }
        }
        [Obsolete]
        private void asia400hpa12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa402_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa402_12.pdf"));
            }

        }
        [Obsolete]
        private void asia500hpa12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa502_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fupa502_12.pdf"));
            }

        }
        [Obsolete]
        private void east850hpatem12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe5782_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe5782_12.pdf"));
            }

        }
        [Obsolete]
        private void Groundpressure12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe502_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxfe502_12.pdf"));
            }

        }
        [Obsolete]
        private void asiaGround_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/feas502_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/feas502_12.pdf"));
            }

        }
        [Obsolete]
        private void japan850hpa_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxjp854_00.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxjp854_00.pdf"));
            }

        }
        [Obsolete]
        private void japan850hpa12_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxjp854_12.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.jma.go.jp/bosai/numericmap/data/nwpmap/fxjp854_12.pdf"));
            }
        }
    }
}