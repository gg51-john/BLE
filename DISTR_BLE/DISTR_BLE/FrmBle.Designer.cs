
namespace DISTR_BLE
{
    partial class FrmBle
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBle));
            this.LstViewDevice = new System.Windows.Forms.ListView();
            this.DeviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.LstViewDeviceID = new System.Windows.Forms.ListView();
            this.DeviceID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnConnectID = new System.Windows.Forms.Button();
            this.btnPath = new System.Windows.Forms.Button();
            this.txtSettingFilePath = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabSetting = new System.Windows.Forms.TabPage();
            this.menuAutoRun = new System.Windows.Forms.CheckBox();
            this.btnReset = new System.Windows.Forms.Button();
            this.lblWeight = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tabInfo = new System.Windows.Forms.TabPage();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.txtDeviceID = new System.Windows.Forms.TextBox();
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.lblDeviceID = new System.Windows.Forms.Label();
            this.lblDeviceName = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabSetting.SuspendLayout();
            this.tabInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // LstViewDevice
            // 
            this.LstViewDevice.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DeviceName});
            this.LstViewDevice.FullRowSelect = true;
            this.LstViewDevice.GridLines = true;
            this.LstViewDevice.HideSelection = false;
            this.LstViewDevice.Location = new System.Drawing.Point(29, 92);
            this.LstViewDevice.Name = "LstViewDevice";
            this.LstViewDevice.Size = new System.Drawing.Size(247, 358);
            this.LstViewDevice.TabIndex = 0;
            this.LstViewDevice.UseCompatibleStateImageBehavior = false;
            this.LstViewDevice.View = System.Windows.Forms.View.Details;
            // 
            // DeviceName
            // 
            this.DeviceName.Text = "裝置名稱";
            this.DeviceName.Width = 240;
            // 
            // LstViewDeviceID
            // 
            this.LstViewDeviceID.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DeviceID});
            this.LstViewDeviceID.FullRowSelect = true;
            this.LstViewDeviceID.GridLines = true;
            this.LstViewDeviceID.HideSelection = false;
            this.LstViewDeviceID.Location = new System.Drawing.Point(296, 92);
            this.LstViewDeviceID.Name = "LstViewDeviceID";
            this.LstViewDeviceID.Size = new System.Drawing.Size(246, 358);
            this.LstViewDeviceID.TabIndex = 1;
            this.LstViewDeviceID.UseCompatibleStateImageBehavior = false;
            this.LstViewDeviceID.View = System.Windows.Forms.View.Details;
            // 
            // DeviceID
            // 
            this.DeviceID.Text = "裝置ID";
            this.DeviceID.Width = 240;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(118, 19);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 2;
            this.btnConnect.Text = "連線裝置";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnConnectID
            // 
            this.btnConnectID.Location = new System.Drawing.Point(206, 19);
            this.btnConnectID.Name = "btnConnectID";
            this.btnConnectID.Size = new System.Drawing.Size(75, 23);
            this.btnConnectID.TabIndex = 3;
            this.btnConnectID.Text = "連線裝置ID";
            this.btnConnectID.UseVisualStyleBackColor = true;
            this.btnConnectID.Click += new System.EventHandler(this.btnConnectID_Click);
            // 
            // btnPath
            // 
            this.btnPath.Location = new System.Drawing.Point(30, 53);
            this.btnPath.Name = "btnPath";
            this.btnPath.Size = new System.Drawing.Size(75, 23);
            this.btnPath.TabIndex = 5;
            this.btnPath.Text = "存取路徑：";
            this.btnPath.UseVisualStyleBackColor = true;
            this.btnPath.Click += new System.EventHandler(this.btnPath_Click);
            // 
            // txtSettingFilePath
            // 
            this.txtSettingFilePath.Location = new System.Drawing.Point(118, 54);
            this.txtSettingFilePath.Name = "txtSettingFilePath";
            this.txtSettingFilePath.Size = new System.Drawing.Size(151, 22);
            this.txtSettingFilePath.TabIndex = 6;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(467, 19);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "儲存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabSetting);
            this.tabControl.Controls.Add(this.tabInfo);
            this.tabControl.Location = new System.Drawing.Point(6, 5);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(572, 489);
            this.tabControl.TabIndex = 8;
            // 
            // tabSetting
            // 
            this.tabSetting.Controls.Add(this.button1);
            this.tabSetting.Controls.Add(this.menuAutoRun);
            this.tabSetting.Controls.Add(this.btnReset);
            this.tabSetting.Controls.Add(this.lblWeight);
            this.tabSetting.Controls.Add(this.btnSearch);
            this.tabSetting.Controls.Add(this.LstViewDevice);
            this.tabSetting.Controls.Add(this.LstViewDeviceID);
            this.tabSetting.Controls.Add(this.btnSave);
            this.tabSetting.Controls.Add(this.txtSettingFilePath);
            this.tabSetting.Controls.Add(this.btnConnect);
            this.tabSetting.Controls.Add(this.btnPath);
            this.tabSetting.Controls.Add(this.btnConnectID);
            this.tabSetting.Location = new System.Drawing.Point(4, 22);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.Padding = new System.Windows.Forms.Padding(3);
            this.tabSetting.Size = new System.Drawing.Size(564, 463);
            this.tabSetting.TabIndex = 0;
            this.tabSetting.Text = "設置";
            this.tabSetting.UseVisualStyleBackColor = true;
            // 
            // menuAutoRun
            // 
            this.menuAutoRun.AutoSize = true;
            this.menuAutoRun.Location = new System.Drawing.Point(285, 57);
            this.menuAutoRun.Name = "menuAutoRun";
            this.menuAutoRun.Size = new System.Drawing.Size(96, 16);
            this.menuAutoRun.TabIndex = 11;
            this.menuAutoRun.Text = "開機自動執行";
            this.menuAutoRun.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(294, 19);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "重設";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Location = new System.Drawing.Point(417, 58);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(0, 12);
            this.lblWeight.TabIndex = 9;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(30, 19);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 8;
            this.btnSearch.Text = "搜尋裝置";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tabInfo
            // 
            this.tabInfo.Controls.Add(this.txtFilePath);
            this.tabInfo.Controls.Add(this.txtDeviceID);
            this.tabInfo.Controls.Add(this.txtDeviceName);
            this.tabInfo.Controls.Add(this.lblFilePath);
            this.tabInfo.Controls.Add(this.lblDeviceID);
            this.tabInfo.Controls.Add(this.lblDeviceName);
            this.tabInfo.Location = new System.Drawing.Point(4, 22);
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tabInfo.Size = new System.Drawing.Size(564, 463);
            this.tabInfo.TabIndex = 1;
            this.tabInfo.Text = "設置資料";
            this.tabInfo.UseVisualStyleBackColor = true;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(113, 136);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(256, 22);
            this.txtFilePath.TabIndex = 5;
            // 
            // txtDeviceID
            // 
            this.txtDeviceID.Location = new System.Drawing.Point(113, 86);
            this.txtDeviceID.Name = "txtDeviceID";
            this.txtDeviceID.ReadOnly = true;
            this.txtDeviceID.Size = new System.Drawing.Size(256, 22);
            this.txtDeviceID.TabIndex = 4;
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.Location = new System.Drawing.Point(113, 37);
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.ReadOnly = true;
            this.txtDeviceName.Size = new System.Drawing.Size(256, 22);
            this.txtDeviceName.TabIndex = 3;
            // 
            // lblFilePath
            // 
            this.lblFilePath.AutoSize = true;
            this.lblFilePath.Location = new System.Drawing.Point(24, 141);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(77, 12);
            this.lblFilePath.TabIndex = 2;
            this.lblFilePath.Text = "重量檔路徑：";
            // 
            // lblDeviceID
            // 
            this.lblDeviceID.AutoSize = true;
            this.lblDeviceID.Location = new System.Drawing.Point(24, 91);
            this.lblDeviceID.Name = "lblDeviceID";
            this.lblDeviceID.Size = new System.Drawing.Size(53, 12);
            this.lblDeviceID.TabIndex = 1;
            this.lblDeviceID.Text = "裝置ID：";
            // 
            // lblDeviceName
            // 
            this.lblDeviceName.AutoSize = true;
            this.lblDeviceName.Location = new System.Drawing.Point(24, 42);
            this.lblDeviceName.Name = "lblDeviceName";
            this.lblDeviceName.Size = new System.Drawing.Size(65, 12);
            this.lblDeviceName.TabIndex = 0;
            this.lblDeviceName.Text = "裝置名稱：";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(375, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "state";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FrmBle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 499);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FrmBle";
            this.Text = "藍芽連接設定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmBle_FormClosing);
            this.Load += new System.EventHandler(this.FrmBle_Load);
            this.Resize += new System.EventHandler(this.FrmBle_Resize);
            this.tabControl.ResumeLayout(false);
            this.tabSetting.ResumeLayout(false);
            this.tabSetting.PerformLayout();
            this.tabInfo.ResumeLayout(false);
            this.tabInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView LstViewDevice;
        private System.Windows.Forms.ColumnHeader DeviceName;
        private System.Windows.Forms.ListView LstViewDeviceID;
        private System.Windows.Forms.ColumnHeader DeviceID;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnConnectID;
        private System.Windows.Forms.Button btnPath;
        private System.Windows.Forms.TextBox txtSettingFilePath;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabSetting;
        private System.Windows.Forms.TabPage tabInfo;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.TextBox txtDeviceID;
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.Label lblDeviceID;
        private System.Windows.Forms.Label lblDeviceName;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.CheckBox menuAutoRun;
        private System.Windows.Forms.Button button1;
    }
}

