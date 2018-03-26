using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MalolosEmergenctNetworkProject
{
    public partial class FormPicture : Form
    {
        public FormPicture()
        {
            InitializeComponent();
        }

        private void FormPicture_Load(object sender, EventArgs e)
        {
            String request_id = DataHolder.requestReport.request_id;
            webBrowser1.Navigate("http://128.199.193.235/view_image?request_id=" + request_id);
        }
    }
}
