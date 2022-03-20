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
    public partial class aero : ContentPage
    {
        public aero()
        {
            InitializeComponent();
            AICsView.Clicked += AICsView_Clicked;
            AIPView.Clicked += AIPView_Clicked;
            SUPsView.Clicked += SUPsView_Clicked;
        }

        private void SUPsView_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.SUPsView());
        }

        private void AIPView_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.AIPView());
        }
        private void AICsView_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FIS_J.Components.AICsView());
        }
    }
   
}