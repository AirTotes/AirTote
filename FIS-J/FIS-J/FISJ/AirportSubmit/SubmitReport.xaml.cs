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
    public partial class SubmitReport : ContentPage
    {
        public SubmitReport()
        {
            InitializeComponent();
            AirportUse.Clicked += AirportUse_Clicked;
            FlightPlan.Clicked += FlightPlan_Clicked;
        }

        private void FlightPlan_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.FlightPlan());
        }

        private void AirportUse_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.AirportUse());
        }
    }
}