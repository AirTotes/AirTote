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
    public partial class AirportUse : ContentPage
    {
        public AirportUse()
        {
            InitializeComponent();
            hirosima.Clicked += Hirosima_Clicked;
            takamatu.Clicked += Takamatu_Clicked;
            haneda.Clicked += Haneda_Clicked;
            kounan.Clicked += Kounan_Clicked;
        }

        private void Kounan_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.kounan());
        }

        private void Haneda_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.hirosima());
        }

        private void Takamatu_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.takamatu());
        }

        private void Hirosima_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.haneda());
        }
    }
}