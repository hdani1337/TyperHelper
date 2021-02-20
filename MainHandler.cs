using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

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
        
        public void updateProgressBar()
        {            
            if (parent.text.Count != 0){
                parent.progressBar1.Invoke((Action) (() =>
                    parent.progressBar1.Increment(1)));
            }
        }

        int typedIndex = 0;
        long startTime = 0;
        long stopTime = 0;

        public void write()
        {
            typedIndex = 0;
            parent.progressBar1.Value = 0;
            startTime = DateTime.Now.Ticks;
            
            if(parent.enterBefore.Checked) pressEnter(false);
            Timer timer1 = new Timer    
            {    
                Interval = parent.latency   
            };    
            timer1.Enabled = true;    
            timer1.Tick += new System.EventHandler(writeTimerVoid);
        }

        private void writeTimerVoid(object sender, EventArgs e)
        {
            if (typedIndex < parent.allTextLength)
            {
                SetForegroundWindow(parent.selectedProcess.MainWindowHandle);
                if(getLetterByIndex(typedIndex) != '\n') pressKey(getLetterByIndex(typedIndex));
                else pressEnter(true);
                typedIndex++;
                updateProgressBar();
            }
            else
            {
                if(parent.enterAfter.Checked) pressEnter(false);
                parent.setButtons(true);
                parent.progressBar1.Value = parent.progressBar1.Maximum;
                ((Timer)sender).Stop();
                stopTime = DateTime.Now.Ticks;
                MessageBox.Show("A szöveg begépelése elkészült  "+Math.Round((stopTime-startTime)/10000000.0f,2)+" másodperc alatt!", "Kész", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private char getLetterByIndex(int index)
        {
            if (parent.breaks.Contains(index))
                return '\n';
            
            string allText = "";

            for (int ind = 0; ind < parent.text.Count; ind++)
            {
                for (int typed = 0; typed < parent.text[ind].Length; typed++)
                {
                    allText += parent.text[ind][typed];
                }
            }
            return allText[index];
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
        
        private void pressEnter(bool shift)
        {
            SetForegroundWindow(parent.selectedProcess.MainWindowHandle);
            if(shift) keybd_event(KeyCodes.VK_SHIFT, 0x52, KeyCodes.KEYEVENTF_KEYDOWN, 0);//hex 'A'
            keybd_event(KeyCodes.VK_RETURN, 0x52, KeyCodes.KEYEVENTF_KEYDOWN, 0);//hex 'A'
            keybd_event(KeyCodes.VK_RETURN, 0x52, KeyCodes.KEYEVENTF_KEYUP, 0);//hex 'A'
            if(shift) keybd_event(KeyCodes.VK_SHIFT, 0x52, KeyCodes.KEYEVENTF_KEYUP, 0);//hex 'A'
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