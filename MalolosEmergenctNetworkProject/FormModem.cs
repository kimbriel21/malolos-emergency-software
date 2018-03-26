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
    public partial class FormModem : Form
    {
        public static String modem_url = "http://192.168.1.1/html/home.html";

        public FormModem()
        {
            InitializeComponent();
        }

        private void FormModem_Load(object sender, EventArgs e)
        {
            var appName = Process.GetCurrentProcess().ProcessName + ".exe";
            WebBrowserHelper.FixBrowserVersion(appName, 11);
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Navigate(modem_url);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
