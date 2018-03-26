using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace MalolosEmergenctNetworkProject
{  
    public partial class RegisterForm : Form
    {
        SqlConnection cn;
        SqlCommand cmd;
        //SqlDataReader dr;

        public RegisterForm()
        {
            InitializeComponent();
            database_connection();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void database_connection()
        {
            cn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\user_db.mdf;Integrated Security=True");
            cmd = new SqlCommand();
            cmd.Connection = cn;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            String username = textBoxUsername.Text.ToString();
            String password = textBoxPassword.Text.ToString();
            String confirm_password = textBoxConfirmPassword.Text.ToString();
            String first_name = textBoxFirstName.Text.ToString();
            String last_name = textBoxLastName.Text.ToString();
            String office_branch = comboBoxOfficeBranch.Text.ToString();
            String department = comboBoxDepartment.Text.ToString();
            if ((username != "") && (password != "") && (first_name != "") && (last_name != "") && (office_branch != "") && (department != ""))
            {
                if (department == "Fire Department")
                {
                    department = "Fire";
                }
                else if (department == "Medical Department")
                {
                    department = "Medical";
                }
                else if (department == "Police Department")
                {
                    department = "Police";
                }
                else if (department == "Search and Rescue Department")
                {
                    department = "Rescue";
                }

                if (password == confirm_password)
                {
                    cn.Open();
                    cmd.CommandText = "insert into tbl_user (id,username, password, first_name, last_name, office_branch, department) values ('0','" + username + "','" + password + "','" + first_name + "','" + last_name + "','" + office_branch + "','" + department + "')";
                    cmd.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Register Success");

                    this.Hide();
                    var form1 = new Form1();
                    form1.Closed += (s, args) => this.Close();
                    form1.Show();
                }
                else
                {
                    MessageBox.Show("Password and Confirm password is not equal");
                }
            }
            else
            {
                MessageBox.Show("Please fillout all the data");
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form1 = new Form1();
            form1.Closed += (s, args) => this.Close();
            form1.Show();
        }

        private void comboBoxOfficeBranch_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBoxDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxOfficeBranch.Items.Clear();
            comboBoxOfficeBranch.DisplayMember = "Text";
            comboBoxOfficeBranch.ValueMember = "Value";

            switch(comboBoxDepartment.Text.ToString())
            {
                case "Fire Department":
                    comboBoxOfficeBranch.Items.Add(new { Text = "Malolos City Fire Station", Value = "Malolos City Fire Station" });
                    comboBoxOfficeBranch.Items.Add(new { Text = "Bulacan Fire Station", Value = "Bulacan Fire Station" });
                    comboBoxOfficeBranch.Items.Add(new { Text = "Panasahan Fire Sub-Station", Value = "Panasahan Fire Sub-Station" });
                    break;
                case "Medical Department":
                    comboBoxOfficeBranch.Items.Add(new { Text = "Malolos Maternity Hospital & Eye Center", Value = "Malolos Maternity Hospital & Eye Center" });
                    comboBoxOfficeBranch.Items.Add(new { Text = "Mary Immaculate Maternity Hospital", Value = "Mary Immaculate Maternity Hospital" });
                    comboBoxOfficeBranch.Items.Add(new { Text = "Sacred Heart Hospital-Bulacan", Value = "Sacred Heart Hospital-Bulacan" });
                    comboBoxOfficeBranch.Items.Add(new { Text = "Santissima Trinidad Hospital", Value = "Santissima Trinidad Hospital" });
                    comboBoxOfficeBranch.Items.Add(new { Text = "Santos General Hospital", Value = "Santos General Hospital" });
                    break;
                case "Police Department":
                    comboBoxOfficeBranch.Items.Add(new { Text = "Headquarters Bulacan Police Provincial Office", Value = "Headquarters Bulacan Police Provincial Office" });
                    comboBoxOfficeBranch.Items.Add(new { Text = "Malolos City Police Station", Value = "Malolos City Police Station" });
                    break;
                case "Search and Rescue Department":
                    comboBoxOfficeBranch.Items.Add(new { Text = "PDRRMC Malolos", Value = "PDRRMC Malolos" });
                    break;
            }
        }
    }
}
