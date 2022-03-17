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
    public partial class SkyInformation : ContentPage
    {
        public SkyInformation()
        {
            InitializeComponent();
            badwetherforecast.Clicked += Badwetherforecast_Clicked;
            lowerbadwether.Clicked += lowerbadwether_Clicked;
            signmet.Clicked += signmet_Clicked;
            Internationalfukuoka.Clicked += Internationalfukuoka_Clicked;
            InternationalDomestic.Clicked += InternationalDomestic_Clicked;
        }

        private void Badwetherforecast_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.Badwetherforecast());
        }
        private void lowerbadwether_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.lowerbadwether());
        }
        private void signmet_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.signmet());
        }
        private void Internationalfukuoka_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.Internationalfukuoka());
        }
        private void InternationalDomestic_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.InternationalDomestic());
        }
      
    }
}