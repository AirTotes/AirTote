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
    public partial class longterm : ContentPage
    {
        public longterm()
        {
            InitializeComponent();
            monthcirculate.Clicked += Monthcirculate_Clicked;
            monthensemble.Clicked += Monthensemble_Clicked;
            monthspread.Clicked += Monthspread_Clicked;
            monthevery.Clicked += Monthevery_Clicked;
            torrid.Clicked += Torrid_Clicked;
        }

        private void Torrid_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.torrid());
        }

        private void Monthevery_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.monthevery());
        }

        private void Monthspread_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.monthspread());
        }

        private void Monthensemble_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.monthensemble());
        }

        private void Monthcirculate_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.monthcirculate());
        }
    }
}