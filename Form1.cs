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
        
        public List<string> text = new List<string>();
        public int latency = 0;
        public int allTextLength= 0;
        public List<int> breaks = new List<int>();

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
            allTextLength = 0;
            text = new List<string>();
            breaks = new List<int>();
            startButton.Enabled = false;
            _mainHandler.progressValue = 0;
            
            Int32.TryParse(textBox1.Text, out latency);

            if (latency != 0)
            {
                if (chooseFile.FileName == "")
                    if (szovegInput.Text == "") _mainHandler.noText();
                    else
                    {
                        text = szovegInput.Text.Split('\r').ToList(); 
                        for (int ind = 0; ind < text.Count; ind++)
                        {
                            for (int typed = 0; typed < text[ind].Length; typed++)
                            {
                                allTextLength++;
                            }
                            breaks.Add(allTextLength);
                        }
                        enterAfter.Enabled = false;
                        enterBefore.Enabled = false;
                        button1.Enabled = false;
                        startButton.Enabled = false;
                    }
                else
                {
                    try 
                    {
                        text = File.ReadAllLines(chooseFile.FileName).ToList();
                        for (int ind = 0; ind < text.Count; ind++)
                        {
                            for (int typed = 0; typed < text[ind].Length; typed++)
                            {
                                allTextLength++;
                            }
                        }
                        enterAfter.Enabled = false;
                        enterBefore.Enabled = false;
                        button1.Enabled = false;
                        startButton.Enabled = false;
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
