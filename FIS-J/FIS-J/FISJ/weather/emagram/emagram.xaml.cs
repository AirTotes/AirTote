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
    public partial class emagram : ContentPage
    {
        public emagram()
        {
            InitializeComponent();
            zenkoku.Clicked += Zenkoku_Clicked;
            hokkaido.Clicked += Hokkaido_Clicked;
            touhoku.Clicked += Touhoku_Clicked;
            kanto.Clicked += Kanto_Clicked;
            hokuriku.Clicked += Hokuriku_Clicked;
            toukai.Clicked += Toukai_Clicked;
            kinki.Clicked += Kinki_Clicked;
            tyuugoku.Clicked += Tyuugoku_Clicked;
            sikoku.Clicked += Sikoku_Clicked;
            kyuusyukita.Clicked += Kyuusyukita_Clicked;
            kyuusyuminami.Clicked += Kyuusyuminami_Clicked;
            nansei.Clicked += Nansei_Clicked;
        }

        private void Nansei_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.nansei());
        }

        private void Kyuusyuminami_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.kyuusyuminami());
        }

        private void Kyuusyukita_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.kyuusyukita());
        }

        private void Sikoku_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.sikoku());
        }

        private void Tyuugoku_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.tyuugoku());
        }

        private void Kinki_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.kinki());
        }

        private void Toukai_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.toukai());
        }

        private void Hokuriku_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.hokuriku());
        }

        private void Kanto_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.kanto());
        }

        private void Touhoku_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.touhoku());
        }

        private void Hokkaido_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.hokkaido());
        }

        private void Zenkoku_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FISJ.zenkoku());
        }
    }
}