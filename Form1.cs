using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace TyperHelper
{
    public partial class Form1 : Form
    {
        private MainHandler _mainHandler;
        
        public List<Process> processes;
        public List<Process> processesVisible;
        public List<string> processNames;
        public Process selectedProcess = null;
        
        public string text = "";
        public int latency = 0;

        public Form1()
        {
            InitializeComponent();
            chooseFile.FileName = "";
            initLists();
            _mainHandler = new MainHandler(this);
        }

        private void initLists()
        {
            processes = Process.GetProcesses().ToList();
            processesVisible = new List<Process>();
            processNames = new List<string>();
            foreach (Process p in processes)
                if(!string.IsNullOrEmpty(p.MainWindowTitle))
                {
                    processNames.Add(p.ProcessName);
                    processesVisible.Add(p);
                }
            
            progresses.DataSource = processNames;
        }

        private void chooseFile_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chooseFile.ShowDialog();

            while (!chooseFile.FileName.EndsWith(".txt")) {
                MessageBox.Show("Kérlek .txt kiterjesztésű fájlt válassz ki!", "Hiba!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                chooseFile.ShowDialog();
            }

            szovegInput.ReadOnly = true;
            szovegInput.Text = "Kiválaszott fájl:\n" + chooseFile.FileName;
            label1.Visible = false;
        }

        private void progresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (processesVisible != null)
                selectedProcess = processesVisible[progresses.SelectedIndex];
        }

        private void progresses_Click(object sender, EventArgs e)
        {
            
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            latency = 0;
            startButton.Enabled = false;
            
            Int32.TryParse(textBox1.Text, out latency);

            MessageBox.Show(latency + "");

            if (latency != 0)
            {
                if (chooseFile.FileName == "")
                    if (szovegInput.Text == "") _mainHandler.noText();
                    else
                    {
                        text = szovegInput.Text; 
                        button1.Enabled = false;
                    }
                else
                {
                    try 
                    {
                        text = File.ReadAllText(chooseFile.FileName);
                        button1.Enabled = false;
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message, "Hiba!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                _mainHandler.write();
                startButton.Enabled = true;
            }else _mainHandler.latencyIsNull();
        }
    }
}
