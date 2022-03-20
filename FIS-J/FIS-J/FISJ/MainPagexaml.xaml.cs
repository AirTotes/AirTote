using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIS_J
{	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPagexaml : MasterDetailPage
    {
        public MainPagexaml()
        {
            InitializeComponent();
            Submit.Clicked += Submit_Clicked;
            Icao.Clicked += Icao_Clicked;
            Wether.Clicked += Wether_Clicked;
            aero.Clicked += aero_Clicked;
        }

        private void Submit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.SubmitFormxaml());
        }
        private void Icao_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.IcaoPage());
        }
        private void Wether_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.WetherInformation());
        }
        private void aero_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.aero());
        }

    }
}