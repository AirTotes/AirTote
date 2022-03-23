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
    public partial class haneda : ContentPage
    {
        public haneda()
        {
            InitializeComponent();
        }
        [Obsolete]
        private void hanedaAirportuse_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3011.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3011.pdf"));
            }
        }
        [Obsolete]
        private void hanedaAirportusepermittion_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3021.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3021.pdf"));
            }
        }    
        [Obsolete]
        private void hanedaWeightOverpermittion_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3031.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3031.pdf"));
            }
        }
        [Obsolete]
        private void hanedaUseInstitutionpermmition_1_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3041.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3041.pdf"));
            }
        }
        [Obsolete]
        private void hanedaUseInstitutionpermmition_2_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3051.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3051.pdf"));
            }
        }
        [Obsolete]
        private void hanedaUseInstitutionpermmition_3_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3061.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3061.pdf"));
            }
        }
        [Obsolete]
        private void hanedaDiscount_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3071.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/R3071.pdf"));
            }
        }
        [Obsolete]
        private void hanedarestrictmap_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/download/d/pdf/hikouseigenn.pdf"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.kouwan.metro.tokyo.lg.jp/business/download/d/pdf/hikouseigenn.pdf"));
            }
        }
    }
}