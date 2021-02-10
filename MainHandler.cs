using System;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms;

namespace TyperHelper
{
    public class MainHandler
    {
        private Form1 parent;
        
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, int Msg, char c, int lParam);
       
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        public static void SendKey(IntPtr hWnd, char c)
        {
            PostMessage(hWnd, 0x0100, c, 0);
            PostMessage(hWnd, 0x0101, c, 0);
        }
        
        public MainHandler(Form1 parent)
        {
            this.parent = parent;
        }

        public void latencyIsNull()
        {
            MessageBox.Show("Kérlek adj meg egy késleltetési időtartamot!", "Hiba!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void noText()
        {
            MessageBox.Show("Kérlek írj be egy szöveget, vagy válassz ki egy fájlt!", "Hiba!",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        int typedIndex = 0;

        public void write()
        {
            typedIndex = 0;
            foreach (char c in parent.text)
            {
                SetForegroundWindow(parent.selectedProcess.MainWindowHandle);
                pressKey(c);
                typedIndex++;
            }
        }

        private void pressKey(char c)
        {
            byte send = KeyCodes.getHungarianKeys(c);
            capsOn(c);
            
            SetForegroundWindow(parent.selectedProcess.MainWindowHandle);
            
            keybd_event(send, 0x52, KeyCodes.KEYEVENTF_KEYDOWN, 0);//hex 'A'
            keybd_event(send, 0x52, KeyCodes.KEYEVENTF_KEYUP, 0);//hex 'A'
            
            capsOn(c);
        }

        private void capsOn(char c)
        {
            if (c.ToString() == c.ToString().ToUpper())
            {
                keybd_event(KeyCodes.VK_CAPITAL, 0x52, KeyCodes.KEYEVENTF_KEYDOWN, 0);//hex 'A'
                keybd_event(KeyCodes.VK_CAPITAL, 0x52, KeyCodes.KEYEVENTF_KEYUP, 0);//hex 'A'
            }
        }
    }
}