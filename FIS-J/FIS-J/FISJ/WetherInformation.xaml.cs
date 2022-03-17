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
    public partial class WetherInformation : ContentPage
    {
        public WetherInformation()
        {
            InitializeComponent();
            Nomwether.Clicked += Nomwether_Clicked;
            Upperwether.Clicked += Upperwether_Clicked;
            SkyInformation.Clicked += SkyInformation_Clicked;
        }
        private void Nomwether_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.Nomwether());
        }
        private void Upperwether_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.Upperwether());
        }
        private void SkyInformation_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.SkyInformation());
        }
    }
}