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
            live.Clicked += live_Clicked;
            weekly.Clicked += weekly_Clicked;
            longterm.Clicked += longterm_Clicked;
            satellite.Clicked += satellite_Clicked;
            emagram.Clicked += emagram_Clicked;
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
        private void live_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.live());
        }
        private void weekly_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.weekly());
        }
        private void longterm_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.longterm());
        }
        private void satellite_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.satellite());
        }
        private void emagram_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.emagram());
        }
    }
}