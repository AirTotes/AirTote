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
    public partial class weekly : ContentPage
    {
        public weekly()
        {
            InitializeComponent();
            weeklysuppoert.Clicked += Weeklysuppoert_Clicked;
            weeklyensemble.Clicked += Weeklyensemble_Clicked;
            weeklyensembleforecast.Clicked += Weeklyensembleforecast_Clicked;
        }

        private void Weeklyensembleforecast_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.weeklyensembleforecast());
        }

        private void Weeklyensemble_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.weeklyensemble());
        }

        private void Weeklysuppoert_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.weeklysuppoert());
        }
    }
}