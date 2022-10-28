using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Security.Cryptography;

namespace DISTR_BLE
{
    class BleCore
    {
        private bool asyncLock = false;

        private DeviceWatcher deviceWatcher = null;

        public GattDeviceServicesResult ServicesResult { get; private set; }

        /// <summary>
        /// 當前連接的服務
        /// </summary>
        public GattDeviceService CurrentService { get; private set; }

        /// <summary>
        /// 當前連接的藍芽設備
        /// </summary>
        public BluetoothLEDevice CurrentDevice { get; private set; }

        /// <summary>
        /// 寫特徵對象
        /// </summary>
        public GattCharacteristic CurrentWriteCharacteristic { get; private set; }

        /// <summary>
        /// 通知特徵對象
        /// </summary>
        public GattCharacteristic CurrentNotifyCharacteristic { get; private set; }

        /// <summary>
        /// 存取搜索到的設備
        /// </summary>
        public List<BluetoothLEDevice> DeviceList { get; private set; }

        /// <summary>
        /// 特性通知類型通知啟用
        /// </summary>
        private const GattClientCharacteristicConfigurationDescriptorValue CHARACTERISTIC_NOTIFICATION_TYPE = GattClientCharacteristicConfigurationDescriptorValue.Notify;

        /// <summary>
        /// 定義搜索藍芽設備委託
        /// </summary>
        public delegate void DeviceWatcherChangedEvent(BluetoothLEDevice bluetoothLEDevice);

        /// <summary>
        ///搜索藍芽事件
        /// </summary>
        public event DeviceWatcherChangedEvent DeviceWatcherChanged;

        /// <summary>
        /// 獲取服務委託
        /// </summary>
        public delegate void CharacteristicFinishEvent(int size);

        /// <summary>
        ///獲取服務事件
        /// </summary>
        public event CharacteristicFinishEvent CharacteristicFinish;

        /// <summary>
        /// 獲取特徵委託
        /// </summary>
        public delegate void CharacteristicAddedEvent(GattCharacteristic gattCharacteristic);

        /// <summary>
        /// 獲取特徵事件
        /// </summary>
        public event CharacteristicAddedEvent CharacteristicAdded;

        /// <summary>
        /// 接受數據委託
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        public delegate void RecDataEvent(GattCharacteristic sender, byte[] data);

        /// <summary>
        /// 接受數據事件
        /// </summary>
        public event RecDataEvent Recdate;

        /// <summary>
        /// 當前連接的藍芽
        /// </summary>
        private string CurrentDeviceMAC { get; set; }

        private BluetoothLEAdvertisementWatcher Watcher = null;

        public string SCALE_INFO = System.Reflection.Assembly.GetExecutingAssembly().Location + @"\" + "_ScaleInfo.txt";

        public string weight = string.Empty;

        public BleCore()
        {
            DeviceList = new List<BluetoothLEDevice>();
        }

        /// <summary>
        /// 偵測藍芽裝置初始化
        /// </summary>
        public void StartBleDeviceWatcher()
        {
            Watcher = new BluetoothLEAdvertisementWatcher();

            Watcher.ScanningMode = BluetoothLEScanningMode.Active;
            // only activate the watcher when we're recieving values >= -80
            Watcher.SignalStrengthFilter.InRangeThresholdInDBm = -80;

            // stop watching if the value drops below -90 (user walked away)
            Watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -90;

            // register callback for when we see an advertisements
            Watcher.Received += OnAdvertisementReceived;

            // wait 5 seconds to make sure the device is really out of range
            Watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(5000);
            Watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(2000);

            // starting watching for advertisements
            Watcher.Start();
            WriteLog("自動發現設備中");
        }

        /// <summary>
        /// 停止搜索藍芽
        /// </summary>
        public void StopBleDeviceWatcher()
        {
            if (Watcher != null)
                this.Watcher.Stop();
        }

        /// <summary>
        /// 斷開連接
        /// </summary>
        /// <returns></returns>
        public void Dispose()
        {
            //CurrentNotifyCharacteristic?.Service?.Dispose();
            CurrentService?.Session.Dispose();
            CurrentDeviceMAC = null;
            CurrentService?.Dispose();
            CurrentDevice?.Dispose();
            CurrentDevice = null;
            CurrentService = null;
            CurrentWriteCharacteristic = null;
            CurrentNotifyCharacteristic = null;
            WriteLog("藍芽設備已斷開");

            //CurrentService.Device.
            //foreach (var service in CurrentService.GetIncludedServices("123"))

        }

        //private async Task<bool> ClearBluetoothLEDeviceAsync()
        //{
        //    ...

        //    foreach (var ser in ServiceCollection)
        //    {
        //        ser.service?.Dispose();
        //    }

        //    bluetoothLeDevice?.Dispose();
        //    bluetoothLeDevice = null;
        //    return true;
        //}



