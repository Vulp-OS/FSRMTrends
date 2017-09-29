using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace FSRMTrends
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            fillSettings();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            applySettings();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            applySettings();
        }

        private void applySettings()
        {
            SQLDataSource.Default.ConnectionString = txtConnString.Text;
            SQLDataSource.Default.FSRMServerName = txtFSRMName.Text;
            SQLDataSource.Default.Save();
        }

        private void fillSettings()
        {
            txtConnString.Text = SQLDataSource.Default.ConnectionString;
        }

        private void lblConnString_Click(object sender, EventArgs e)
        {

        }
    }
}
