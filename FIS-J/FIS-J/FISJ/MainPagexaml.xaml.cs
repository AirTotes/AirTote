using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIS_J
{	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPagexaml : MasterDetailPage
    {
        public MainPagexaml()
        {
            InitializeComponent();
            Submit.Clicked += Submit_Clicked;
            Icao.Clicked +=Icao_Clicked;
            Wether.Clicked += Wether_Clicked;
        }

        private void Submit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.SubmitFormxaml());
        }

        private void Image_SizeChanged(object sender, EventArgs e)
        {

        }
        private void Icao_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.IcaoPage());
        }
        private void Wether_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.WetherInformation());
        }
    }
}