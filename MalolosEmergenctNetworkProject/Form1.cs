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
    public partial class Form1 : Form
    {
        SqlConnection cn;
        SqlCommand cmd;
        SqlDataReader dr;

        public Form1()
        {
            InitializeComponent();
            database_connection();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {

            String username = textBoxUsername.Text.ToString();
            String password = textBoxPassword.Text.ToString();
            if ((username != "") && (password != ""))
            {
                cn.Open();
                string query = "SELECT TOP 1 * FROM tbl_user where username = '" + username + "' and password = '" + password + "'";
                cmd.CommandText = query;
                dr = cmd.ExecuteReader();
                if (dr.Read()) //chect if have data
                {
                    DataHolder.office_branch    = dr[5].ToString();
                    DataHolder.department       = dr[6].ToString();

                    cn.Close();
                    this.Hide();
                    var mf = new ReportRequestForm();
                    mf.Closed += (s, args) => this.Close();
                    mf.Show();
                }
                else
                {
                    MessageBox.Show("Wrong Username or Password");
                }

                cn.Close();
            }
            else
            {
                MessageBox.Show("Please fill up username and password");
            }
        }

        private void database_connection()
        {
            cn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\user_db.mdf;Integrated Security=True");
            cmd = new SqlCommand();
            cmd.Connection = cn;
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            var registerForm = new RegisterForm();
            registerForm.Closed += (s, args) => this.Close();
            registerForm.Show();
        }


    }
}