        /// <summary>
        /// 配對
        /// </summary>
        /// <param name="Device"></param>
        public void StartMatching(BluetoothLEDevice Device)
        {
            this.CurrentDevice = Device;
        }

        /// <summary>
        /// 發送數據接口
        /// </summary>
        /// <returns></returns>
        public void Write(byte[] data)
        {
            if (CurrentWriteCharacteristic != null)
            {
                CurrentWriteCharacteristic.WriteValueAsync(CryptographicBuffer.CreateFromByteArray(data), GattWriteOption.WriteWithResponse).Completed = (asyncInfo, asyncStatus) =>
                {
                    if (asyncStatus == AsyncStatus.Completed)
                    {
                        GattCommunicationStatus a = asyncInfo.GetResults();
                        //Console.WriteLine("發送數據：" + BitConverter.ToString(data) + " State : " + a);
                    }                    
                };
            }
        }

        public async void GetBleData()
        {
            var result = await this.CurrentDevice.GetGattServicesAsync();
            foreach (var service in result.Services)
            {
                var chrresult = await service.GetCharacteristicsAsync();
                foreach (var chr in chrresult.Characteristics)
                {
                    var dscresult = await chr.GetDescriptorsAsync();
                    //Console.WriteLine(dscresult);
                }
            }
        }

        /// <summary>
        /// 取得藍芽服務
        /// </summary>
        public void FindService()
        {
            this.CurrentDevice.GetGattServicesAsync().Completed = (asyncInfo, asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Completed)
                {
                    var services = asyncInfo.GetResults().Services;
                    Console.WriteLine("GattServices size=" + services.Count);
                    foreach (GattDeviceService ser in services)
                    {
                        FindCharacteristic(ser);
                    }
                    CharacteristicFinish?.Invoke(services.Count);
                }
            };
        }

