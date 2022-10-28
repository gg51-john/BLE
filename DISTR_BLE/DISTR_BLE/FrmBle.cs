using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;

namespace DISTR_BLE
{
    public partial class FrmBle : Form
    {
        private static BleCore bleCore = null;

        private static List<GattCharacteristic> characteristics = new List<GattCharacteristic>();

        List<BluetoothLEDevice> lstCurrentDevice = new List<BluetoothLEDevice>();

        private System.Windows.Forms.ContextMenu contextMenu1;

        private System.Windows.Forms.MenuItem menuItem1;

        /// <summary>
        /// 裝置名稱
        /// </summary>
        private string _deviceName = string.Empty;

        /// <summary>
        /// 裝置ID
        /// </summary>
        private string  _deviceID = string.Empty;

        /// <summary>
        /// 寫入檔案路徑
        /// </summary>
        private string _path = string.Empty;

        /// <summary>
        /// 是否自動執行
        /// </summary>
        private bool _autoScan = false;

        /// <summary>
        /// 是否縮放到右下
        /// </summary>
        private bool _notification = false;


        public FrmBle()
        {
            InitializeComponent();
        }

        private void FrmBle_Load(object sender, EventArgs e)
        {
            try
            {
                this.contextMenu1 = new System.Windows.Forms.ContextMenu();
                this.menuItem1 = new System.Windows.Forms.MenuItem();
                this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { this.menuItem1 });

                this.menuItem1.Index = 0;
                this.menuItem1.Text = "結束";
                this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);

                notifyIcon1.ContextMenu = this.contextMenu1;

                //開機自動執行(縮小至右下)
                if (CheckAutoRun())
                {
                    FormSizeSetting();
                }

