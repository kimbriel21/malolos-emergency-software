using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Microsoft.Win32;
using System.Diagnostics;

namespace MalolosEmergenctNetworkProject
{
    public partial class MainForm : Form
    {


        public MainForm()
        {
            InitializeComponent();
            background_color();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var appName = Process.GetCurrentProcess().ProcessName + ".exe";

            WebBrowserHelper.FixBrowserVersion(appName, 11);
            this.WindowState = FormWindowState.Maximized;
            getDisplayData();
           

        }

       
        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        public void getDisplayData()
        {
            String request_id           = DataHolder.requestReport.request_id;
            String location_longhitude  = DataHolder.requestReport.location_longhitude;
            String location_latitude    = DataHolder.requestReport.location_latitude;
            String office_branch        = DataHolder.requestReport.office_branch;
            String emergency_type       = DataHolder.requestReport.emergency_type;
            String emergency_category   = DataHolder.requestReport.emergency_category;
            String date_requested       = DataHolder.requestReport.date_requested;
            String status               = DataHolder.requestReport.status;
            String first_name           = DataHolder.requestReport.first_name;
            String last_name            = DataHolder.requestReport.last_name;
            String contact_number       = DataHolder.requestReport.contact_number;
            String address              = DataHolder.requestReport.address;
            String email                = DataHolder.requestReport.email;

            tbNameSender.Text       = first_name + " " + last_name;
            tbContactNumber.Text    = contact_number;
            tbAddress.Text          = address;
            tbEmailAddress.Text     = email;
            tbTypeReport.Text       = emergency_category;

            webBrowserMap.ScriptErrorsSuppressed = true;

            //webBrowserMap.Navigate("http://128.199.193.235/location/" + location_longhitude + "/" + location_latitude);
            //webBrowserMap.Navigate("http://www.google.com/maps/place/" + location_longhitude + "," + location_latitude);
            webBrowserMap.Navigate("https://www.google.com/maps?q=loc:" + location_latitude + "," + location_longhitude);
            webBrowserPicture.Navigate("http://128.199.193.235/view_image?request_id="+request_id);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            action_request("Accepted");
        }

        private void buttonComplete_Click(object sender, EventArgs e)
        {
            action_request("Completed");
            //send_completed("Hello World", "639959953335", DataHolder.requestReport.contact_number);
        }

        private void buttonReject_Click(object sender, EventArgs e)
        {

            action_request("Rejected");
        }

        public void action_request(String request_action)
        {
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                var postData = "action_request=" + request_action;
                postData += "&request_id=" + DataHolder.requestReport.request_id;
                byte[] data = encoding.GetBytes(postData);

                WebRequest request = (HttpWebRequest)WebRequest.Create("http://128.199.193.235/action_request");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                WebResponse response = request.GetResponse();
                stream = response.GetResponseStream();

                StreamReader sr = new StreamReader(stream);
                string response_output = sr.ReadToEnd();
                if (response_output == "success")
                {
                    MessageBox.Show("Request "+request_action);
                }
                else
                {
                    MessageBox.Show("Request failed to " + request_action);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data\n" + ex.Message);
            }
        }

        public void send_completed(String text_message, String number_from, String number_to)
        {
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                var postData = "number_to=" + number_to;
                postData += "&number_from=" + number_from;
                postData += "&text=" + text_message;
                byte[] data = encoding.GetBytes(postData);

                WebRequest request = (HttpWebRequest)WebRequest.Create("http://128.199.193.235/send_message");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                Stream stream = request.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                WebResponse response = request.GetResponse();
                stream = response.GetResponseStream();

                StreamReader sr = new StreamReader(stream);
                string response_output = sr.ReadToEnd();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Failed to text message" + ex.Message);
            }
        }

        private void background_color()
        {
            switch (DataHolder.department)
            {
                case "Fire":
                    pictureBoxLogo.Image = Properties.Resources.fire_station_full;
                    backgroundMain.BackColor = Color.FromArgb(225, 183, 13);
                    backgroundSub.BackColor = Color.FromArgb(253, 239, 140);
                    break;
                case "Medical":
                    pictureBoxLogo.Image = Properties.Resources.medical_station_full;
                    backgroundMain.BackColor = Color.FromArgb(219, 65, 0);
                    backgroundSub.BackColor = Color.FromArgb(254, 202, 206);
                    break;
                case "Rescue":
                    pictureBoxLogo.Image = Properties.Resources.search_and_rescue_full;
                    backgroundMain.BackColor = Color.FromArgb(66, 171, 66);
                    backgroundSub.BackColor = Color.FromArgb(159, 254, 196);
                    break;
                case "Police":
                    pictureBoxLogo.Image = Properties.Resources.police_station_full;
                    backgroundMain.BackColor = Color.FromArgb(0, 72, 206);
                    backgroundSub.BackColor = Color.FromArgb(159, 214, 253);
                    break;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            var mf = new FormMaps();
            //mf.Closed += (s, args) => this.Close();
            mf.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            var mf = new FormPicture();
            //mf.Closed += (s, args) => this.Close();
            mf.Show();
        }
    }
}
