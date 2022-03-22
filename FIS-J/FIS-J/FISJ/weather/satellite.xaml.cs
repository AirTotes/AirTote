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
        }


        [Obsolete]
        private void Japansatellite_Clicked_1(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("http://www.micosfit.jp/wakayama08/satellite/"));
            }
            else
            {
                Device.OpenUri(new Uri("http://www.micosfit.jp/wakayama08/satellite/"));
            }
        }
    }
}