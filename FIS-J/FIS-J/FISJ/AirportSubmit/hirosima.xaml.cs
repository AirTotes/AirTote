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
    public partial class hirosima : ContentPage
    {
        public hirosima()
        {
            InitializeComponent();
        }
        [Obsolete]
        private void hirosimaAirportUse_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.hij.airport.jp/assets/files/operation/airport_usage.pdf?202203211957"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.hij.airport.jp/assets/files/operation/airport_usage.pdf?202203211957"));
            }
        }
        [Obsolete]

        private void landingdiscount_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.hij.airport.jp/assets/files/operation/operator_information.pdf?202203211957"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.hij.airport.jp/assets/files/operation/operator_information.pdf?202203211957"));
            }
        }
        [Obsolete]

        private void PilotInformation_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://www.hij.airport.jp/assets/files/operation/operator_notification.pdf?202203211957"));
            }
            else
            {
                Device.OpenUri(new Uri("https://www.hij.airport.jp/assets/files/operation/operator_notification.pdf?202203211957"));
            }
        }
    }
}