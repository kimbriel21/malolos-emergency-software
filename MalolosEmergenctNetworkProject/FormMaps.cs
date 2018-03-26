using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;

namespace MalolosEmergenctNetworkProject
{
    public partial class FormMaps : Form
    {
        public FormMaps()
        {
            InitializeComponent();
        }

        private void FormMaps_Load(object sender, EventArgs e)
        {
            var appName = Process.GetCurrentProcess().ProcessName + ".exe";

            WebBrowserHelper.FixBrowserVersion(appName, 11);

            webBrowser1.ScriptErrorsSuppressed = true;
            String location_longhitude = DataHolder.requestReport.location_longhitude;
            String location_latitude = DataHolder.requestReport.location_latitude;

            //webBrowserMap.Navigate("http://128.199.193.235/location/" + location_longhitude + "/" + location_latitude);
            //webBrowserMap.Navigate("http://www.google.com/maps/place/" + location_longhitude + "," + location_latitude);
            webBrowser1.Navigate("https://www.google.com/maps?q=loc:" + location_latitude + "," + location_longhitude);
            //webBrowser1.Navigate(url);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
