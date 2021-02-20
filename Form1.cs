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
using System.Windows.Forms.VisualStyles;

namespace TyperHelper
{
    public partial class Form1 : Form
    {
        private MainHandler _mainHandler;//MainHandler példány
        
        public List<Process> processes;//Folyamatok
        public List<Process> processesVisible;//Látható folyamatok (amiknek vannak ablakai)
        public List<string> processNames;//Folyamatok nevei
        public Process selectedProcess = null;//Kiválasztott folyamat
        
        public List<string> text = new List<string>();//Beírt vagy betöltött szöveg
        public int latency = 0;//Késleltetés ezredmásodpercben
        public int allTextLength = 0;//Beírt vagy betöltött szöveg hossza
        public List<int> breaks = new List<int>();//Sortörések indexei

        public Form1()
        {
            InitializeComponent();
            chooseFile.FileName = "";
            _mainHandler = new MainHandler(this);
            checkStart();
        }

        /**
         * Látható folyamatok feltöltése a listába
         **/
        private void initLists()
        {
            processes = Process.GetProcesses().ToList();
            processesVisible = new List<Process>();
            processNames = new List<string>();
            foreach (Process p in processes)
                if(!string.IsNullOrEmpty(p.MainWindowTitle))
                {
                    processNames.Add("["+p.ProcessName.Substring(0,1).ToUpper()+p.ProcessName.Substring(1,p.ProcessName.Length-1)+"]: " + p.MainWindowTitle);
                    processesVisible.Add(p);
                }

            progresses.DataSource = processNames;
        }

        /**
         * Ha a fájlkiválasztásnál OK gombot nyom le, ez fut le
         **/
        private void chooseFile_FileOk(object sender, CancelEventArgs e)
        {
            if (!chooseFile.FileName.EndsWith(".txt"))
            {
                MessageBox.Show("Kérlek .txt kiterjesztésű fájlt válassz ki!", "Hiba!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                szovegInput.ReadOnly = true;
                textTypedCheck.Checked = true;
                szovegInput.Text = "Kiválaszott fájl:\n" + chooseFile.FileName;
            }
        }

        /**
         * Folyamat kiválasztva
         **/
        private void progresses_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (processesVisible != null){
                selectedProcess = processesVisible[progresses.SelectedIndex];
                processSelectedCheck.Checked = true;
                checkStart();
            }
        }

        /**
         * Ha rákattintunk a folyamat kiválasztásra, frissüljön a lista
         **/
        private void progresses_Click(object sender, EventArgs e)
        {
            initLists();
        }

        /**
         * UI engedélyezése vagy tiltása
         **/
        public void setButtons(bool enabled)
        {
            enterAfter.Enabled = enabled;
            enterBefore.Enabled = enabled;
            button1.Enabled = enabled;
            startButton.Enabled = enabled;
            progresses.Enabled = enabled;
            numericUpDown.Enabled = enabled;
        }

        /**
         * Írás indítása
         **/
        private void startButton_Click(object sender, EventArgs e)
        {
            latency = 0;
            allTextLength = 0;
            text = new List<string>();
            breaks = new List<int>();
            setButtons(false);

            latency = (int) numericUpDown.Value;
            
            if (chooseFile.FileName == "")
                text = szovegInput.Text.Split('\r').ToList();
            else
            {
                try 
                {
                    text = File.ReadAllText(chooseFile.FileName).Split('\r').ToList();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Hiba!",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
            for (int ind = 0; ind < text.Count; ind++)
            {
                for (int typed = 0; typed < text[ind].Length; typed++)
                    allTextLength++;
                breaks.Add(allTextLength);
            }

            progressBar1.Maximum = allTextLength;

            _mainHandler.write();
        }

        /**
         * Szöveg gépelése
         **/
        private void szovegInput_TextChanged(object sender, EventArgs e)
        {
            textTypedCheck.Checked = (szovegInput.Text.Length > 0);
            checkStart();
        }

        /**
         * Késleltetés változtatása
         **/
        private void numericUpDown_ValueChanged(object sender, EventArgs e)
        {
            latencyCheck.Checked = (numericUpDown.Value > 0);
            checkStart();
        }

        /**
         * Fájl kiválasztása... gomb
         **/
        private void button1_Click_1(object sender, EventArgs e)
        {
            chooseFile.ShowDialog();
        }

        /**
         * Követelmények ellenőrzése
         **/
        private void checkStart()
        {
            startButton.Enabled = (textTypedCheck.Checked && latencyCheck.Checked && processSelectedCheck.Checked);
        }
    }
}
