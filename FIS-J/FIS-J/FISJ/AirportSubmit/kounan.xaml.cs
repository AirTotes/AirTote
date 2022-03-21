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
    public partial class kounan : ContentPage
    {
        public kounan()
        {
            InitializeComponent();
        }
        [Obsolete]
        private void kounanAirportUse_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727276_misc.doc&wdOrigin=BROWSELINK"));
            }
            else
            {
                Device.OpenUri(new Uri("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727276_misc.doc&wdOrigin=BROWSELINK"));
            }
        }
        [Obsolete]

        private void kounandepart_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727265_misc.docx&wdOrigin=BROWSELINK"));
            }
            else
            {
                Device.OpenUri(new Uri("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727265_misc.docx&wdOrigin=BROWSELINK"));
            }
        }
        [Obsolete]

        private void landingdiscount_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727267_misc.docx&wdOrigin=BROWSELINK"));
            }
            else
            {
                Device.OpenUri(new Uri("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727267_misc.docx&wdOrigin=BROWSELINK"));
            }
        }
        [Obsolete]

        private void AirportCheck_Clicked(object sender, EventArgs e)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Device.OpenUri(new Uri("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727269_misc.docx&wdOrigin=BROWSELINK"));
            }
            else
            {
                Device.OpenUri(new Uri("https://view.officeapps.live.com/op/view.aspx?src=https%3A%2F%2Fwww.pref.okayama.jp%2Fuploaded%2Flife%2F736586_6727269_misc.docx&wdOrigin=BROWSELINK"));
            }
        }
    }
}