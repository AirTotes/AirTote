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
    public partial class live : ContentPage
    {
        public live()
        {
            InitializeComponent();
            liveasia200.Clicked += Liveasia200_Clicked;
            liveasia300.Clicked += Liveasia300_Clicked;
            liveasia500.Clicked += Liveasia500_Clicked;
            livenews.Clicked += Livenews_Clicked;
            liveuppercut.Clicked += Liveuppercut_Clicked;
            liveasia850.Clicked += Liveasia850_Clicked;
            liveasiaground.Clicked += Liveasiaground_Clicked;
            livenorthhemi.Clicked += Livenorthhemi_Clicked;
            liveeast850.Clicked += Liveeast850_Clicked;
        }

        private void Liveeast850_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.liveeast850());
        }

        private void Livenorthhemi_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.livenorthhemi());
        }

        private void Liveasiaground_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.liveasiaground());
        }

        private void Liveasia850_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.liveasia850());
        }

        private void Liveuppercut_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.liveuppercut());
        }

        private void Livenews_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.livenews());
        }

        private void Liveasia500_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.liveasia500());
        }

        private void Liveasia300_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.liveasia300());
        }

        private void Liveasia200_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.liveasia200());
        }
    }
}