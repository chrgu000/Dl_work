using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DlApplication
{
    public partial class FrmPass : Form
    {
        public FrmPass()
        {
            InitializeComponent();
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            String pass = this.txtPass.Text;
            String pass_c = Util.getConfig(Util.close_pass);
            if (pass.Equals(pass_c))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("口令错误，你没有关闭权限！");
            }
        }

        private void cmdReturn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
