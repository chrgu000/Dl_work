using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DlApplication
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        private Hook hk = new Hook();
        private void FrmMain_Load(object sender, EventArgs e)
        {
            var exename = Process.GetCurrentProcess().MainModule.ModuleName;
            string FEATURE_BROWSER_EMULATION = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION";
            string FEATURE_DOCUMENT_COMPATIBLE_MODE = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_DOCUMENT_COMPATIBLE_MODE";
            using (RegistryKey regkey1 = Registry.CurrentUser.CreateSubKey(FEATURE_BROWSER_EMULATION))
            using (RegistryKey regkey2 = Registry.CurrentUser.CreateSubKey(FEATURE_DOCUMENT_COMPATIBLE_MODE))
            {
                regkey1.SetValue(exename, 9999, RegistryValueKind.DWord);
                //regkey2.SetValue(exename, 90000, RegistryValueKind.DWord);
                regkey1.Close();
                //regkey2.Close();
            }

            hk.Hook_Start();
            webBrowser1.Url = new Uri(Util.getConfig(Util.web_url));
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            FrmPass frmPass = new FrmPass();
            DialogResult rs = frmPass.ShowDialog();
            if (rs == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                hk.Hook_Clear();
            }
        }
    }
}
