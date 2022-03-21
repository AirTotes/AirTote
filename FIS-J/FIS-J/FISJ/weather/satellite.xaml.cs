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
    public partial class satellite : ContentPage
    {
        public satellite()
        {
            InitializeComponent();
            Japansatellite.Clicked += Japansatellite_Clicked;
        }

        private void Japansatellite_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.Japansatellite());
        }
    }
}