        /// <summary>
        /// 以ID查找設備
        /// </summary>
        public void SelectDeviceFromIdAsync(string MAC)
        {
            CurrentDeviceMAC = MAC;
            CurrentDevice = null;
            BluetoothAdapter.GetDefaultAsync().Completed = (asyncInfo, asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Completed)
                {
                    BluetoothAdapter mBluetoothAdapter = asyncInfo.GetResults();
                    byte[] _Bytes1 = BitConverter.GetBytes(mBluetoothAdapter.BluetoothAddress);
                    Array.Reverse(_Bytes1);
                    string macAddress = BitConverter.ToString(_Bytes1, 2, 6).Replace('-', ':').ToLower();
                    string Id = "BluetoothLE#BluetoothLE" + macAddress + "-" + MAC;
                    Matching(Id);
                }
            };
        }

        /// <summary>
        /// 獲取操作
        /// </summary>
        /// <returns></returns>
        public void SetOpteron(GattCharacteristic gattCharacteristic)
        {
            byte[] _Bytes1 = BitConverter.GetBytes(this.CurrentDevice.BluetoothAddress);
            Array.Reverse(_Bytes1);
            this.CurrentDeviceMAC = BitConverter.ToString(_Bytes1, 2, 6).Replace('-', ':').ToLower();

            string msg = "正在連接設備<" + this.CurrentDeviceMAC + ">..";
            WriteLog(msg);

            if (gattCharacteristic.CharacteristicProperties == GattCharacteristicProperties.Write)
            {
                this.CurrentWriteCharacteristic = gattCharacteristic;
            }
            if (gattCharacteristic.CharacteristicProperties == GattCharacteristicProperties.Notify)
            {
                this.CurrentNotifyCharacteristic = gattCharacteristic;
            }
            if ((uint)gattCharacteristic.CharacteristicProperties == 26)
            {

            }
            if (gattCharacteristic.CharacteristicProperties == GattCharacteristicProperties.Read)
            {
                this.CurrentNotifyCharacteristic = gattCharacteristic;
            }
            if (gattCharacteristic.CharacteristicProperties == (GattCharacteristicProperties.Read | GattCharacteristicProperties.WriteWithoutResponse | GattCharacteristicProperties.Write | GattCharacteristicProperties.Notify | GattCharacteristicProperties.Indicate))
            {
                this.CurrentWriteCharacteristic = gattCharacteristic;
                this.CurrentNotifyCharacteristic = gattCharacteristic;
                this.CurrentNotifyCharacteristic.ProtectionLevel = GattProtectionLevel.Plain;
                this.CurrentNotifyCharacteristic.ValueChanged += Characteristic_ValueChanged;
                this.CurrentDevice.ConnectionStatusChanged += this.CurrentDevice_ConnectionStatusChanged;
                this.EnableNotifications(CurrentNotifyCharacteristic);
            }
        }

        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            BluetoothLEDevice.FromBluetoothAddressAsync(eventArgs.BluetoothAddress).Completed = (asyncInfo, asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Completed)
                {
                    if (asyncInfo.GetResults() != null)
                    {
                        BluetoothLEDevice currentDevice = asyncInfo.GetResults();
                        if (DeviceList.FindIndex((x) => { return x.Name.Equals(currentDevice.Name); }) < 0)
                        {
                            this.DeviceList.Add(currentDevice);
                            DeviceWatcherChanged?.Invoke(currentDevice);
                        }
                    }
                }
            };
        }

        private void FindCharacteristic(GattDeviceService gattDeviceService)
        {
            this.CurrentService = gattDeviceService;
            this.CurrentService.GetCharacteristicsAsync().Completed = (asyncInfo, asyncStatus) =>
            {
                if (asyncStatus == AsyncStatus.Completed)
                {
                    var services = asyncInfo.GetResults().Characteristics;
                    foreach (var c in services)
                    {
                        this.CharacteristicAdded?.Invoke(c);
                    }
                }
            };
        }

        /// <summary>
        /// 搜尋到的藍芽設備
        /// </summary>
        /// <returns></returns>
        private void Matching(string Id)
        {
            try
            {
                BluetoothLEDevice.FromIdAsync(Id).Completed = (asyncInfo, asyncStatus) =>
                {
                    if (asyncStatus == AsyncStatus.Completed)
                    {
                        BluetoothLEDevice bleDevice = asyncInfo.GetResults();
                        this.DeviceList.Add(bleDevice);
                    }
                    if (asyncStatus == AsyncStatus.Started)
                    {
                        WriteLog(asyncStatus.ToString());
                    }
                    if (asyncStatus == AsyncStatus.Canceled)
                    {
                        WriteLog(asyncStatus.ToString());
                    }
                    if (asyncStatus == AsyncStatus.Error)
                    {
                        WriteLog(asyncStatus.ToString());
                    }
                };
            }
            catch (Exception e)
            {
                string msg = "沒有發現設備" + e.ToString();
                WriteLog(msg);
                this.StartBleDeviceWatcher();
            }
        }

        /// <summary>
        /// 藍芽連接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CurrentDevice_ConnectionStatusChanged(BluetoothLEDevice sender, object args)
        {
            if (sender.ConnectionStatus == BluetoothConnectionStatus.Disconnected && CurrentDeviceMAC != null)
            {
                if (!asyncLock)
                {
                    asyncLock = true;
                    //this.StartBleDeviceWatcher();
                    //WriteLog("設備已斷開");
                   Dispose();
                }
            }
            else
            {
                if (!asyncLock)
                {
                    asyncLock = true;
                    WriteLog("設備已連接");
                }
            }
        }

        /// <summary>
        /// 設置特徵對象為接收通知對象
        /// </summary>
        /// <param name="characteristic"></param>
        /// <returns></returns>
        private void EnableNotifications(GattCharacteristic characteristic)
        {
            //Console.WriteLine("收通知對象=" + CurrentDevice.Name + ":" + CurrentDevice.ConnectionStatus);
            characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(CHARACTERISTIC_NOTIFICATION_TYPE).Completed = (asyncInfo, asyncStatus) =>
            {
                if (asyncStatus != AsyncStatus.Completed)
                {
                    GattCommunicationStatus status = asyncInfo.GetResults();
                    if (status == GattCommunicationStatus.Unreachable)
                    {
                        WriteLog(CurrentDevice.Name + " 設備不可用");
                        if (CurrentNotifyCharacteristic != null && !asyncLock)
                        {
                            this.EnableNotifications(CurrentNotifyCharacteristic);
                        }
                        return;
                    }
                    asyncLock = false;
                    WriteLog(CurrentDevice.Name + " 設備連接狀態"+ status);
                }
            };
        }

        /// <summary>
        /// 接收到的藍芽數據
        /// </summary>
        private void Characteristic_ValueChanged(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            byte[] data;
            CryptographicBuffer.CopyToByteArray(args.CharacteristicValue, out data);
            Recdate?.Invoke(sender, data);
        }

        private void BLE_Connection()
        {
            string[] requestproperties = { "System.Devices.Aep.DeviceAddress", "System.Devices.Aep.Isconnected" };
            deviceWatcher = DeviceInformation.CreateWatcher(
                BluetoothLEDevice.GetDeviceSelectorFromPairingState(false),
                requestproperties,
                DeviceInformationKind.AssociationEndpoint);
        }
        public  void WriteLog(string message)
        {
            string DIRNAME = AppDomain.CurrentDomain.BaseDirectory + @"\Log\";
            string FILENAME = DIRNAME + DateTime.Now.ToString("yyyyMMdd") + ".txt";

            if (!Directory.Exists(DIRNAME))
                Directory.CreateDirectory(DIRNAME);

            if (!File.Exists(FILENAME))
            {
                File.Create(FILENAME).Close();
            }
            using (StreamWriter sw = File.AppendText(FILENAME))
            {
                Log(message, sw);
            }
        }

        private  void Log(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}
