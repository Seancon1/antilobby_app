﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2.Alert
{

    class AlertAction
    {
        Alert alert;
        Process process;

        //static extern bool SetForegroundWindow(IntPtr hWnd);


        public AlertAction() { }

        public AlertAction(Alert alert) {
            this.alert = alert;
        }

        public void closeProcess()
        {
                try
                {
                    this.process = Process.GetProcessesByName(alert.ProcessName).First();
                    process.Kill();
                } catch(Exception e)
                {
                    Debug.WriteLine(e.ToString());
                }
        }
        public void bringToFront()
        {
                    
            try
            {
                //this.process = Process.GetProcessesByName(alert.ProcessName).First();
                //IntPtr intPtr = process.MainWindowHandle;
                //SetForegroundWindow(intPtr);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }
    }
}
