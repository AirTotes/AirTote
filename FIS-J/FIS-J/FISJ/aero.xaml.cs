﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FIS_J.Services;
using FIS_J.ViewModels;

namespace FIS_J.FISJ
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class aero : ContentPage
    {
        HtmlViewModel html = new();
        public aero()
        {
            InitializeComponent();
            BindingContext = html;
            AICsView.Clicked += AICsView_Clicked;
            AIPView.Clicked += AIPView_Clicked;
            SUPsView.Clicked += SUPsView_Clicked;
        }
        private void SUPsView_Clicked(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                var ais = new AISJapan("kojdai", "Kojdai0510");
                System.Diagnostics.Debug.WriteLine("PASS Running");
                var result = await ais.GetPage("https://aisjapan.mlit.go.jp/html/AIP/html/20220224/eSUP/JP-eSUPs-en-JP.html");
                System.Diagnostics.Debug.WriteLine(result);
                html.HTML = new HtmlWebViewSource()
                {
                    Html = result
                };
            });
        }
        private void AIPView_Clicked(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                var ais = new AISJapan("kojdai", "Kojdai0510");
                System.Diagnostics.Debug.WriteLine("PASS Running");
                var result = await ais.GetPage("https://aisjapan.mlit.go.jp/html/AIP/html/20220224/eAIP/20220301/JP-menu-en-JP.html");
                System.Diagnostics.Debug.WriteLine(result);
                html.HTML = new HtmlWebViewSource()
                {
                    Html = result
                };
            });
        }
        private void AICsView_Clicked(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                var ais = new AISJapan("kojdai", "Kojdai0510");
                System.Diagnostics.Debug.WriteLine("PASS Running");
                var result = await ais.GetPage("https://aisjapan.mlit.go.jp/html/AIP/html/20220324/eAIC/JP-eAICs-jp-JP.html");
                System.Diagnostics.Debug.WriteLine(result);
                html.HTML = new HtmlWebViewSource()
                {
                    Html = result
                };
            });
        }
    }
}