                string strFilePath = Application.ExecutablePath;
                string localPath = System.IO.Path.GetDirectoryName(strFilePath);
                //讀取.ini
                IniFile MyIni = new IniFile(localPath + @"\weight.ini");
                string deviceName = MyIni.Read("DeviceName");
                string deviceID = MyIni.Read("DeviceID");
                string filePath = MyIni.Read("FilePath");
                if (string.IsNullOrEmpty(deviceName) || string.IsNullOrEmpty(deviceID) || string.IsNullOrEmpty(filePath))
                {
                    MessageBox.Show("尚未設置藍芽裝置設定");
                    return;
                }
                else
                {
                    //是否驗證檔案路徑??
                    this._deviceName = deviceName;
                    this._deviceID = deviceID;
                    this._path = filePath;
                    this._autoScan = true;
                }
                ShowSettingInfo(deviceName, deviceID, filePath);                
                InitDeviceWatcher();
            }
            catch (Exception ex)
            {
                bleCore.WriteLog(ex.Message);
            }
        }

        #region 小圖示
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            ShowForm();
        }

        /// <summary>
        /// 顯示出主畫面
        /// </summary>
        private void ShowForm()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //如果目前是縮小狀態，才要回覆成一般大小的視窗
                this.Show();
                this.WindowState = FormWindowState.Normal;
                _notification = false;
            }
            // Activate the form.
            this.Activate();
            this.Focus();
        }

        private void FormSizeSetting()
        {
            this.WindowState = FormWindowState.Minimized;
            notifyIcon1.Tag = string.Empty;
            _notification = true;
            //notifyIcon1.ShowBalloonTip(3000, this.Text,
            //     "程式並未結束，要結束請在圖示上按右鍵，選取結束功能!",
            //     ToolTipIcon.Info);
            this.Hide();
        }

        /// <summary>
        /// 使用者按下視窗關閉，把它最小化就好
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmBle_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                e.Cancel = true;
                FormSizeSetting();
            }
        }

        private void menuItem1_Click(object Sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Close();
            this.notifyIcon1.Dispose();
        }

        #endregion

        #region 開機自動執行設定
        /// <summary>
        /// 設置程式開機自啟動
        /// </summary>
        private void SetAutoRun()
        {
            string strFilePath = Application.ExecutablePath;
            string strFileName = System.IO.Path.GetFileName(strFilePath);
            try
            {
                SystemHelper.SetAutoRun(strFilePath + " -autostart", strFileName, menuAutoRun.Checked);
                //menuAutoRun.Checked = !menuAutoRun.Checked;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "錯誤提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 檢查是否開機啟動，並設置控制項狀態
        /// </summary>
        private bool CheckAutoRun()
        {
            string strFilePath = Application.ExecutablePath;
            string strFileName = System.IO.Path.GetFileName(strFilePath);

            bool autoRun = SystemHelper.IsAutoRun(strFilePath + " -autostart", strFileName);
            menuAutoRun.Checked = autoRun;
            return autoRun;
        }

        #endregion

        /// <summary>
        /// 顯示設置資料
        /// </summary>
        /// <param name="deviceName"></param>
        /// <param name="deviceID"></param>
        /// <param name="filePath"></param>
        private void ShowSettingInfo(string deviceName, string deviceID, string filePath)
        {
            this.txtDeviceName.Text = deviceName;
            this.txtDeviceID.Text = deviceID;
            this.txtFilePath.Text = filePath;
        }

        /// <summary>
        /// 初始化藍芽搜尋
        /// </summary>
        private void InitDeviceWatcher()
        {
            try
            {
                bleCore = new BleCore();
                bleCore.DeviceWatcherChanged += DeviceWatcherChanged;
                bleCore.CharacteristicAdded += CharacteristicAdded;
                bleCore.CharacteristicFinish += CharacteristicFinish;
                bleCore.Recdate += Recdata;
                bleCore.StartBleDeviceWatcher();
            }
            catch (Exception ex)
            {
                bleCore.WriteLog(ex.Message);
            }
        }

        private static void CharacteristicFinish(int size)
        {
            if (size <= 0)
            {
                return;
            }
        }

        /// <summary>
        /// 寫入接收到的資料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private  void Recdata(GattCharacteristic sender, byte[] data)
        {
            try
            {
                if (data.Length > 10)
                {
                    //增加一暫存變數
                    string str = Encoding.UTF7.GetString(data).Remove(0, 2).Trim();
                    if (bleCore.weight != str)
                    {
                        bleCore.weight = str;
                        lblWeight.InvokeIfRequired(() =>
                        {
                            lblWeight.Text = bleCore.weight;
                        });
                        if (_autoScan)
                        {
                            File.WriteAllLines(_path, new string[1] { bleCore.weight });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                bleCore.WriteLog(ex.Message);
            }
        }

        /// <summary>
        ///  偵測藍芽裝置ID
        /// </summary>
        /// <param name="gatt"></param>
        private void CharacteristicAdded(GattCharacteristic gatt)
        {
            try
            {
                string[] biligiler = { gatt.Uuid.ToString() };
                ListViewItem lst = new ListViewItem(biligiler);
                LstViewDeviceID.InvokeIfRequired(() =>
                {
                    LstViewDeviceID.Items.Add(lst);
                });
                characteristics.Add(gatt);
            }
            catch (Exception ex)
            {
                bleCore.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// 偵測藍芽裝置Changed事件
        /// </summary>
        /// <param name="currentDevice"></param>
        private void DeviceWatcherChanged(BluetoothLEDevice currentDevice)
        {
            try
            {
                byte[] _Bytes1 = BitConverter.GetBytes(currentDevice.BluetoothAddress);
                Array.Reverse(_Bytes1);
                string[] biligiler = { currentDevice.Name };
                ListViewItem lst = new ListViewItem(biligiler);
                LstViewDevice.InvokeIfRequired(() =>
                {
                    LstViewDevice.Items.Add(lst);
                });
                if (!lstCurrentDevice.Contains(currentDevice))
                {
                    lstCurrentDevice.Add(currentDevice);
                }
                if (_autoScan)
                {
                    if (currentDevice.Name.Equals(this._deviceName))
                    {
                        ConnectDeviceByName();
                    }
                }
            }
            catch (Exception ex)
            {
                bleCore.WriteLog(ex.Message);
            }
        }      
        
        /// <summary>
        /// 以藍芽名稱連線藍芽設備
        /// </summary>
        private void ConnectDeviceByName()
        {
            try
            {
                if (!_autoScan)
                {
                    if (this.LstViewDevice.SelectedItems.Count > 0)
                    {
                        //選取的裝置名稱
                        _deviceName = LstViewDevice.SelectedItems[0].Text;
                    }
                }
                //選取的裝置名稱
                BluetoothLEDevice currentDevice = lstCurrentDevice.Where(n => n.Name == _deviceName).FirstOrDefault();
                ConnectDevice(currentDevice);
            }
            catch (Exception ex)
            {
                bleCore.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// 以藍芽ID連線藍芽設備
        /// </summary>
        public void ConnectDeviceID()
        {
            try
            {
                //"bef8d6c9-9c21-4c9e-b632-bd58c1009f9f"
                if (!_autoScan)
                {
                    if (this.LstViewDeviceID.SelectedItems.Count > 0)
                    {
                        _deviceID = this.LstViewDeviceID.SelectedItems[0].Text;
                    }
                }
                GattCharacteristic gattCharacteristic = characteristics.Find((x) => { return x.Uuid.Equals(new Guid(_deviceID)); });
                if (gattCharacteristic != null)
                {
                    bleCore.SetOpteron(gattCharacteristic);
                    bleCore.Write(new byte[] { 0x01, 0xFF, 0xFF, 0xFF, 0xFF, });
                }
                else
                {
                    throw new Exception("無法獲取UUID服務");
                }
            }
            catch(Exception ex)
            {
                bleCore.WriteLog(ex.Message);
            }           
        }

        /// <summary>
        /// 連線選取的裝置
        /// </summary>
        /// <param name="Device"></param>
        private  void ConnectDevice(BluetoothLEDevice Device)
        {
            try
            {
                characteristics.Clear();
                bleCore.StopBleDeviceWatcher();
                bleCore.StartMatching(Device);
                bleCore.FindService();
                if (_autoScan)
                {
                    ConnectDeviceID();
                }
            }
            catch(Exception ex)
            {
                bleCore.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// 儲存
        /// </summary>
        public void DoSave()
        {
            try
            {
                string localPath = System.IO.Directory.GetCurrentDirectory();

                IniFile MyIni = new IniFile(localPath + @"\weight.ini");

                if (LstViewDevice.SelectedItems.Count == 0)
                {
                    MessageBox.Show("請選擇裝置名稱");
                    return;
                }
                if (LstViewDeviceID.SelectedItems.Count == 0)
                {
                    MessageBox.Show("請選擇裝置ID");
                    return;
                }
                if (string.IsNullOrEmpty(txtSettingFilePath.Text))
                {
                    MessageBox.Show("請選擇裝重量寫入路徑");
                    return;
                }
                MyIni.Write("DeviceName", LstViewDevice.SelectedItems[0].Text);
                MyIni.Write("DeviceID", LstViewDeviceID.SelectedItems[0].Text);
                MyIni.Write("FilePath", txtSettingFilePath.Text);
                ShowSettingInfo(LstViewDevice.SelectedItems[0].Text, LstViewDeviceID.SelectedItems[0].Text, txtSettingFilePath.Text);

                SetAutoRun();

                MessageBox.Show("儲存成功，裝置將重新啟動");
                Application.Restart();
                notifyIcon1.Dispose();
                Environment.Exit(0);

            }
            catch (Exception ex)
            {
                bleCore.WriteLog(ex.Message);
            }
        }

        private void Reset()
        {
            
        }

        #region 事件
        private void btnSearch_Click(object sender, EventArgs e)
        {
            InitDeviceWatcher();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectDeviceByName();
        }

        private void btnConnectID_Click(object sender, EventArgs e)
        {
            ConnectDeviceID();
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Title = "磅秤重量檔路徑";
                fileDialog.Filter = "Txt files (*.txt)|*.txt";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (!File.Exists(fileDialog.FileName))
                    {
                        File.Create(fileDialog.FileName);
                    }
                    this.txtSettingFilePath.Text = fileDialog.FileName;
                }
            }
            catch (Exception ex)
            {
                bleCore.WriteLog(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DoSave();
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            bleCore.StopBleDeviceWatcher();
            LstViewDevice.Items.Clear();
            LstViewDeviceID.Items.Clear();
            lstCurrentDevice.Clear();
            txtSettingFilePath.Text = string.Empty;
            bleCore.Dispose();
        }
        #endregion

        private void FrmBle_Resize(object sender, EventArgs e)
        {
                this.ShowInTaskbar = !_notification;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string id =  bleCore.CurrentDevice.DeviceId;
            string name = bleCore.CurrentService.Device.BluetoothDeviceId.Id; ;
            var state = bleCore.CurrentNotifyCharacteristic.Service.Session.SessionStatus;
        }
    }
    public static class Extension
    {
        //非同步委派更新UI
        public static void InvokeIfRequired(
            this Control control, MethodInvoker action)
        {
            if (control.InvokeRequired)//在非當前執行緒內 使用委派
            {
                control.Invoke(action);
            }
            else
            {
                action();
            }
        }        
    }
}
