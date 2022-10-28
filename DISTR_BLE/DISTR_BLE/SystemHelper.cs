using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DISTR_BLE
{
    class SystemHelper
    {
        private SystemHelper() { }

        /// <summary>
        /// 設置程式開機啟動
        /// </summary>
        /// <param name="strAppPath">應用程式exe所在文件夾</param>
        /// <param name="strAppName">應用程式exe名稱</param>
        /// <param name="bIsAutoRun">自動運行狀態</param>
        public static void SetAutoRun(string strAppPath, string strAppName, bool bIsAutoRun)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strAppPath)
                    || string.IsNullOrWhiteSpace(strAppName))
                {
                    throw new Exception("應用程式路徑或名稱為空！");
                }

                RegistryKey reg = Registry.LocalMachine;
                RegistryKey run = reg.CreateSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\");

                if (bIsAutoRun)
                {
                    run.SetValue(strAppName, strAppPath);
                }
                else
                {
                    if (null != run.GetValue(strAppName))
                    {
                        run.DeleteValue(strAppName);
                    }
                }

                run.Close();
                reg.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 判斷是否開機啟動
        /// </summary>
        /// <param name="strAppPath">應用程式路徑</param>
        /// <param name="strAppName">應用程式名稱</param>
        /// <returns></returns>
        public static bool IsAutoRun(string strAppPath, string strAppName)
        {
            try
            {
                RegistryKey reg = Registry.LocalMachine;
                RegistryKey software = reg.OpenSubKey(@"SOFTWARE");
                RegistryKey run = reg.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\");
                object key = run.GetValue(strAppName);
                software.Close();
                run.Close();
                if (null == key || !strAppPath.Equals(key.ToString()))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
