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

namespace MalolosEmergenctNetworkProject
{
    public partial class ReportRequestForm : Form
    {
        RequestReport[] requestReport;
        String status_filter        = "";
        String date_filter          = "";
        String office_branch_filter = "";
        public ReportRequestForm()
        {
            InitializeComponent();
            background_color();
        }

        private void ReportRequestForm_Load(object sender, EventArgs e)
        {
            populateData();
            loadDepartmentOptions();
           
        }

        private void background_color()
        {
            switch (DataHolder.department)
            {
                case "Fire":
                    pictureBoxLogo.Image = Properties.Resources.fire_station_full;
                    backgroundMain.BackColor = Color.FromArgb(225, 183, 13);
                    backgroundSub2.BackColor = Color.FromArgb(253, 239, 140);
                    backgroundSub.BackColor = Color.FromArgb(253, 239, 140);
                    break;
                case "Medical":
                    pictureBoxLogo.Image = Properties.Resources.medical_station_full;
                    backgroundMain.BackColor = Color.FromArgb(219, 65, 0);
                    backgroundSub2.BackColor = Color.FromArgb(254, 202, 206);
                    backgroundSub.BackColor = Color.FromArgb(254, 202, 206);
                    break;
                case "Rescue":
                    pictureBoxLogo.Image = Properties.Resources.search_and_rescue_full;
                    backgroundMain.BackColor = Color.FromArgb(66, 171, 66);
                    backgroundSub2.BackColor = Color.FromArgb(159, 254, 196);
                    backgroundSub.BackColor = Color.FromArgb(159, 254, 196);
                    break;
                case "Police":
                    pictureBoxLogo.Image = Properties.Resources.police_station_full;
                    backgroundMain.BackColor = Color.FromArgb(0, 72, 206);
                    backgroundSub2.BackColor = Color.FromArgb(159, 214, 253);
                    backgroundSub.BackColor = Color.FromArgb(159, 214, 253);
                    break;
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
           
        }
        
        public void populateData()
        {
            String json_responce = get_request_data();
            JavaScriptSerializer js = new JavaScriptSerializer();
            requestReport = js.Deserialize<RequestReport[]>(json_responce);
            requestReport = requestReport.OrderByDescending(x => x.date_requested).ToArray();
            
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("date_requested", "Date Requested");
            dataGridView1.Columns.Add("office_branch", "Office Branch");
            dataGridView1.Columns.Add("emergency_category", "Emergency Category");
            dataGridView1.Columns.Add("full_name", "Full Name");
            dataGridView1.Columns.Add("contact_number", "Contact #");
            dataGridView1.Columns.Add("status", "Status");

            for (int i = 0; i < requestReport.Length; i++)
            {
                dataGridView1.Rows.Add(new object[] { requestReport[i].date_requested, requestReport[i].office_branch, requestReport[i].emergency_category, requestReport[i].first_name + " " +requestReport[i].last_name, requestReport[i].contact_number, requestReport[i].status });
            }
        }

        public String get_request_data()
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            var postData = "emergency_type=" + DataHolder.department;
            postData += "&status=" + status_filter;
            postData += "&request_date=" + date_filter;
            postData += "&office_branch=" + office_branch_filter;
            
            byte[] data = encoding.GetBytes(postData);

            WebRequest request = (HttpWebRequest)WebRequest.Create("http://128.199.193.235/select_data_request");
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

            return response_output;
        }

        public void action_request(String request_action)
        {
            DataHolder.requestReport = requestReport[dataGridView1.CurrentCell.RowIndex];
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
                    populateData();
                }
                else
                {
                    MessageBox.Show("Request Failed");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Failed to import data\n" + ex.Message);
            }
        }

        public void send_completed(String text_message,String number_from, String number_to)
        {
            DataHolder.requestReport = requestReport[dataGridView1.CurrentCell.RowIndex];

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
                if (response_output == "success")
                {
                    populateData();
                }
                else
                {
                    MessageBox.Show("Request Failed");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Failed to text message" + ex.Message);
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            DataHolder.requestReport = requestReport[dataGridView1.CurrentCell.RowIndex];
            var mf = new MainForm();
            //mf.Closed += (s, args) => this.Close();
            mf.Show();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            var mf = new Form1();
            mf.Closed += (s, args) => this.Close();
            mf.Show();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            status_filter = "";
            date_filter = "";
            office_branch_filter = "";
            populateData();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void backgroundMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void loadDepartmentOptions()
        {
            comboBoxOfficeBranchFilter.Items.Clear();
            comboBoxOfficeBranchFilter.DisplayMember = "Text";
            comboBoxOfficeBranchFilter.ValueMember = "Value";
            comboBoxOfficeBranchFilter.Items.Add(new { Text = "", Value = "" });
            switch (DataHolder.department)
            {
                case "Fire":
                    comboBoxOfficeBranchFilter.Items.Add(new { Text = "Malolos City Fire Station", Value = "Malolos City Fire Station" });
                    comboBoxOfficeBranchFilter.Items.Add(new { Text = "Bulacan Fire Station", Value = "Bulacan Fire Station" });
                    comboBoxOfficeBranchFilter.Items.Add(new { Text = "Panasahan Fire Sub-Station", Value = "Panasahan Fire Sub-Station" });
                    break;
                case "Medical":
                    comboBoxOfficeBranchFilter.Items.Add(new { Text = "Malolos Maternity Hospital And Eye Center", Value = "Malolos Maternity Hospital And Eye Center" });
                    comboBoxOfficeBranchFilter.Items.Add(new { Text = "Mary Immaculate Maternity Hospital", Value = "Mary Immaculate Maternity Hospital" });
                    comboBoxOfficeBranchFilter.Items.Add(new { Text = "Sacred Heart Hospital-Bulacan", Value = "Sacred Heart Hospital-Bulacan" });
                    comboBoxOfficeBranchFilter.Items.Add(new { Text = "Santissima Trinidad Hospital", Value = "Santissima Trinidad Hospital" });
                    comboBoxOfficeBranchFilter.Items.Add(new { Text = "Santos General Hospital", Value = "Santos General Hospital" });
                    break;
                case "Police":
                    comboBoxOfficeBranchFilter.Items.Add(new { Text = "Headquarters Bulacan Police Provincial Office", Value = "Headquarters Bulacan Police Provincial Office" });
                    comboBoxOfficeBranchFilter.Items.Add(new { Text = "Malolos City Police Station", Value = "Malolos City Police Station" });
                    break;
                case "Rescue":
                    comboBoxOfficeBranchFilter.Items.Add(new { Text = "PDRRMC Malolos", Value = "PDRRMC Malolos" });
                    break;
            }
        }

        private void buttonAccept_Click_1(object sender, EventArgs e)
        {
            action_request("Accepted");
        }

        private void buttonComplete_Click_1(object sender, EventArgs e)
        {
            action_request("Completed");
            //send_completed("Hello World", "639959953335", requestReport[dataGridView1.CurrentCell.RowIndex].contact_number);
        }

        private void buttonReject_Click_1(object sender, EventArgs e)
        {
            action_request("Rejected");
        }

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            date_filter = "";
            status_filter = comboBoxStatusFilter.Text.ToString();

            if (checkBoxDateFilter.Checked)
            {
                date_filter = dateTimePicker1.Text.ToString();
            }
            
            office_branch_filter = comboBoxOfficeBranchFilter.Text.ToString();
            populateData();
        }

        private void buttonModem_Click(object sender, EventArgs e)
        {
            var mf = new FormModem();
            //mf.Closed += (s, args) => this.Close();
            mf.Show();
        }
    }
}
