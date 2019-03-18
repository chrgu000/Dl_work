using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Threading;
using Microsoft.Win32;
using mshtml;
using System.Text.RegularExpressions;

namespace ActiveX
{
    [Guid("EF930FB4-1101-4A80-8D4D-A016AF081BA7"), ProgId("ActiveX.UserControl1"), ComVisible(true)]
    public partial class UserControl1 : UserControl, IObjectSafety
    {
        #region IObjectSafety 成员 格式固定

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;

        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            int Rslt = E_FAIL;
            string strGUID = riid.ToString("B");
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) && (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) && (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        #endregion

        #region "实例化"
        public UserControl1()
        {
            InitializeComponent();
        }
        #endregion

        #region "activeX接口"
        IHTMLWindow2 htmlWin;

        /// <summary>
        /// 服务入口
        /// </summary>
        /// <param name="rate">波特率</param>
        /// <param name="port">端口号</param>
        public void doAccess(string rate, string port, object win)
        {
            this.htmlWin = win as IHTMLWindow2;
            this.cmRate.Text = rate;
            if (!port.Equals(""))
            {
                this.cmID.Text = port;
            }
            btENT_Click();
        }

        /// <summary>
        /// 称重结果取得（JS调用）
        /// </summary>
        /// <returns></returns>
        public string getResult()
        {
            string str = this.tbData.Text.Trim();
            string val = Regex.Replace(str, "[^\\d.]+", "");
            return val;
        }

        /// <summary>
        /// 称重结果取得（调用JS）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbData_TextChanged(object sender, EventArgs e)
        {
            //调用JS
            //string jsCode = string.Format("{0}('{1}')", "Js_SetResult_wg", this.tbData.Text);
            //htmlWin.execScript(jsCode, "jscript");
        }
        #endregion

        #region "称重业务"
        private SerialPort Sp = new SerialPort();
        public delegate void HandleInterfaceUpdataDelegate(string text); //委托，此为重点
        private HandleInterfaceUpdataDelegate interfaceUpdataHandle;

        /// <summary>
        /// 开启监听
        /// </summary>
        private void btENT_Click()
        {
            if ((cmRate.Text.Trim() != "") && (cmID.Text != ""))
            {
                interfaceUpdataHandle = new HandleInterfaceUpdataDelegate(UpdateTextBox);//实例化委托对象
                Sp.PortName = cmID.Text.Trim();
                Sp.BaudRate = Convert.ToInt32(cmRate.Text.Trim());
                Sp.Parity = Parity.None;
                Sp.StopBits = StopBits.One;
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived);
                Sp.ReceivedBytesThreshold = 1;
                try
                {
                    Sp.Open();
                    ATCommand3("AT+CLIP=1\r", "OK");
                }
                catch
                {
                    MessageBox.Show("地磅COM端口" + cmID.Text.Trim() + "打开失败！");
                }
            }
            else
            {
                MessageBox.Show("地磅端口号和波特率无效，打开失败！");
                cmRate.Focus();
            }
        }

        /// <summary>
        /// 画面关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Sp.Close();
        }

        /// <summary>
        /// 画面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            GetComList();
        }

        /// <summary>
        /// 接收地磅数据监听事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Sp_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string strTemp = "";
            double iSecond = 0.5;
            DateTime dtOld = System.DateTime.Now;
            DateTime dtNow = System.DateTime.Now;
            TimeSpan dtInter;
            dtInter = dtNow - dtOld;
            int i = Sp.BytesToRead;
            if (i > 0)
            {
                try
                {
                    strTemp = Sp.ReadExisting();
                }
                catch
                { }
                if (strTemp.ToLower().IndexOf("\r") < 0)
                {
                    i = 0;
                }
                else
                {
                    this.Invoke(interfaceUpdataHandle, strTemp);
                }
            }
            while (dtInter.TotalSeconds < iSecond && i <= 0)
            {
                dtNow = System.DateTime.Now;
                dtInter = dtNow - dtOld;
                i = Sp.BytesToRead;
                if (i > 0)
                {
                    try
                    {
                        strTemp += Sp.ReadExisting();
                    }
                    catch
                    { }
                    if (strTemp.ToLower().IndexOf("\r") < 0)
                    {
                        i = 0;
                    }
                    else
                    {
                        this.Invoke(interfaceUpdataHandle, strTemp);
                    }
                }
            }
            // do null
        }

        /// <summary>
        /// 返回数据赋值接口
        /// </summary>
        /// <param name="text"></param>
        private void UpdateTextBox(string text)
        {
            tbData.Text = text;
        }

        /// <summary>
        /// 执行AT指令并返回 成功失败
        /// </summary>
        /// <param name="ATCmd">AT指令</param>
        /// <param name="StCmd">AT指令标准结束标识</param>
        /// <returns></returns>
        private void ATCommand3(string ATCmd, string StCmd)
        {
            string response = "";
            response = ATCommand(ATCmd, StCmd);
        }

        /// <summary>
        /// 执行AT指令并返回结果字符
        /// </summary>
        /// <param name="ATCmd">AT指令</param>
        /// <param name="StCmd">AT指令标准结束标识</param>
        /// <returns></returns>
        private string ATCommand(string ATCmd, string StCmd)
        {
            string response = "";
            int i;
            if (!ATCmd.EndsWith("\x01a"))
            {
                if (!(ATCmd.EndsWith("\r") || ATCmd.EndsWith("\r\n")))
                {
                    ATCmd = ATCmd + "\r";
                }
            }
            Sp.WriteLine(ATCmd);
            //第一次读响应数据
            if (Sp.BytesToRead > 0)
            {
                response = Sp.ReadExisting();
                //去除前端多可能多读取的字符
                if (response.IndexOf(ATCmd) > 0)
                {
                    response = response.Substring(response.IndexOf(ATCmd));
                }
                else
                {
                }
                if (response == "" || response.IndexOf(StCmd) < 0)
                {
                    if (response != "")
                    {
                        if (response.Trim() == "ERROR")
                        {
                            //throw vError = new UnknowException("Unknown exception in sending command:" + ATCmd);
                        }
                        if (response.IndexOf("+CMS ERROR") >= 0)
                        {
                            string[] cols = new string[100];
                            cols = response.Split(';');
                            if (cols.Length > 1)
                            {
                                string errorCode = cols[1];
                            }
                        }
                    }
                }
            }
            //读第一次没有读完的响应数据，直到读到特征数据或超时
            for (i = 0; i < 3; i++)
            {
                Thread.Sleep(1000);
                response = response + Sp.ReadExisting();
                if (response.IndexOf(StCmd) >= 0)
                {
                    break;
                }
            }
            return response;
        }

        /// <summary>
        /// 从注册表获取系统串口列表
        /// </summary>
        private void GetComList()
        {
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                this.cmID.Items.Clear();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    this.cmID.Items.Add(sValue);
                }
                //默认取得下拉第一个
                if (this.cmID.Items.Count > 0)
                {
                    this.cmID.Text = this.cmID.Items[0].ToString();
                }
            }
        }
        #endregion

    }
}
